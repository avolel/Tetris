namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        //Indexer for getting and setting Game Grid objects
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInsideGrid(int row, int column) =>
            row >= 0 && row < Rows && column >= 0 && column < Columns;

        public bool IsEmpty(int row, int column) =>
            IsInsideGrid(row, column) && grid[row, column] == 0;

        public bool IsRowFull(int row)
        {
            for(int col = 0; col < Columns; col++)
            {
                if(grid[row,col] == 0) return false;
            }
            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for(int col =0; col < Columns; col++)
            {
                if(grid[row,col] != 0) return false;
            }
            return true;
        }

        private void ClearRow(int row)
        {
            for(int col = 0; col < Columns; col++) grid[row, col] = 0;
        }

        private void MoveRowDown(int row, int numOfRows)
        {
            for(int col = 0; col < Columns; col++)
            {
                grid[row + numOfRows, col] = grid[row, col];
                grid[row,col] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;
            for(int row = Rows-1; row >= 0; row--)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    cleared++;
                }
                else if(cleared > 0)
                    MoveRowDown(row,cleared);
            }
            return cleared;
        }
    }
}