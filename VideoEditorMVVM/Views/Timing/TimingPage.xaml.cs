using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Models;
using VideoEditorMVVM.ViewModels;
using VideoEditorMVVM.ViewModels.Timing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoEditorMVVM.Views.Timing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimingPage : Page
    {
        public TimingViewModel ViewModel { get; set; }
     
        public TimingPage(TimingModel timingModel)
        {
            this.InitializeComponent();
            ViewModel = new TimingViewModel(timingModel);
        }

        private async void ChangeMusicBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wav");

            var file = await picker.PickSingleFileAsync();
            ViewModel.MusicPath = file?.Path;
            Bindings.Update();

            // These files could be picked from a location that we won't have access to later
            var storageItemAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
            storageItemAccessList.Add(file);
        }
        private async void ImportMidi_Clicked(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".midi");
            picker.FileTypeFilter.Add(".mid");

            var file = await picker.PickSingleFileAsync();
            if (file == null) return; // pick cancelled
            
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                ViewModel.ImportFromMidiFile(MidiFile.Read(fileStream.AsStream()), file.DisplayName);
            }
            Bindings.Update();
        }

        private void DeleteSequence_Clicked(object sender, RoutedEventArgs e)
        {
            ViewModel.Sequences.Remove(SelectedSequence.TimingSequence);
            SelectedSequence = null;
            Bindings.Update();
        }

        private SequenceModel SelectedSequence = null;

        private void SequenceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (sender as ListView).SelectedItem as TimingSequence;
            if (selected != null) SelectedSequence = new SequenceModel(selected);
            Bindings.Update();
        }

        private async void BindingsUpdate(object sender, RoutedEventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(200);
            Bindings.Update();
        }

        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem firstItem = new MenuFlyoutItem { Text = "First item" };
            MenuFlyoutItem secondItem = new MenuFlyoutItem { Text = "Second item" };
            MenuFlyoutSubItem subItem = new MenuFlyoutSubItem { Text = "Other items" };
            MenuFlyoutItem item1 = new MenuFlyoutItem { Text = "First sub item" };
            MenuFlyoutItem item2 = new MenuFlyoutItem { Text = "Second sub item" };
            subItem.Items.Add(item1);
            subItem.Items.Add(item2);
            myFlyout.Items.Add(firstItem);
            myFlyout.Items.Add(secondItem);
            myFlyout.Items.Add(subItem);
            FrameworkElement senderElement = sender as FrameworkElement;
            myFlyout.ShowAt(senderElement);
        }

        private async void SyncBtn_Pressed(object sender, RoutedEventArgs e)
        {
            TextBox input = new TextBox()
            {
                Height = (double)App.Current.Resources["TextControlThemeMinHeight"],
                PlaceholderText = "Display Text"
            };
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Input Dialog",
                MaxWidth = this.ActualWidth,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel",
                Content = input
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                input = (TextBox)dialog.Content;
                long val = (long)(double.Parse(input.Text)*1000000.0);
                // Here starts alligning all

                foreach(TimingItem t in SelectedSequence.TimingSequence.Timings)
                {
                    t.TimeMicroSec += val;
                }
            }
        }

        private void MelodiseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            TimingSequence sel = SelectedSequence.TimingSequence;
            List<TimingItem> timings = new List<TimingItem>();
            timings.Add(sel.Timings[0]);
            foreach(TimingItem t in sel.Timings)
            {
                TimingItem last = timings.Last();
                if (t.TimeMicroSec != last.TimeMicroSec) timings.Add(t);
                else
                {
                    if(last.NoteNumber < t.NoteNumber)
                    {
                        timings.Remove(last);
                        timings.Add(t);
                    }
                }
            }
            TimingSequence newItem = new TimingSequence(sel.Id, sel.Name+"_melo");
            newItem.MicrosecondsPerQuarterNote = sel.MicrosecondsPerQuarterNote;
            newItem.TimeSignatureNumerator = sel.TimeSignatureNumerator;
            newItem.TimeSignatureDenominator = sel.TimeSignatureDenominator;
            newItem.Timings = timings;
            ViewModel.This.Sequences.Add(newItem);
        }

        private void NormalizeBtn_Clicked(object sender, RoutedEventArgs e)
        {
            TimingSequence sel = SelectedSequence.TimingSequence;
            List<TimingItem> timings = new List<TimingItem>();
            List<TimingItem> timingIter = new List<TimingItem>();
            long barIter = 0;
            foreach (TimingItem t in sel.Timings)
            {
                var metric = new MetricTimeSpan(t.TimeMicroSec);
                var barTime = TimeConverter.ConvertTo<BarBeatTicksTimeSpan>(metric, SequenceModel.TempoMap);
                long bar = barTime.Bars;
                if (bar == barIter) timingIter.Add(t);
                else
                {
                    if (timingIter.Any())
                    {
                        int max = timingIter.Max(i => i.NoteNumber).Value;
                        int min = timingIter.Min(i => i.NoteNumber).Value;
                        foreach (TimingItem i in timingIter)
                        {
                            if (min == max) i.NoteNumber = 64;
                            else i.NoteNumber = (128 * (i.NoteNumber - min)) / (max - min);
                        }
                        timings.AddRange(timingIter);
                    }
                    barIter = bar;
                    timingIter = new List<TimingItem>();
                }
            }
            if (timingIter.Any())
            {
                int max = timingIter.Max(i => i.NoteNumber).Value;
                int min = timingIter.Min(i => i.NoteNumber).Value;
                foreach (TimingItem i in timingIter)
                {
                    if (min == max) i.NoteNumber = 64;
                    else i.NoteNumber = (128 * (i.NoteNumber - min)) / (max - min);
                }
                timings.AddRange(timingIter);
            }
            TimingSequence newItem = new TimingSequence(sel.Id, sel.Name + "_norm");
            newItem.MicrosecondsPerQuarterNote = sel.MicrosecondsPerQuarterNote;
            newItem.TimeSignatureNumerator = sel.TimeSignatureNumerator;
            newItem.TimeSignatureDenominator = sel.TimeSignatureDenominator;
            newItem.Timings = timings;
            ViewModel.This.Sequences.Add(newItem);
        }

        private async void ExportBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("MuzU file", new List<string>() { ".MuzU" });
            picker.SuggestedFileName = "MuzU file.MuzU";
            try
            {
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    TimingSequence sel = SelectedSequence.TimingSequence;
                    var root = new XElement("MuzU");
                    root.Add(new XAttribute("name", sel.Name));
                    root.Add(sel.ToXElement());
                    XDocument doc = new XDocument(root);
                    doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        await System.Threading.Tasks.Task.Run(() => {
                            stream.SetLength(0);
                            doc.Save(stream);
                            stream.Close();
                        });
                    }
                    MainPage.Status = (file.Name + " Succesfully saved"); // This must be inside task
                }
            }
            catch (Exception ex)
            {
                MainPage.Status = ex.ToString();
            }
        }
    }
}
