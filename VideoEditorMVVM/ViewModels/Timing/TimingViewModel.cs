using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Models;

namespace VideoEditorMVVM.ViewModels.Timing
{
    public class TimingViewModel : NotificationBase<TimingModel>
    {
        public TimingViewModel(TimingModel timingModel) : base(timingModel)
        { 
            Sequences = new ObservableCollection<TimingSequence>(This.Sequences);
            Sequences.CollectionChanged += OnSequencesCollectionChanged;
        }

        public string MusicPath
        {
            set { SetProperty(This.Music.Path, value, () => This.Music.Path = value); }
        }

        public string MusicName
        {
            get { return Path.GetFileName(This.Music.Path); }
        }

        public ObservableCollection<TimingSequence> Sequences { get; }

        private void OnSequencesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    This.Sequences.Add((e.NewItems[0] as TimingSequence));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    This.Sequences.Remove((e.OldItems[0] as TimingSequence));
                    break;
                default:
                    MainPage.Status = "LibraryVM collection change not fully implemented";
                    break;
            }
        }

        public void AddNewSequence()
        {
            Sequences.Add(new TimingSequence());
        }

        public void ImportFromMidiFile(MidiFile midiFile, string displayName)
        {
            This.ImportFromMidiFile(midiFile, displayName);
        }
    }
}
