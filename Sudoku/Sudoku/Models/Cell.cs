namespace Sudoku.Models
{
    using Sudoku.Interfaces;

    internal class Cell : NotifyPropertyChangeModel
    {
        private ISudokuService sudokuService;
        private string currentInput;
        private int selectionCode;

        public Cell(int cellIndex, int regionId, ISudokuService sudokuService)
        {
            this.CellId = cellIndex;
            this.RegionId = regionId;
            this.sudokuService = sudokuService;

            ////Normally, I would unsubcribe these with a dispose pattern
            this.sudokuService.SelectedCellChangedEvent += this.OnSelectedCellChangedEvent;
            this.sudokuService.CellValueChangedEvent += OnCellValueChangedEvent;
            this.sudokuService.NewBoardGeneratedEvent += OnNewBoardGeneratedEvent;
        }

        private void OnCellValueChangedEvent(int cellId, int changedValue)
        {
            if (this.CellId != cellId)
            {
                return;
            }

            this.CurrentInput = changedValue <= 0 ? string.Empty : changedValue.ToString();
        }

        public int SelectionCode
        {
            get
            {
                return this.selectionCode;
            }

            set
            {
                this.selectionCode = value;
                this.RaisePropertyChange(nameof(this.SelectionCode));
            }
        }

        public int CellId
        { 
            get;
            set;
        }

        public int RegionId
        {
            get;
            set;
        }

        public string CurrentInput
        {
            get
            {
                int currentValue = this.sudokuService.GetCurrentCellInput(this.CellId);
                if (currentValue < 1)
                {
                    return string.Empty;
                }

                return currentValue.ToString();
            }

            set
            {
                this.currentInput = value;
                this.RaisePropertyChange(nameof(this.CurrentInput));
            }
        }

        public void ProcessPointerPressedInput()
        {
            this.sudokuService.UpdateSelectedCell(this.CellId, this.RegionId);
        }

        private void OnSelectedCellChangedEvent(int[] selectedCellMap)
        {
            this.SelectionCode = selectedCellMap[this.CellId];
        }

        private void OnNewBoardGeneratedEvent()
        {
            this.RaisePropertyChange(nameof(this.CurrentInput));
        }
    }
}
