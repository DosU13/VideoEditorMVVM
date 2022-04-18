using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using Windows.Foundation;
using Windows.Media.Editing;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;

namespace VideoEditorMVVM.Models
{
    public class CompositionModel
    {
        public MediaComposition composition = new MediaComposition();
        public TimelineModel TimelineModel;

        public CompositionModel(TimelineModel timelineModel)
        {
            TimelineModel = timelineModel;
        }

        public async void Update()
        {
            try
            {
                composition.Clips.Clear();
                await SetMusicAsync(TimelineModel.MusicPath);

                Rect overlayPosition;
                overlayPosition.Width = 900;
                overlayPosition.Height = 500;
                overlayPosition.X = 0;
                overlayPosition.Y = 0;

                MediaOverlayLayer mediaOverlayLayer = new MediaOverlayLayer();

                foreach (CmpMediaClip clip in TimelineModel.CmpMediaClips)
                {
                    if(clip.Length.Ticks == 0) continue;
                    Debug.WriteLine(clip.FilePath);
                    MediaClip overlayMediaClip = await MediaClip.CreateFromImageFileAsync(
                        await StorageFile.GetFileFromPathAsync(clip.FilePath), clip.Length);

                    MediaOverlay mediaOverlay = new MediaOverlay(overlayMediaClip);
                    mediaOverlay.Position = overlayPosition;
                    mediaOverlay.Opacity = 1.0;
                    mediaOverlay.Delay = clip.Time;
                    mediaOverlayLayer.Overlays.Add(mediaOverlay);
                }
                composition.OverlayLayers.Add(mediaOverlayLayer);
                Render();
            }
            catch (Exception ex) { MainPage.Status = "Exception in composition controller: " + ex; }
        }

        public event EventHandler ChangeRender;
        public void Render()
        {
            ChangeRender?.Invoke(this, new EventArgs() { });
        }

        public void OnChange()
        {
            Update();
        }

        public async Task SetMusicAsync(string filePath)
        {
            StorageFile audioFile = await StorageFile.GetFileFromPathAsync(filePath);
            var backgroundTrack = await BackgroundAudioTrack.CreateFromFileAsync(audioFile);
            composition.BackgroundAudioTracks.Add(backgroundTrack);
            composition.Clips.Add(MediaClip.CreateFromColor(Colors.Aquamarine, backgroundTrack.OriginalDuration));
        }

        public CoreDispatcher Dispatcher { get; private set; } = null;

        internal async void RenderCompositionToFile()
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeChoices.Add("MP4 files", new List<string>() { ".mp4" });
            picker.SuggestedFileName = "RenderedComposition.mp4";

            try
            {
                if (Dispatcher == null) Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    // Call RenderToFileAsync
                    var saveOperation = composition.RenderToFileAsync(file, MediaTrimmingPreference.Precise);

                    saveOperation.Progress = new AsyncOperationProgressHandler<TranscodeFailureReason, double>(async (info, progress) =>
                    {
                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            MainPage.Status = string.Format("Saving file... Progress: {0:F0}%", progress);
                        }));
                    });
                    saveOperation.Completed = new AsyncOperationWithProgressCompletedHandler<TranscodeFailureReason, double>(async (info, status) =>
                    {
                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            try
                            {
                                var results = info.GetResults();
                                if (results != TranscodeFailureReason.None || status != AsyncStatus.Completed)
                                {
                                    MainPage.Status = "Saving was unsuccessful";
                                }
                                else
                                {
                                    MainPage.Status = "Trimmed clip saved to file";
                                }
                            }
                            finally
                            {
                                // Update UI whether the operation succeeded or not
                            }

                        }));
                    });
                }
                else
                {
                    MainPage.Status = "User cancelled the file selection";
                }
            }
            catch (Exception ex) { MainPage.Status = "Exception in rendering to file: " + ex.Message; }
        }
    }
}
