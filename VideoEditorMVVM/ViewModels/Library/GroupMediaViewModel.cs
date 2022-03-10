using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Data.Library;
using Windows.UI.Xaml;

namespace VideoEditorMVVM.ViewModels
{
    public class GroupMediaViewModel: NotificationBase<MediaGroup>
    {
        public GroupMediaViewModel(MediaGroup mediaGroup) : base(mediaGroup)
        {
        }

        public int Id { get { return This.Id; } }
        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public ObservableCollection<FilePathData> FilePaths
        {
            get
            {
                return new ObservableCollection<FilePathData>(This.Files);
            }
        }

        public void AddItem_Click(object sender, RoutedEventArgs e)
        {
            This.Files.Add(new FilePathData());
            NotifyPropertyChanged(nameof(FilePaths));
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
