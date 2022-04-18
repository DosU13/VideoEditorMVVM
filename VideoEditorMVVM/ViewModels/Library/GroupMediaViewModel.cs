using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using Windows.UI.Xaml;

namespace VideoEditorMVVM.ViewModels
{
    public class GroupMediaViewModel: NotificationBase<MediaGroup>
    {
        public GroupMediaViewModel(MediaGroup mediaGroup) : base(mediaGroup)
        {
            FilePaths = new ObservableCollection<FilePathData>(This.Files);
            FilePaths.CollectionChanged += OnCollectionChanged;
        }

        public int Id { get { return This.Id; } }
        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public ObservableCollection<FilePathData> FilePaths { get; }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MainPage.Status = "Collection: "+e.Action.ToString();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    This.Files.Insert(e.NewStartingIndex, e.NewItems[0] as FilePathData);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    This.Files.RemoveAt(e.OldStartingIndex);
                    break;
                //case NotifyCollectionChangedAction.Move:
                //    FilePathData item = This.Files[e.OldStartingIndex];
                //    This.Files.RemoveAt(e.OldStartingIndex);
                //    This.Files.Insert(e.NewStartingIndex, item);
                //    break;
                //case NotifyCollectionChangedAction.Replace:
                //    This.Files.Insert(e.NewStartingIndex, e.NewItems[0] as FilePathData);
                //    break;
                //case NotifyCollectionChangedAction.Reset:
                //    This.Files.Clear();
                //    break;
                default:
                    MainPage.Status = "GroupMedia collection change not fully implemented";
                    break;
            }
        }


        public void AddItem_Click(object sender, RoutedEventArgs e)
        {
            FilePaths.Add(new FilePathData());
            //GroupMediasChanged.Invoke(sender, null);
        }

        public bool IsTemplate
        {
            get { return This.IsTemplate; }
            set
            {
                SetProperty(This.IsTemplate, value, () => This.IsTemplate = value);
            }
        }
    }
}
