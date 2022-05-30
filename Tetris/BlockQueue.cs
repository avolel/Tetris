using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[] {
            new I_Block(),
            new J_Block(),
            new L_Block(),
            new O_Block(),
            new S_Block(),
            new T_Block(),
            new Z_Block()
        };

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueue() =>
            NextBlock = RandomBlock();        

        private Block RandomBlock() => 
            blocks[random.Next(blocks.Length)];

        public Block GetAndUpdate()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.ID == NextBlock.ID);

            return block;
        }
    }
}