using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using Windows.Storage;

namespace VideoEditorMVVM.Models
{
    public class TimelineModel
    {
        private ProjectData ProjectData { get;}
        public TimelineModel(ProjectData projectData)
        {
            ProjectData = projectData;
        }
        public List<XMediaClip> MediaClips => ProjectData.Timeline.MediaClips;
        public List<CmpMediaClip> CmpMediaClips
        { 
            get
            {
                List<CmpMediaClip> result = new List<CmpMediaClip>();
                foreach (XMediaClip clip in MediaClips)
                {
                    string str = clip.Timing;
                    if (str == null || str == "")
                    {
                        long time = long.Parse(clip.StartTime);
                        long length = long.Parse(clip.LengthTime);
                        result.Add(new CmpMediaClip(GetFilePath(clip), 
                            new TimeSpan(time*10), new TimeSpan(10*length)));
                    }
                    else {
                        var timings = ProjectData.Timing.Sequences.Find(it => it.Name == str)?.Timings??null;
                        if (timings == null)
                        {
                            MainPage.Status = "can't find timing with name " + str + " ";
                        }
                        else
                        {
                            for (int i = 0; i < timings.Count; i++)
                            {
                                long time;
                                if (long.TryParse(clip.StartTime, out long _time)) time = _time;
                                else
                                {
                                    time = (long)(timings[i].GetType().GetProperty(clip.StartTime)?.GetValue(timings[i]) ?? 0L);
                                }
                                long length;
                                if (long.TryParse(clip.LengthTime, out long _length)) length = _length;
                                else
                                {
                                    length = (long)(timings[i].GetType().GetProperty(clip.LengthTime)?.GetValue(timings[i]) ?? 0L);
                                }
                                result.Add(new CmpMediaClip(GetFilePath(clip, timings[i], i),
                                    new TimeSpan(time * 10), new TimeSpan(length * 10)));
                            }
                        }
                    }
                }
                return result;
            }
        }

        public string GetFilePath(XMediaClip media, TimingItem timing = null, int timingIndex=-1)
        {
            MediaBase mediaBase = ProjectData.Library.Medias.Find( it => it.Name == media.Media);
            if (mediaBase == null)
            {
                MainPage.Status = "Media doesn't dound";
                return null;
            }
            string path;
            if (mediaBase is MediaSingle) path = (mediaBase as MediaSingle).File.Path;
            else
            {
                MediaGroup mediaGroup = mediaBase as MediaGroup;
                string str = media.MediaGroupIndex;
                int groupInd;
                if (!int.TryParse(str, out groupInd) 
                    && str == "index") groupInd = timingIndex % mediaGroup.Files.Count;
                else groupInd = 0;// HERE else TimingUtils.getValueFrom(Timing, propertyName)
                path = (mediaBase as MediaGroup).Files[groupInd].Path;
            }
            return path;
        }

        public TimeSpan GetDuration(int clipInd, int timingInd)
        {
            return TimeSpan.FromSeconds(5);
        }

        public string MusicPath => ProjectData.Timing.Music.Path;
    }
}
