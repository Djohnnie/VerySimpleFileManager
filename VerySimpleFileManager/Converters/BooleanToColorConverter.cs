using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VerySimpleFileManager.Converters;

public class BooleanToColorConverter : IValueConverter
{
    public Brush TrueColor { get; set; }
    public Brush FalseColor { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? TrueColor : FalseColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}