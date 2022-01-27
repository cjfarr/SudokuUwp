namespace Sudoku.Interfaces
{
    using Events;

    public interface ISudokuService
    {
        void GenerateBoard();

        int GetCellSolution(int cellIndex);

        int[] GetCellIndexesByRegion(int regionId);

        int GetCurrentCellInput(int cellIndex);

        void UpdateSelectedCell(int cellIndex, int regionId);

        void GiveInput(int inputNumber);

        int GetEmptyRemainingInputCount(int inputNumber);

        bool CheckIfBoardWasSolved();

        event SelectedCellChanged SelectedCellChangedEvent;

        event CellValueChanged CellValueChangedEvent;

        event NewBoardGenerated NewBoardGeneratedEvent;
    }
}
