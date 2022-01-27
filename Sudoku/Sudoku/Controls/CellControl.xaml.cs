namespace Sudoku.Controls
{
    using Sudoku.Models;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    public sealed partial class CellControl : UserControl
    {
        private Cell model;

        public CellControl()
        {
            this.InitializeComponent();
            this.model = this.DataContext as Cell;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (this.model == null)
            {
                this.model = this.DataContext as Cell;
            }

            this.model?.ProcessPointerPressedInput();
            base.OnPointerPressed(e);
        }
    }
}
