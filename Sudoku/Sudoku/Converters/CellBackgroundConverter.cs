namespace Sudoku.Converters
{
    using System;
    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    internal class CellBackgroundConverter : IValueConverter
    {
        private SolidColorBrush[] brushes;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (this.brushes == null)
            {
                ////0 = No guide, 1 = row, col, region guide, 2 = selected, 3 = error viewings
                this.brushes = new SolidColorBrush[4]
                {
                    new SolidColorBrush(Colors.White),
                    new SolidColorBrush(Color.FromArgb(255, 224, 246, 255)),
                    new SolidColorBrush(Color.FromArgb(255, 255, 230, 140)),
                    new SolidColorBrush(Color.FromArgb(255, 220, 68, 68))
                };
            }

            if (value.GetType() != typeof(int))
            {
                return this.brushes[0];
            }

            int selectionCode = int.Parse(value.ToString());
            return this.brushes[selectionCode];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
