using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
            set { 
                if(_status!=null) _status.Text = value.Replace("\r\n", "->") + new Random().Next();
                Debug.WriteLine(value);
            }
        }

        private Repository Repository { get; }
        public MainPage(Repository repository)
        {
            this.InitializeComponent();
            Repository = repository;
            _status = StatusTextBlock;
            //Composition = new CompositionViewModel();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(1000); //Later you should replace it with repository load complete listener
            NavView.SelectedItem = NavView.MenuItems[3];
            ContentFrame.Content = new TimingPage(Repository.TimingModel);
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
                    ContentFrame.Content = new TimingPage(Repository.TimingModel);
                    break;
                case "liblary":
                    ContentFrame.Content = new LiblaryPage(Repository.LibraryModel);
                    break;
                case "timeline":
                    ContentFrame.Content = new TimelinePage(Repository.TimelineModel);
                    break;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("XML file", new List<string>() { ".xml" });
            picker.FileTypeChoices.Add("VidU file", new List<string>() { ".VidU" });
            picker.SuggestedFileName = "VidU test.VidU";
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
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".xml");
            picker.FileTypeFilter.Add(".VidU");
            try
            {
                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    XDocument doc = null;
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        doc = await XDocument.LoadAsync(stream, LoadOptions.None, System.Threading.CancellationToken.None);
                        Repository.LoadFromXDoc(doc);
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame?.Navigate(typeof(MainPage));
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
            double navWidth = NavView.ActualWidth - 170;
            double navItemsWid = LiblaryNavItem.ActualWidth + TimeLineNavItem.ActualWidth +
                TimingNavItem.ActualWidth;
            double calcedWid = (navWidth - navItemsWid) / 2;
            if (calcedWid > 0) CenterNavItemsMargin = (int)calcedWid;
            else CenterNavItemsMargin = 0;
        }
    }
}
