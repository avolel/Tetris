using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int ID { get; }

        private int rotationState;
        private Position offset;

        public Block() =>
            offset = new Position(StartOffset.Row, StartOffset.Column);

        //Return Grid position of current block factoring the current offset
        public IEnumerable<Position> TilePositions()
        {
            foreach(Position p in Tiles[rotationState]) 
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
        }

        //Rotate Block Clock wise
        public void RotateBlockClockwise() =>
            rotationState = (rotationState + 1) % Tiles.Length;

        //Rotate Block Counter ClockWise
        public void RotateBlockCounterClockWise()
        {
            if (rotationState == 0) rotationState = Tiles.Length - 1;
            else rotationState--;
        }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}