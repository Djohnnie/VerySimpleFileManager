using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VerySimpleFileManager.Converters;

internal class BytesToBitmapImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var imageBytes = value as byte[];

        BitmapImage image = new BitmapImage();

        if (imageBytes != null)
        {
            try
            {
                using MemoryStream memStream = new MemoryStream(imageBytes);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memStream;
                image.DecodePixelWidth = 100;
                image.EndInit();
                image.Freeze();
            }
            catch { }
        }

        return image;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}