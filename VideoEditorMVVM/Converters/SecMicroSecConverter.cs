using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VideoEditorMVVM.Converters
{
    public class SecMicroSecConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a DateTime object to 
        // a month string.
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            if (!(value is long)) return "null";
            return (long)value / 1000000.0;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            if (!(value is double)) return "null";
            return (long)((double)value * 1000000.0);
        }

        #endregion
    }
}
