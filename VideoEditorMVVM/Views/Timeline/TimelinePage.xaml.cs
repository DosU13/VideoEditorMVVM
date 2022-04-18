using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Models;
using VideoEditorMVVM.ViewModels;
using VideoEditorMVVM.ViewModels.Timeline;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoEditorMVVM.Views.Timeline
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimelinePage : Page
    {
        public TimelinePage(TimelineModel timelineModel)
        {
            this.InitializeComponent();
            ViewModel = new TimelineViewModel(timelineModel);
            CompositionModel = new CompositionModel(timelineModel);
            CompositionModel.ChangeRender += UpdateMediaElementSource;
            if (timelineModel.MediaClips.Count == 0) timelineModel.MediaClips.Add(new XMediaClip()); 
            SelectedMediaClipVM = new MediaClipViewModel(ViewModel.MediaClips[0]);
        }

        public TimelineViewModel ViewModel { get; }
        private MediaClipViewModel selectedMediaClipVM;
        public MediaClipViewModel SelectedMediaClipVM { get => selectedMediaClipVM;
            set
            {
                selectedMediaClipVM = value;
                if (value == null) return;
                selectedMediaClipVM.PropertyChanged += (sender, e) => CompositionModel.OnChange();
                CompositionModel.OnChange();
                Bindings.Update();
            }
        }
        public CompositionModel CompositionModel { get; }

        private MediaStreamSource mediaStreamSource;
        public void UpdateMediaElementSource(object sender, EventArgs arg)
        {
            mediaStreamSource = CompositionModel.composition.GeneratePreviewMediaStreamSource(
                (int)mediaPlayerElement.ActualWidth,
                (int)mediaPlayerElement.ActualHeight);
            mediaPlayerElement.Source = MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Timeline_Grid.SelectedItem != null)
            {
                SelectedMediaClipVM = new MediaClipViewModel(Timeline_Grid.SelectedItem as XMediaClip);
            }else SelectedMediaClipVM = null;
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (Timeline_Grid.SelectedItem != null)
            {
                ViewModel.MediaClips.Remove(Timeline_Grid.SelectedItem as XMediaClip);
            }
        }
    }
}
