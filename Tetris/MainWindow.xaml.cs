﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
             new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
             new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[] { 
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private GameState gameState = new GameState();
        private int delay = 1200;

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for(int row = 0; row < grid.Rows; row++)
            {
                for(int col = 0; col < grid.Columns; col++)
                {
                    Image imageControl = new Image { 
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (row - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, col * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[row,col] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for(int row = 0; row < grid.Rows; row++)
            {
                for(int col = 0; col < grid.Columns; col++)
                {
                    int id = grid[row, col];
                    imageControls[row, col].Opacity = 1;
                    imageControls[row, col].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.ID];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.ID];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.ID];
            }
        }


        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.ID];
            }
        }

        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                if (gameState.GamePaused)
                {
                    GamePauseMenu.Visibility = Visibility.Visible;
                    YourScoreText.Text = $" Current Score: {gameState.Score}";
                    break;
                }
                await Task.Delay(gameState.Delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            if (gameState.GameOver)
            {
                GameOverMenu.Visibility = Visibility.Visible;
                FinalScoreText.Text = $"Score: {gameState.Score}";
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score} Level: {gameState.Level}";

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
                return;
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.S:
                    gameState.RotateBlockCCW();
                    break;
                case Key.H:
                    gameState.HoldBlock();
                    break;
                case Key.D:
                    gameState.DropBlock();
                    break;
                case Key.P:
                    gameState.PauseGame();
                    break;
                case Key.U:
                    gameState.UnPauseGame();
                    GamePauseMenu.Visibility = Visibility.Hidden;
                    await GameLoop();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private async void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            gameState.UnPauseGame();
            GamePauseMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}