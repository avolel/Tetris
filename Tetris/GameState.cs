namespace Tetris
{
    public class GameState
    {
        private Block? currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for(int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!DoesBlockFit())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public bool GamePaused { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; } = 0;
        public int Delay { get; private set; } = 1200;
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool DoesBlockFit()
        {
            foreach(Position p in CurrentBlock.TilePositions())
            {
                if(!GameGrid.IsEmpty(p.Row, p.Column)) return false;
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
                return;
            if(HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }
            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateBlockClockwise();
            if(!DoesBlockFit()) CurrentBlock.RotateBlockCounterClockWise();
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateBlockCounterClockWise();
            if (!DoesBlockFit()) CurrentBlock.RotateBlockClockwise();
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!DoesBlockFit()) CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!DoesBlockFit()) CurrentBlock.Move(0, -1);
        }

        private bool IsGameOver() =>
            !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions()) 
                GameGrid[p.Row, p.Column] = CurrentBlock.ID;

            CalculateScore(GameGrid.ClearFullRows());

            if (IsGameOver())
                GameOver = true;
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }

        }

        private void CalculateScore(int numRowsCleared)
        {
            switch (Level)
            {
                case 0:
                    if (numRowsCleared == 1)
                        Score = Score + 40;
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 100;
                        Level += 1;
                        Delay += 1;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 300;
                        Level += 1;
                        Delay += 1;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 1200;
                        Level += 1;
                        Delay += 1;
                    }
                    break;
                case 1:
                    if (numRowsCleared == 1)
                        Score = Score + 80;
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 200;
                        Level += 1;
                        Delay += 2;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 600;
                        Level += 1;
                        Delay += 2;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 2400;
                        Level += 1;
                        Delay += 2;
                    }
                    break;
                case 2:
                    if (numRowsCleared == 1)
                        Score = Score + 120;
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 300;
                        Level += 1;
                        Delay += 3;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 900;
                        Level += 1;
                        Delay += 3;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 3600;
                        Level += 1;
                        Delay += 3;
                    }
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    if (numRowsCleared == 1)
                        Score = Score + 120;
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 300;
                        Level += 1;
                        Delay -= 4;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 900;
                        Level += 1;
                        Delay -= 4;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 3600;
                        Level += 1;
                        Delay -= 4;
                    }
                    break;
                case 9:
                    if (numRowsCleared == 1)
                        Score = Score + 400;
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 1000;
                        Level += 1;
                        Delay -= 10;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 3000;
                        Level += 1;
                        Delay -= 10;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 12000;
                        Level += 1;
                        Delay -= 10;
                    }
                    break;
                default:
                    if (numRowsCleared == 1)
                    {
                        Score = Score + 40 * (Level + 1);
                        Delay = Delay - 40;
                    }
                    else if (numRowsCleared == 2)
                    {
                        Score = Score + 100 * (Level + 1);
                        Delay = Delay - 100;
                    }
                    else if (numRowsCleared == 3)
                    {
                        Score = Score + 300 * (Level + 1);
                        Delay = Delay - 300;
                    }
                    else if (numRowsCleared >= 4)
                    {
                        Score = Score + 1200 * (Level + 1);
                        Delay = Delay - 1200;
                    }
                    break;
            }

        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        public void PauseGame() =>
            GamePaused = true;

        public void UnPauseGame() =>
            GamePaused = false;

        public void MoveBlockDown() 
        {
            CurrentBlock.Move(1, 0);
            if (!DoesBlockFit())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        
        }
    }
}