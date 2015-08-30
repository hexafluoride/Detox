using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Globalization;

using Detox.ViewModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Detox.Converters
{
    // thanks to http://stackoverflow.com/a/1763678
    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            if (path != null)
            {
                BitmapImage image = new BitmapImage();

                using (FileStream stream = File.OpenRead(path))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit(); // load the image from the stream
                } // close the stream
                return image;
            }

            return null;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
