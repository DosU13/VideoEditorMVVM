using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Models;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;

namespace VideoEditorMVVM.ViewModels
{
	public class CompositionViewModel : NotificationBase<CompositionService>
	{
		private CompositionService _compositionService;

		public CompositionViewModel()
		{
			_compositionService = new CompositionService();
		}

		public int Count
		{
			get { return This.composition.Clips.Count; }
        }

        public MediaSource Source
        {
            get
            {
                try
                {
                    MediaStreamSource mediaStreamSource = This.composition.GeneratePreviewMediaStreamSource(1000, 1000);
                    MainPage.status = "creating source";
                    return MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
                }catch (Exception ex)
                {
                    MainPage.status = "source creation error: "+ex.Message;
                    return null;
                }
            }
        }

        public IList<MediaClip> CmpClips
        {
            get { return This.composition.Clips; }
            set { SetProperty(This.composition.Clips, value, () =>
            {
                MainPage.status = "clips adding started";
                foreach(MediaClip clip in value) This.composition.Clips.Add(clip);
                RaiseProperty(ref This.composition, null);
                MainPage.status = "clips adding finished";
            }); }
        }

        internal async void AddFiles(List<StorageFile> files)
        {
            try
            {
                MainPage.status = "adding files started";
                This.composition.Clips.Clear();
                IList<MediaClip> clips = new List<MediaClip>();
                for (int i = 0; i < files.Count; i++)
                {
                    double t = 1;
                    MediaClip clip = await MediaClip.CreateFromImageFileAsync(files[i], TimeSpan.FromSeconds(t));
                    clips.Add(clip);
                    MainPage.status = "adding "+i+"'s file";
                }
                CmpClips = clips;
                //Render();
            }
            catch (Exception ex) { Console.WriteLine("Exception in composition controller: " + ex.Message); }
        }
    }
}
