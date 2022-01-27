namespace Sudoku.Models
{
    using Sudoku.Interfaces;
    using System.Collections.Generic;

    internal class Region
    {
        private ISudokuService sudokuService;
        private List<Cell> cells;

        public Region(int regionId)
        {
            this.RegionId = regionId;

            foreach (int cellId in this.SudokuService.GetCellIndexesByRegion(this.RegionId))
            {
                this.Cells.Add(new Cell(cellId, this.RegionId, this.SudokuService));
            }
        }

        public int RegionId 
        { 
            get; 
            set;
        }

        public List<Cell> Cells
        {
            get
            {
                return this.cells ?? (this.cells = new List<Cell>());
            }
        }

        private ISudokuService SudokuService
        {
            get
            {
                return this.sudokuService ?? (this.sudokuService = ((SudokuApp)SudokuApp.Current).GetService<ISudokuService>());
            }
        }

        public override string ToString()
        {
            return this.RegionId.ToString();
        }
    }
}
