using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Models
{
    public class TimingModel
    {
        private TimingData TimingData { get; set; }
        public TimingModel(TimingData timingData)
        {
            TimingData = timingData;
        }

        public FilePathData Music { get { return TimingData.Music; } }
        public List<TimingSequence> Sequences { get { return TimingData.Sequences; } }

        public void ImportFromMidiFile(MidiFile midiFile, String displayName = "new track")
        {
            IEnumerable<Note> notes = midiFile.GetNotes();
            TempoMap tempoMap = midiFile.GetTempoMap();
            // I hope tempoMap will remains same all the time
            long? microsecondPerQuarterNote = tempoMap.GetTempoChanges()?.LastOrDefault()?.Value?.MicrosecondsPerQuarterNote;
            int? timeSignatureNumerator = tempoMap.GetTimeSignatureChanges()?.LastOrDefault()?.Value?.Numerator;
            int? timeSignatureDenominator = tempoMap.GetTimeSignatureChanges()?.LastOrDefault()?.Value?.Denominator;
            // I hope tempoMap will remains same all the time
            int timingSeqNameNumber = 1;
            foreach (TrackChunk trackChunk in midiFile.GetTrackChunks())
            {
                var trackNotes = trackChunk.GetNotes();
                using (var chordsManager = new ChordsManager(trackChunk.Events))
                {
                    ChordsCollection chords = chordsManager.Chords;
                    foreach (Chord chord in chords)
                    {
                        //Debug.WriteLine(chord.TimeAs<BarBeatFractionTimeSpan>(tempoMap) + " " + chord);
                    }
                }
                if (trackNotes.Count > 0)
                {
                    var timingSequence = new TimingSequence(1,
                        displayName + ((timingSeqNameNumber != 1) ? timingSeqNameNumber.ToString() : ""));
                    timingSeqNameNumber++;
                    if(microsecondPerQuarterNote.HasValue) timingSequence.MicrosecondsPerQuarterNote = microsecondPerQuarterNote.Value;
                    if(timeSignatureNumerator.HasValue) timingSequence.TimeSignatureNumerator = timeSignatureNumerator.Value;
                    if(timeSignatureDenominator.HasValue) timingSequence.TimeSignatureDenominator = timeSignatureDenominator.Value;
                    foreach (Note note in trackNotes)
                    {
                        MetricTimeSpan metricTime = note.TimeAs<MetricTimeSpan>(tempoMap);
                        BarBeatTicksTimeSpan musicalTime = note.TimeAs<BarBeatTicksTimeSpan>(tempoMap);

                        MetricTimeSpan metricLength = note.LengthAs<MetricTimeSpan>(tempoMap);
                        int id = note.NoteNumber;
                        //Debug.WriteLine("note: " + note + " " + id + " " + getNoteFromMidiNumber(id) + " " + note.Time + " " +
                        //     note.TimeAs<BarBeatFractionTimeSpan>(tempoMap) +" "+metricTime.TotalMicroseconds/1000000.0 +" "+
                        //     TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap).TotalMicroseconds/1000000.0);

                        TimingItem item = new TimingItem(metricTime.TotalMicroseconds, metricLength.TotalMicroseconds, note.NoteNumber, note.Velocity, note.OffVelocity);
                        timingSequence.Timings.Add(item);
                    }
                    Sequences.Add(timingSequence);
                }
            }
        }
    }
}
