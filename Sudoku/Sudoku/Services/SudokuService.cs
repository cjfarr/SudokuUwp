namespace Sudoku.Services
{
    using Sudoku.Events;
    using Sudoku.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class SudokuService : ISudokuService
    {
        private Random random;

        private int[] currentBoard;
        private int[] solvedBoard;
        private int[] selectedCellMap;
        private int currentSelectedCellId;

        private Dictionary<int, int> offsetIndexByRegion;

        public SudokuService()
        {
            this.random = new Random();
            this.currentBoard = new int[81];
            this.solvedBoard = new int[81];
            this.selectedCellMap = new int[81];

            this.offsetIndexByRegion = new Dictionary<int, int>()
            {
                { 1, 0 },
                { 2, 3 },
                { 3, 6 },
                { 4, 27 },
                { 5, 30 },
                { 6, 33 },
                { 7, 54 },
                { 8, 57 },
                { 9, 60 }
            };
        }

        public int StartingEmpytCount
        {
            get;
            set;
        }

        public event SelectedCellChanged SelectedCellChangedEvent;

        public event CellValueChanged CellValueChangedEvent;

        public event NewBoardGenerated NewBoardGeneratedEvent;

        public void GenerateBoard()
        {
            List<int> seeds = Enumerable.Range(1, 9).ToList();
            int[] seededBoard = new int[]
            {
                5, 3, 4, 6, 7, 8, 9, 1, 2,
                6, 7, 2, 1, 9, 5, 3, 4, 8,
                1, 9, 8, 3, 4, 2, 5, 6, 7,

                8, 5, 9, 7, 6, 1, 4, 2, 3,
                4, 2, 6, 8, 5, 3, 7, 9, 1,
                7, 1, 3, 9, 2, 4, 8, 5, 6,

                9, 6, 1, 5, 3, 7, 2, 8, 4,
                2, 8, 7, 4, 1, 9, 6, 3, 5,
                3, 4, 5, 2, 8, 6, 1, 7, 9
            };

            Dictionary<int, int> seedMap = new Dictionary<int, int>();
            for (int key = 1; key <= 9; key++)
            {
                int assignment = this.random.Next(1, 10);
                while (!seeds.Contains(assignment))
                {
                    assignment = this.random.Next(1, 10);
                }

                seedMap[key] = assignment;
                seeds.Remove(assignment);
            }

            this.InitializedArray(0, ref this.solvedBoard);

            for (int index = 0; index < this.solvedBoard.Length; index++)
            {
                this.solvedBoard[index] = seedMap[seededBoard[index]];
            }

            Array.Copy(this.solvedBoard, this.currentBoard, this.solvedBoard.Length);
            List<int> emptyCells = new List<int>();

            for (int index = 0; index < this.StartingEmpytCount; index++)
            {
                int cellIndex = this.random.Next(0, this.solvedBoard.Length);
                while (emptyCells.Contains(cellIndex))
                {
                    cellIndex = this.random.Next(0, this.solvedBoard.Length);
                }

                emptyCells.Add(cellIndex);
                this.currentBoard[cellIndex] = -1;
            }

            this.selectedCellMap = new int[]
            {
                2, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 0, 0, 0, 0, 0, 0,
                1, 1, 1, 0, 0, 0, 0, 0, 0,

                1, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 0,

                1, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 0
            };

            this.currentSelectedCellId = 0;
            System.Diagnostics.Debug.WriteLine("GenerateBoard completed");
            this.NewBoardGeneratedEvent?.Invoke();
            this.SelectedCellChangedEvent?.Invoke(this.selectedCellMap);
        }

        public int GetCellSolution(int cellIndex)
        {
            return this.solvedBoard[cellIndex];
        }

        public int[] GetCellIndexesByRegion(int regionId)
        {
            int[] regionIndexes = new int[9];

            int cellId = this.offsetIndexByRegion[regionId] - 1;
            for (int index = 0; index < regionIndexes.Length; index++)
            {
                int offset = 0;
                if (index > 0 && index % 3 == 0)
                {
                    offset = 6;
                }

                cellId += 1 + offset;
                regionIndexes[index] = cellId;
            }

            return regionIndexes;
        }

        public int GetCurrentCellInput(int cellIndex)
        {
            return this.currentBoard[cellIndex];
        }

        public int[] GetAllRowIndexes(int cellIndex)
        {
            int[] rowIndexes = new int[9];
            int mod = cellIndex % 9;
            int offset = cellIndex - mod;

            for (int columnIndex = offset; columnIndex < offset + 9; columnIndex++)
            {
                rowIndexes[columnIndex - offset] = columnIndex;
            }

            return rowIndexes;
        }

        public int[] GetAllColumnIndexes(int cellIndex)
        {
            int[] columnIndexes = new int[9];
            int offset = 0;
            for (int rowIndex = cellIndex % 9; rowIndex < this.solvedBoard.Length; rowIndex += 9)
            {
                columnIndexes[offset++] = rowIndex;
            }

            return columnIndexes;
        }

        public void UpdateSelectedCell(int cellIndex, int regionId)
        {
            this.currentSelectedCellId = cellIndex;
            this.InitializedArray(0, ref this.selectedCellMap);

            foreach (int rowIndex in this.GetAllRowIndexes(cellIndex))
            {
                this.selectedCellMap[rowIndex] = 1;
            }

            foreach (int regionIndex in this.GetCellIndexesByRegion(regionId))
            {
                this.selectedCellMap[regionIndex] = 1;
            }

            foreach (int columnIndex in this.GetAllColumnIndexes(cellIndex))
            {
                this.selectedCellMap[columnIndex] = 1;
            }

            this.selectedCellMap[cellIndex] = 2;
            this.SelectedCellChangedEvent?.Invoke(this.selectedCellMap);
        }

        public void GiveInput(int inputNumber)
        {
            int currentValue = this.currentBoard[this.currentSelectedCellId];

            if (inputNumber == currentValue)
            {
                ////treat this as an undo
                this.currentBoard[this.currentSelectedCellId] = -1;
            }
            else
            {
                this.currentBoard[this.currentSelectedCellId] = inputNumber;
            }

            this.CellValueChangedEvent?.Invoke(this.currentSelectedCellId, this.currentBoard[this.currentSelectedCellId]);
        }

        public int GetEmptyRemainingInputCount(int inputNumber)
        {
            ////I want to add up what they have then subtract from 9.  Then if it's negative, they know
            ////that too many have been put out on the board.
            int totalGiven = this.currentBoard.Count(n => n == inputNumber);

            return 9 - totalGiven;
        }

        public bool CheckIfBoardWasSolved()
        {
            for (int index = 0; index < this.currentBoard.Length; index++)
            {
                if (this.currentBoard[index] != this.solvedBoard[index])
                {
                    return false;
                }
            }

            return true;
        }

        private void InitializedArray(int defaultValue, ref int[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                array[index] = defaultValue;
            }
        }
    }
}
