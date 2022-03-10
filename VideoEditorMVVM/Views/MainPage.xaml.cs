using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Models;
using VideoEditorMVVM.ViewModels;
using VideoEditorMVVM.Views.Liblary;
using VideoEditorMVVM.Views.Timeline;
using VideoEditorMVVM.Views.Timing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoEditorMVVM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static TextBlock _status = null;
        public static string Status
        {
            set { if(_status!=null) _status.Text = value.Replace("\r\n", "->") + new Random().Next(); }
        }

        private Repository Repository;
        public MainPage(Repository repository)
        {
            this.InitializeComponent();

            Repository = repository;

            _status = StatusTextBlock;
            //Composition = new CompositionViewModel();
        }


        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                //ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "timing":
                    ContentFrame.Navigate(typeof(TimingPage));
                    break;

                case "liblary":
                    ContentFrame.Content = new LiblaryPage(Repository.LibraryModel);
                    break;

                case "timeline":
                    ContentFrame.Navigate(typeof(TimelinePage));
                    break;
            }
        }


        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var files = (await picker.PickMultipleFilesAsync()).ToList();
            if (files.Count > 0)
            {
                //Composition.AddFiles(files);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeChoices.Add("XML files", new List<string>() { ".xml" });
            picker.SuggestedFileName = "XML test file.xml";
            try
            {
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    XDocument doc = Repository.ExportToXDoc();
                    using (var stream = await file.OpenStreamForWriteAsync())
                    { await System.Threading.Tasks.Task.Run(() => {
                        stream.SetLength(0);
                        doc.Save(stream);
                        stream.Close();
                    }); }
                    Status = (file.Name + " Succesfully saved"); // This must be inside task
                }
            }
            catch (Exception ex) { 
                Status = ex.ToString();
            }
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".xml");
            try
            {
                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    XDocument doc = null;
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        await System.Threading.Tasks.Task.Run(() => {
                            doc = XDocument.Load(stream);
                            Repository.LoadFromXDoc(doc);
                        });
                    }
                    if (doc!=null) Status = (file.Name + " Succesfully loaded"); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HERE: -->>" + ex.ToString());
                Status = "MainPage"+ex.ToString();
            }
        }

        private int centerNavItemsMargin = 0;
        public int CenterNavItemsMargin
        {
            get => centerNavItemsMargin;
            set
            {
                centerNavItemsMargin = value;
                Bindings.Update();
            }
        }

        private void CenterNavItems()
        {

            double navWidth = NavView.ActualWidth;
            double navItemsWid = LiblaryNavItem.ActualWidth + TimeLineNavItem.ActualWidth +
                TimingNavItem.ActualWidth;
            double calcedWid = (navWidth - navItemsWid) / 2;
            if (calcedWid > 0) CenterNavItemsMargin = (int)calcedWid;
            else CenterNavItemsMargin = 0;
        }

    }
}
