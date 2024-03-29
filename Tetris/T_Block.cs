﻿namespace Tetris
{
    public class T_Block : Block
    {
        protected override Position[][] Tiles => tiles;

        protected override Position StartOffset => new Position(0, 3);

        public override int ID => 6;

        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] {new(0,1), new(1,0), new(1,1), new(1,2)},
            new Position[] {new(0,1), new(1,1), new(1,2), new(2,1)},
            new Position[] {new(1,0), new(1,1), new(1,2), new(2,1)},
            new Position[] {new(0,1), new(1,0), new(1,1),new(2,1)}
        };
    }
}