using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using Windows.UI.Xaml.Controls;

namespace VideoEditorMVVM.Models
{
    public class SequenceModel
    {
        public readonly TimingSequence TimingSequence;
        public static TempoMap TempoMap = null;
        public SequenceModel(TimingSequence timingSequence) 
        { 
            TimingSequence = timingSequence;
            TempoMap = TempoMap.Create(new Tempo(timingSequence.MicrosecondsPerQuarterNote),
                    new TimeSignature(timingSequence.TimeSignatureNumerator, timingSequence.TimeSignatureDenominator));
        }

        private List<TimingItem> timings { get => TimingSequence.Timings; }

        private void ArrangeListCount(int count)
        {
            Debug.WriteLine("ArrangeListCount" + count + " " + timings.Count);
            if (count < timings.Count)
            {
                timings.RemoveRange(count, timings.Count - count);
            }
            else if (count > timings.Count)
            {
                int countLack = count - timings.Count;
                for (int i = 0; i < countLack; i++) timings.Add(new TimingItem());
            }
        }

        public double BeatsPerMinute
        {
            get => new Tempo(TimingSequence.MicrosecondsPerQuarterNote).BeatsPerMinute;
            set => TimingSequence.MicrosecondsPerQuarterNote = Tempo.FromBeatsPerMinute(value).MicrosecondsPerQuarterNote;
        }
        public int TimeSignatureNumerator
        {
            get => TimingSequence.TimeSignatureNumerator;
            set => TimingSequence.TimeSignatureNumerator = value;
        }
        public int TimeSignatureDenominator
        {
            get => TimingSequence.TimeSignatureDenominator;
            set => TimingSequence.TimeSignatureDenominator = value;
        }

        public List<string> TimeDropDownList = new List<string>{"Time(sec)", "Time(bars)", "DeltaTime(sec)", "DeltaTime(sec)"};
        public static int _TimeDropDownIndex = 0;
        public int TimeDropDownIndex
        { 
            get { return _TimeDropDownIndex; }
            set { _TimeDropDownIndex = value; }
        }
        public string Time
        {
            get
            {
                var joinedNames = new StringBuilder();
                timings.ForEach(a => joinedNames.Append((joinedNames.Length > 0 ? "\r" : "") + a.TimeMicroSec));
                return TimeDropDownList[_TimeDropDownIndex];
                return joinedNames.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    long[] times = Array.ConvertAll(lines, long.Parse);
                    ArrangeListCount(times.Length);
                    for (int i = 0; i < times.Length; i++)
                    {
                        timings[i].TimeMicroSec = times[i];
                    }
                }
                catch (Exception ex) { MainPage.Status = "TimeMicroSecs are inappropriate: " + ex.Message; }
            }
        }

        public string DeltaTime
        {
            get
            {
                Debug.WriteLine(nameof(DeltaTime));
                if (TimingSequence == null) return "Select";
                var strBuil = new StringBuilder();
                for (int i = 0; i < timings.Count - 1; i++)
                {
                    strBuil.Append((timings[i + 1].TimeMicroSec - timings[i].TimeMicroSec));
                    if (i != timings.Count - 2) strBuil.Append("\r");
                }
                return strBuil.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    long[] deltaTimes = Array.ConvertAll(lines, long.Parse);
                    ArrangeListCount(deltaTimes.Length + 1);
                    long temp = timings[0].TimeMicroSec;
                    for (int i = 0; i < deltaTimes.Length; i++)
                    {
                        temp += deltaTimes[i];
                        timings[i + 1].TimeMicroSec = temp;
                    }
                }
                catch (Exception ex) { MainPage.Status = "DeltaTimeMicroSecs are inappropriate: " + ex.Message; }
            }
        }

        public List<string> LengthDropDownList = new List<string> { "Length(sec)", "Length(bars)"};
        private int lengthDropDownIndex = 0;
        public int LengthDropDownIndex
        {
            get { return lengthDropDownIndex; }
            set { lengthDropDownIndex = value; }
        }
        public string Length
        {
            get
            {
                Debug.WriteLine(nameof(Length));
                if (TimingSequence == null) return "Select";
                var joinedNames = new StringBuilder();
                timings.ForEach(a => joinedNames.Append((joinedNames.Length > 0 ? "\r" : "") + a.LengthMicroSec));
                return joinedNames.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    long[] times = Array.ConvertAll(lines, long.Parse);
                    ArrangeListCount(times.Length);
                    for (int i = 0; i < times.Length; i++)
                    {
                        timings[i].LengthMicroSec = times[i];
                    }
                }
                catch (Exception ex) { MainPage.Status = "LengthMicroSecs are inappropriate: " + ex.Message; }
            }
        }

        public List<string> NoteDropDownList = new List<string> { "Note", "NoteNumber" };
        private int noteDropDownIndex = 0;
        public int NoteDropDownIndex
        {
            get { return noteDropDownIndex; }
            set { noteDropDownIndex = value; }
        }
        public string NoteNumber
        {
            get
            {
                Debug.WriteLine(nameof(NoteNumber));
                if (TimingSequence == null) return "Select";
                var strBuild = new StringBuilder();
                timings.ForEach(a => strBuild.Append((strBuild.Length > 0 ? "\r" : "") + a.NoteNumber));
                return strBuild.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    int[] notes = Array.ConvertAll(lines, int.Parse);
                    for (int i = 0; i < timings.Count; i++)
                    {
                        if (i < notes.Count()) timings[i].NoteNumber = notes[i];
                        else timings[i].NoteNumber = null;
                    }
                }
                catch (Exception ex) { MainPage.Status = "NoteNumbers are inappropriate: " + ex.Message; }
            }
        }

        public string Velocity
        {
            get
            {
                Debug.WriteLine(nameof(Velocity));
                if (TimingSequence == null) return "Select";
                var strBuild = new StringBuilder();
                timings.ForEach(a => strBuild.Append((strBuild.Length > 0 ? "\r" : "") + a.Velocity));
                return strBuild.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    int[] notes = Array.ConvertAll(lines, int.Parse);
                    for (int i = 0; i < timings.Count; i++)
                    {
                        if (i < notes.Count()) timings[i].Velocity = notes[i];
                        else timings[i].Velocity = null;
                    }
                }
                catch (Exception ex) { MainPage.Status = "Velocitys are inappropriate: " + ex.Message; }
            }
        }

        public string OffVelocity
        {
            get
            {
                Debug.WriteLine(nameof(OffVelocity));
                if (TimingSequence == null) return "Select";
                var strBuild = new StringBuilder();
                timings.ForEach(a => strBuild.Append((strBuild.Length > 0 ? "\r" : "") + a.OffVelocity));
                return strBuild.ToString();
            }
            set
            {
                try
                {
                    string[] lines = value.Split('\r');
                    int[] notes = Array.ConvertAll(lines, int.Parse);
                    for (int i = 0; i < timings.Count; i++)
                    {
                        if (i < notes.Count()) timings[i].OffVelocity = notes[i];
                        else timings[i].OffVelocity = null;
                    }
                }
                catch (Exception ex) { MainPage.Status = "OffVelocitys are inappropriate: " + ex.Message; }
            }
        }
    }
}
