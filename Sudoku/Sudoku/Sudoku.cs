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
    private Button NewGameButton;
    private Button HintButton;
    private Button SolveButton;

    private int solveCount = 0;

    public SudokuGame(UniformGrid g, Button newGame, Button hint, Button solve)
    {
      this.grid = g;
      this.InitializeBoard();
      this.AddBoardButtonsToGrid();
      this.NewGameButton = newGame;
      this.HintButton = hint;
      this.SolveButton = solve;

      //initialize event handlers for buttons
      this.SolveButton.Click += new RoutedEventHandler(SolveButton_Click);

      InitializeSounds();

      //PlaySound("MineSweeperStart.mp3");
    }

    private void SolveButton_Click(object sender, RoutedEventArgs args)
    {
      Solve(0, 0);
    }

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
      this.AddSquares();
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
    private void AddSquares()
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
            Square tile = new Square(num, num1, "MineSweeperMove.mp3");
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

    public void TileClick(object sender, EventArgs e)
    {
      //TODO: fill this in for what to do when a user clicks a square
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

    static void tile_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
      //TODO: here maybe we should allow the user to decrement the number when right clicking?
      // and then the number could be incremented on left click?
      // that might be nice to they don't have to click as many times. It could just wrap around.
    }

    public bool Solve(int row, int col)
    {
      //base cases
      if (row > 8 || col > 8 || solveCount == 81 || board[row, col].IsFixed)
      {
        return true;
      }

      //try each number 1-9
      for (int n = 1; n <= board.GetLength(1); n++)
      {
        if (col > 8) throw new Exception("WHAT THE CRAP?!?!");
        board[row, col].Number = n;
        if (CheckMove(row, col))
        {
          solveCount++;
          int newCol = ((col + 1) % 9);
          if (newCol > 8)
            throw new Exception("WHAT THE CRAP?!?!");
          if (Solve(col == 8 ? row + 1 : row, newCol))
            return true;
        }
      }

      //backtrack
      board[row, col].Number = null;
      solveCount--;
      return false;
    }

    private bool CheckMove(int row, int col)
    {
      return CheckRow(row, col) && CheckColumn(row, col) && CheckGroup(row, col);
    }

    private bool CheckRow(int row, int col)
    {
      for (int i = 0; i < board.GetLength(1); i++)
      {
        if (i != col)
          if (board[row, i].Number == board[row, col].Number)
            return false;
      }
      return true;
    }

    private bool CheckColumn(int row, int col)
    {
      for (int i = 0; i < board.GetLength(1); i++)
      {
        if (i != row)
          if (board[i, col].Number == board[row, col].Number)
            return false;
      }
      return true;
    }

    private bool CheckGroup(int row, int col)
    {
      int upperLeftRow = row - (row % 3);
      int upperLeftCol = col - (col % 3);
      for (int i = upperLeftRow; i <= upperLeftRow + 2; i++)
        for (int j = upperLeftCol; j <= upperLeftCol + 2; j++)
          if (i != row && j != col && board[i, j].Number == board[row, col].Number)
            return false;
      return true;
    } 
  }
}