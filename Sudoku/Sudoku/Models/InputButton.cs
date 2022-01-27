namespace Sudoku.Models
{
    using GalaSoft.MvvmLight.Command;
    using Sudoku.Interfaces;
    using System.Windows.Input;

    internal class InputButton : NotifyPropertyChangeModel
    {
        private ISudokuService sudokuService;
        private ICommand inputCommand;
        
        public InputButton(int inputNumber)
        {
            this.InputNumber = inputNumber;
            this.sudokuService = ((SudokuApp)SudokuApp.Current).GetService<ISudokuService>();
            this.sudokuService.CellValueChangedEvent += (c, v) => { this.RefreshEmptyRemainingCount(); };
            this.sudokuService.NewBoardGeneratedEvent += this.RefreshEmptyRemainingCount;
        }

        public int InputNumber
        {
            get;
            private set;
        }

        public ICommand InputCommand
        {
            get
            {
                return this.inputCommand ?? (this.inputCommand = new RelayCommand(() =>
                {
                    this.sudokuService.GiveInput(this.InputNumber);
                    this.RaisePropertyChange(nameof(this.EmptyRemainingCount));
                }));
            }
        }

        public int EmptyRemainingCount
        {
            get
            {
                return this.sudokuService.GetEmptyRemainingInputCount(this.InputNumber);
            }
        }

        private void RefreshEmptyRemainingCount()
        {
            this.RaisePropertyChange(nameof(this.EmptyRemainingCount));
        }
    }
}
