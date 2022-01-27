namespace Sudoku.Views
{
    using Sudoku.Interfaces;
    using System;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ISudokuService sudokuService;

        public MainPage()
        {
            this.InitializeComponent();
            this.viewModel.Initialize();
            this.sudokuService = ((SudokuApp)SudokuApp.Current).GetService<ISudokuService>();
            this.sudokuService.CellValueChangedEvent += OnCellValueChangedEvent;
        }

        private void OnCellValueChangedEvent(int cellId, int changedValue)
        {
            if (this.sudokuService.CheckIfBoardWasSolved())
            {
                this.TellPlayerTheyWon();
            }
        }

        private async void TellPlayerTheyWon()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Congratulations!",
                Content = "You solved the board!",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "New Game"
            };

            ContentDialogResult result = await dialog.ShowAsync(ContentDialogPlacement.Popup);

            switch (result)
            {
                case ContentDialogResult.Primary:
                    ////Do nothing.  Let them review the board if they would like.
                    break;
                case ContentDialogResult.Secondary:
                    this.sudokuService.GenerateBoard();

                    break;
            }
        }
    }
}
