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
using VideoEditorMVVM.Models;
using Windows.UI.Xaml.Controls;

namespace VideoEditorMVVM.ViewModels
{
    public class LiblaryViewModel: NotificationBase<LibraryModel>
    {
        public LiblaryViewModel(LibraryModel libraryModel): base(libraryModel)
        {
            SingleMedias = new ObservableCollection<SingleMediaViewModel>();
            foreach (var single in This.Medias
                        .Where(i => { return i is MediaSingle; })
                        .Select(i => { return i as MediaSingle; }))
            {
                SingleMedias.Add(new SingleMediaViewModel(single));
            }
            GroupMedias = new ObservableCollection<GroupMediaViewModel>();
            foreach (var group in This.Medias
                        .Where(i => { return i is MediaGroup; })
                        .Select(i => { return i as MediaGroup; }))
            {
                GroupMedias.Add(new GroupMediaViewModel(group));
            }
            SingleMedias.CollectionChanged += OnSingleMediasCollectionChanged;
            GroupMedias.CollectionChanged += OnGroupMediasCollectionChanged;
        }

        public ObservableCollection<SingleMediaViewModel> SingleMedias { get; }
        public ObservableCollection<GroupMediaViewModel> GroupMedias { get; }
        private void OnSingleMediasCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    This.Medias.Add((e.NewItems[0] as SingleMediaViewModel).This);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    This.Medias.Remove((e.OldItems[0] as SingleMediaViewModel).This);
                    break;
                default:
                    MainPage.Status = "LibraryVM collection change not fully implemented";
                    break;
            }
        }
        private void OnGroupMediasCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    This.Medias.Add((e.NewItems[0] as GroupMediaViewModel).This);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    This.Medias.Remove((e.OldItems[0] as GroupMediaViewModel).This);
                    break;
                default:
                    MainPage.Status = "LibraryVM collection change not fully implemented";
                    break;
            }
        }

        public void AddSingleMedia()
        {
            SingleMedias.Add(new SingleMediaViewModel(new MediaSingle()));
        }

        public void AddGroupMedia()
        {
            GroupMedias.Add(new GroupMediaViewModel(new MediaGroup()));
        }
    } 
}
