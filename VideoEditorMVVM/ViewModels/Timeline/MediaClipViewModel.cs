using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.ViewModels.Timeline
{
    public class MediaClipViewModel : NotificationBase<XMediaClip>
    {
        public MediaClipViewModel(XMediaClip uMediaClip) : base(uMediaClip){}

        public string Timing
        {
            get { return This.Timing; }
            set { SetProperty(This.Timing, value, () => This.Timing = value); }
        }

        public string StartTime
        {
            get { return This.StartTime; }
            set { SetProperty(This.StartTime, value, () => This.StartTime = value); }
        }

        public string LengthTime
        {
            get { return This.LengthTime; }
            set { SetProperty(This.LengthTime, value, () => This.LengthTime = value); }
        }

        public string TrimTimeFromStart
        {
            get { return This.TrimTimeFromStart; }
            set { SetProperty(This.TrimTimeFromStart, value, () => This.TrimTimeFromStart = value); }
        }

        public string Media
        {
            get { return This.Media; }
            set { SetProperty(This.Media, value, () => This.Media = value); }
        }

        public string MediaGroupIndex
        {
            get { return This.MediaGroupIndex; }
            set { SetProperty(This.MediaGroupIndex, value, () => This.MediaGroupIndex = value); }
        }
    }
}
