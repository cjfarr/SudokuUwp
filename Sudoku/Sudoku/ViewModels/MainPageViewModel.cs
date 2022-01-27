namespace Sudoku.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Interfaces;
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;

    internal class MainPageViewModel : NotifyPropertyChangeModel
    {
        private ISudokuService sudokuService;
        private ObservableCollection<Region> regions;
        private ObservableCollection<InputButton> inputButtons;
        private string startingEmpytCountText;

        private ICommand newGameCommand;

        public ObservableCollection<Region> Regions
        {
            get
            {
                return this.regions ?? (this.regions = new ObservableCollection<Region>());
            }
        }

        public ObservableCollection<InputButton> InputButtons
        {
            get
            {
                return this.inputButtons ?? (this.inputButtons = new ObservableCollection<InputButton>(Enumerable.Range(1, 9).Select(i => new InputButton(i))));
            }
        }

        public ICommand NewGameCommand
        {
            get
            {
                return this.newGameCommand ?? (this.newGameCommand = new RelayCommand(this.OnNewGameRequested, true));
            }
        }

        public int StartingEmpytCount
        {
            get
            {
                return this.sudokuService.StartingEmpytCount;
            }

            set
            {
                this.sudokuService.StartingEmpytCount = value;
                this.RaisePropertyChange(nameof(this.StartingEmpytCount));
                this.RaisePropertyChange(nameof(this.StartingEmpytCountText));
            }
        }

        public string StartingEmpytCountText
        {
            get
            {
                return $"Starting Empty: {this.sudokuService.StartingEmpytCount}";
            }
        }

        public void Initialize()
        {
            this.sudokuService = ((SudokuApp)SudokuApp.Current).GetService<ISudokuService>();
            this.sudokuService.StartingEmpytCount = 10;

            for (int index = 1; index <= 9; index++)
            {
                this.Regions.Add(new Region(index));
            }

            this.sudokuService.GenerateBoard();
        }

        private async void OnNewGameRequested()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Start over?",
                Content = "Current progress will be lost. Start over?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await dialog.ShowAsync(ContentDialogPlacement.Popup);

            switch (result)
            {
                case ContentDialogResult.Primary:
                    this.sudokuService.GenerateBoard();

                    break;
                case ContentDialogResult.Secondary:
                    ////return to current board

                    break;
            }
        }
    }
}
