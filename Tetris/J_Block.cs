namespace Tetris
{
    public class J_Block : Block
    {
        protected override Position[][] Tiles => tiles;

        protected override Position StartOffset => new Position(0, 3);

        public override int ID => 2;

        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] {new(0,0), new(1,0), new(1,1), new(1,2)},
            new Position[] {new(0,1), new(0,2), new(1,1), new(2,1)},
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,2)},
            new Position[] {new(0,1), new(1,1), new(2,0),new(2,1)}
        };
    }
}