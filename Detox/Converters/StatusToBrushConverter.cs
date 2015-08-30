using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Globalization;

using SharpTox.Core;

using Detox.ViewModel;
using System.Windows.Media;

namespace Detox.Converters
{
    class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if (!(value is Status))
                return null;

            switch((Status)value)
            {
                case Status.Available:
                    return new SolidColorBrush(Color.FromRgb(0, 201, 30));
                case Status.Away:
                    return Brushes.Orange;
                case Status.Busy:
                    return Brushes.Red;
                case Status.Offline:
                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
