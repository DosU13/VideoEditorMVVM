using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Data.Library;
using VideoEditorMVVM.Models;
using Windows.UI.Xaml.Controls;

namespace VideoEditorMVVM.ViewModels
{
    public class LiblaryViewModel: NotificationBase<LibraryModel>
    {
        public LiblaryViewModel(LibraryModel libraryModel): base(libraryModel)
        {
        }
        private void test(object sender, NotifyCollectionChangedEventArgs e)
        {
            MainPage.Status = "It is working";
        }

        public ObservableCollection<SingleMediaViewModel> SingleMedias { 
            get { 
                var res = new ObservableCollection<SingleMediaViewModel>();
                foreach(var single in This.SingleMedias)
                {
                    res.Add(new SingleMediaViewModel(single));
                }
                return res;
            } 
        }
        public ObservableCollection<GroupMediaViewModel> GroupMedias { 
            get {
                var res = new ObservableCollection<GroupMediaViewModel>();
                foreach (var group in This.GroupMedias)
                {
                    var gmvm =  new GroupMediaViewModel(group);
                    res.Add(gmvm);
                    gmvm.FilePaths.CollectionChanged += GroupMediasChanged;
                }
                res.CollectionChanged += test;
                return res;
            }
        }
        private void GroupMediasChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(GroupMedias));
        }

        public void AddSingleMedia()
        {
            This.AddSingleMedia();
            NotifyPropertyChanged(nameof(SingleMedias));
        }

        public void AddGroupMedia()
        {
            This.AddGroupMedia();
            NotifyPropertyChanged(nameof(GroupMedias));
        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (SingleMediaViewModel i in e.RemovedItems)
            {
                i.Selected = false;
            }
            foreach (SingleMediaViewModel i in e.AddedItems)
            {
                i.Selected = true;
            }
        }
    } 
}
