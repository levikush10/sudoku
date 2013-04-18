using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Media;

namespace Sudoku
{
    internal class SudokuGame
    {
        private UniformGrid grid;
        private Square[,] board;
        private MediaElement player;
        private bool finished;

        public SudokuGame(UniformGrid g)
        {
            this.grid = g;
            this.InitializeBoard();
            this.AddBoardButtonsToGrid();
            InitializeSounds();

            //PlaySound("MineSweeperStart.mp3");
        }
        public event EventHandler GameFinished = null;

        private void InitializeSounds()
        {
            player = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop,
            };
            player.MediaEnded += (o, e) => player.Stop();

            grid.Children.Add(player);
        }
        private void InitializeBoard()
        {
            this.board = new Square[this.grid.Rows, this.grid.Columns];
            //this.AddMines(10);
            this.AddTiles();
        }
        private void AddBoardButtonsToGrid()
        {
            int num = 0;
            while (true)
            {
                bool rows = num < this.grid.Rows;
                if (!rows)
                {
                    break;
                }
                int num1 = 0;
                while (true)
                {
                    rows = num1 < this.grid.Columns;
                    if (!rows)
                    {
                        break;
                    }
                    this.grid.Children.Add(this.board[num, num1]);
                    num1++;
                }
                num++;
            }
        }
        //private void AddMines(int numberOfMines)
        //{
        //    bool flag = numberOfMines <= this.board.Length;
        //    if (flag)
        //    {
        //        Random random = new Random();
        //        int num = 0;
        //        while (true)
        //        {
        //            flag = num < numberOfMines;
        //            if (!flag)
        //            {
        //                break;
        //            }
        //            bool flag1 = true;
        //            do
        //            {
        //                int num1 = random.Next(this.board.Length);
        //                int length = num1 / this.board.GetLength(0);
        //                int length1 = num1 % this.board.GetLength(0);
        //                flag = this.board[length, length1] != null;
        //                if (!flag)
        //                {
        //                    Mine mine = new Mine(length, length1, "MineSweeperMines.mp3");
        //                    mine.Click += new RoutedEventHandler(this.MineClick);
        //                    mine.MouseRightButtonUp += tile_MouseRightButtonUp;
        //                    this.board[length, length1] = mine;
        //                    flag1 = false;
        //                }
        //                flag = flag1;
        //            }
        //            while (flag);
        //            num++;
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        throw new ArgumentException();
        //    }
        //}
        private void AddTiles()
        {
            int num = 0;
            while (true)
            {
                bool length = num < this.board.GetLength(0);
                if (!length)
                {
                    break;
                }
                int num1 = 0;
                while (true)
                {
                    length = num1 < this.board.GetLength(1);
                    if (!length)
                        break;
                    length = board[num, num1] != null;
                    if (!length)
                    {
                        Square tile = new Square(0, num, num1, "MineSweeperMove.mp3");
                        tile.Click += TileClick;
                        tile.MouseRightButtonUp += tile_MouseRightButtonUp;
                        this.board[num, num1] = tile;
                    }
                    num1++;
                }
                num++;
            }
        }

        private void PlaySound(Square t)
        {
            PlaySound(t.Sound);
        }
        private void PlaySound(string soundName)
        {
            player.Source = new Uri("Sounds/" + soundName, UriKind.Relative);
            player.Play();
        }
        private void ClickTile(Square tile, bool playSound = false)
        {
            if (tile == null || tile.Status == TileStatus.Uncovered) return;
            tile.Status = TileStatus.Uncovered;
            if (playSound) PlaySound(tile);
            if (tile.NumberOfMines != 0) return;

            for (var row = tile.Row - 1; row <= tile.Row + 1; row++)
                for (var col = tile.Col - 1; col <= tile.Col + 1; col++)
                {
                    if (IsInvalidRowCol(row, col)) continue;
                    var t = board[row, col];
                    if (t.NumberOfMines == 0) ClickTile(t);
                    if (t.NumberOfMines >= 0) t.Status = TileStatus.Uncovered;
                }
        }
        //private int NumberOfSurroundingMines(int row, int col)
        //{
        //    bool flag;
        //    int num;
        //    int num1;
        //    int num2;
        //    int num3;
        //    int num4 = 0;
        //    int num5 = row - 1;
        //    int num6 = col - 1;
        //    while (true)
        //    {
        //        flag = num6 <= col + 1;
        //        if (!flag)
        //        {
        //            break;
        //        }
        //        int num7 = num4;
        //        num = (this.IsMine(num5, num6) ? 1 : 0);
        //        num4 = num7 + num;
        //        num6++;
        //    }
        //    int num8 = num4;
        //    num1 = (this.IsMine(row, col - 1) ? 1 : 0);
        //    num4 = num8 + num1;
        //    int num9 = num4;
        //    num2 = (this.IsMine(row, col + 1) ? 1 : 0);
        //    num4 = num9 + num2;
        //    num5 = row + 1;
        //    num6 = col - 1;
        //    while (true)
        //    {
        //        flag = num6 <= col + 1;
        //        if (!flag)
        //        {
        //            break;
        //        }
        //        int num10 = num4;
        //        num3 = (this.IsMine(num5, num6) ? 1 : 0);
        //        num4 = num10 + num3;
        //        num6++;
        //    }
        //    int num11 = num4;
        //    return num11;
        //}

        public void TileClick(object sender, EventArgs e)
        {
            if (finished) return;
            var tile = sender as Square;
            if (tile == null || tile.Status == TileStatus.Uncovered) return;
            ClickTile(tile, true);
        }

        static void tile_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tile = sender as Square;
            if (tile == null) return;
            switch (tile.Status)
            {
                case TileStatus.Covered:
                    tile.Status = TileStatus.Flag;
                    break;
                case TileStatus.Flag:
                    tile.Status = TileStatus.Question;
                    break;
                case TileStatus.Question:
                    tile.Status = TileStatus.Covered;
                    break;
            }
        }
        bool IsInvalidRowCol(int row, int col)
        {
            return (row < 0 || row > board.GetUpperBound(0) || col < 0 || col > board.GetUpperBound(1));
        }
    }
}