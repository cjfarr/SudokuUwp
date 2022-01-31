namespace Sudoku.Converters
{
    using System;
    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class CellForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int selectionCode = int.Parse(value.ToString());

            switch (selectionCode)
            {
                case 3:
                    return new SolidColorBrush(Colors.White);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
