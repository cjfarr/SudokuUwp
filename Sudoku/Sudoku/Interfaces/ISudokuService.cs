namespace Sudoku.Interfaces
{
    using Events;
    using System;

    public interface ISudokuService
    {
        int StartingEmpytCount
        {
            get;
            set;
        }

        TimeSpan ViewTimeForErrors
        {
            get;
        }

        bool IsShowingErrors
        {
            get;
        }

        int RemainingErrorViews
        {
            get;
        }

        void GenerateBoard();

        int GetCellSolution(int cellIndex);

        int[] GetCellIndexesByRegion(int regionId);

        int GetCurrentCellInput(int cellIndex);

        void UpdateSelectedCell(int cellIndex, int regionId);

        void GiveInput(int inputNumber);

        int GetEmptyRemainingInputCount(int inputNumber);

        bool CheckIfBoardWasSolved();

        void ShowErrors();

        void RemoveErrors();

        event SelectedCellChanged SelectedCellChangedEvent;

        event CellValueChanged CellValueChangedEvent;

        event NewBoardGenerated NewBoardGeneratedEvent;
    }
}
