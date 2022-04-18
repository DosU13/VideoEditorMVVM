using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Models;

namespace VideoEditorMVVM.ViewModels.Timeline
{
    public class TimelineViewModel : NotificationBase<TimelineModel>
    {
        public TimelineViewModel(TimelineModel timelineModel) : base(timelineModel)
        {
            MediaClips = new ObservableCollection<XMediaClip>(This.MediaClips);
            MediaClips.CollectionChanged += OnMediaClipsCollectionChanged;
        }

        public ObservableCollection<XMediaClip> MediaClips{ get; }

        public void OnMediaClipsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    This.MediaClips.Add((e.NewItems[0] as XMediaClip));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    This.MediaClips.Remove((e.OldItems[0] as XMediaClip));
                    break;
                default:
                    MainPage.Status = "LibraryVM collection change not fully implemented";
                    break;
            }
        }

        public void AddNewSequence()
        {
            MediaClips.Add(new XMediaClip());
        }
    }
}
