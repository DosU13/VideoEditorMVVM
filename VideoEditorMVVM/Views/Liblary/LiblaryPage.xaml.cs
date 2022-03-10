using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VideoEditorMVVM.Models;
using VideoEditorMVVM.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoEditorMVVM.Views.Liblary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LiblaryPage : Page
    {
        public LiblaryPage(LibraryModel libraryModel)
        {
            this.InitializeComponent();

            ViewModel = new LiblaryViewModel(libraryModel);
        }

        public LiblaryViewModel ViewModel { get; set; }

        private void onGroupsGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ItemsWrapGrid)GroupsGrid.ItemsPanelRoot).ItemWidth = e.NewSize.Width;
        }

        private GridView lastGrid = null;
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lastGrid!=null && lastGrid!=sender) lastGrid.SelectedItem = null;
            lastGrid = sender as GridView;
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (lastGrid == null) return;
            (GroupsGrid.ItemsSource as ObservableCollection<GroupMediaViewModel>).
                Remove(GroupsGrid.SelectedItem as GroupMediaViewModel);
        }
    }
}
