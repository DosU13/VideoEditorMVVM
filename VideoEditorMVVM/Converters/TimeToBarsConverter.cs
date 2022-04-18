using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Models;
using Windows.UI.Xaml.Data;

namespace VideoEditorMVVM.Converters
{
    // Custom class implements the IValueConverter interface.
    public class TimeToBarsConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a DateTime object to 
        // a month string.
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            if (!(value is long)) return "null";
            var metric = new MetricTimeSpan((long)value);
            var r = TimeConverter.ConvertTo<BarBeatFractionTimeSpan>(metric, SequenceModel.TempoMap);
            return r.ToString();
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
