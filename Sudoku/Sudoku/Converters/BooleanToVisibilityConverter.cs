namespace Sudoku.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVisible = false;
            if (value?.GetType() == typeof(bool))
            {
                isVisible = bool.Parse(value.ToString());
            }

            if (parameter != null)
            {
                if (bool.TryParse(parameter?.ToString(), out bool doOpposite))
                {
                    isVisible = !isVisible;
                }
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
