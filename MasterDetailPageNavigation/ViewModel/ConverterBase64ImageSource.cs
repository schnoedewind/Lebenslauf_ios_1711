using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Lebenslauf
{
    public class ConverterBase64ImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                Assembly assembly = GetType().Assembly();
                Stream imageStream = assembly.GetManifestResourceStream("Lebenslauf.Foto.png");
                var imgsrc = ImageSource.FromStream(() =>
                {

                    imageStream.Position = 0;
                    return imageStream;
                });
                return imgsrc;
            }
            else
            {
                var bArray = (byte[])value;
                var resizer = DependencyService.Get<IMediaService>(); //TODO: DependecyService für iOS implementieren A.S. 
                var resizedBytes = resizer.ResizeImage(bArray, 137, 140);
                Stream stream = new MemoryStream(resizedBytes);
                var imgsrc = ImageSource.FromStream(() =>
                {
                    var ms = new MemoryStream(resizedBytes);
                    ms.Position = 0;
                    return ms;
                });

                return imgsrc;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
