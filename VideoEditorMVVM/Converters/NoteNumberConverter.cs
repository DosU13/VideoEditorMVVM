using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VideoEditorMVVM.Converters
{
    public class NoteNumberConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a DateTime object to 
        // a month string.
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            if (!(value is int)) return "null";
            else return getNoteFromMidiNumber((int)value);
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            return getNumberFromNote((string)value);
        }


        private string[] note_names = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        private String getNoteFromMidiNumber(int midiNote)
        {
            return note_names[midiNote % 12] + ((midiNote / 12) - 1);
        }

        private int getNumberFromNote(string noteStr)
        {
            int letter = Array.IndexOf(note_names, noteStr.Substring(0, noteStr.Length - 1));
            int octave = noteStr.Last() - '0';
            return octave * 12 + letter + 12;
        }
        #endregion
    }
}
