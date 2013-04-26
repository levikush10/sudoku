using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Media;
using System.Collections.Generic;

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
    private Random random;
    List<int> numbers;

    private int numSquaresToSolve = 0;

    #region Initialization

    public SudokuGame(UniformGrid g, Button newGame, Button hint, Button solve)
    {
      this.grid = g;
      this.InitializeBoard();
      this.AddBoardButtonsToGrid();
      this.NewGameButton = newGame;
      this.HintButton = hint;
      this.SolveButton = solve;
      random = new Random();
      numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

      //initialize event handlers for buttons
      this.SolveButton.Click += new RoutedEventHandler(SolveButton_Click);
      this.NewGameButton.Click += new RoutedEventHandler(NewGameButton_Click);
      this.HintButton.Click += new RoutedEventHandler(HintButton_Click);

      InitializeSounds();
      //PlaySound("MineSweeperStart.mp3"); Delete
    }

    private void InitializeBoard()
    {
      this.board = new Square[this.grid.Rows, this.grid.Columns];
      this.AddSquares();
    }
    private void AddBoardButtonsToGrid()
    {
      for(int i = 0; i < this.grid.Rows; i++)
        for (int j = 0; j < this.grid.Columns; j++)
        {
          this.grid.Children.Add(this.board[i, j]);
          //shade the squares
          Square upperLeft = FindUpperLeftOfGroup(i, j);
          if ((upperLeft.Row == 0 && upperLeft.Col == 3) ||
              (upperLeft.Row == 3 && upperLeft.Col == 0) ||
              (upperLeft.Row == 3 && upperLeft.Col == 6) ||
              (upperLeft.Row == 6 && upperLeft.Col == 3))
          {
            board[i, j].Background = Brushes.LightGray;
          }
        }
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
            Square tile = new Square(num, num1, "");
            tile.Click += TileClick;
            tile.MouseRightButtonUp += tile_MouseRightButtonUp;
            this.board[num, num1] = tile;
          }
          num1++;
        }
        num++;
      }
    }
    #endregion

    #region Event Handlers

    private void SolveButton_Click(object sender, RoutedEventArgs args)
    {
      if (IsBoardValid())
      {
        if (!Solve(81))
        {
          MessageBoxButton messageBoxButton = MessageBoxButton.OK;
          MessageBox.Show("The current board layout does not lead to a solution. Try removing some numbers and try again.", "Solve Message", messageBoxButton);
        }
      }
      else
      {
        MessageBoxButton messageBoxButton = MessageBoxButton.OK;
        MessageBox.Show("The board is not currently valid. Please remove invalid numbers and try again.", "Solve Message", messageBoxButton);
      }
    }

    private void NewGameButton_Click(object sender, RoutedEventArgs args)
    {
     // MessageBoxButton messageBoxButton = MessageBoxButton.OK;
      //MessageBox.Show("Please select your difficulty", "Difficulty Message", messageBoxButton);
      DifficultyForm frm = new DifficultyForm();
      frm.ShowDialog();
      int dif = frm.dif;
      NewGame(0, 0, dif);
    }


    private void HintButton_Click(object sender, RoutedEventArgs args)
    {
      if (!Solve(1))
      {
        MessageBoxButton messageBoxButton = MessageBoxButton.OK;
        MessageBox.Show("There is no current valid move with this board layout. Try removing some numbers and try again.", "Hint Message", messageBoxButton);
      }
    }

    public void TileClick(object sender, EventArgs e)
    {
      Square s = sender as Square;
      if (s.IsChangable == true)
      {
        if (s.Number == null)
          s.Number = 1;
        else if (s.Number == 9)
          s.Number = null;
        else
          s.Number++;

        CheckUserMove(s);

      }
    }

    public void tile_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
      Square s = sender as Square;
      if (s.IsChangable == true)
      {
        if (s.Number == null)
          s.Number = 9;
        else if (s.Number == 1)
          s.Number = null;
        else
          s.Number--;

        CheckUserMove(s);
      }
    }

    private void CheckUserMove(Square s)
    {
      if (!CheckMove(s.Row, s.Col))
      {
        s.Foreground = Brushes.Red;
        s.IsValid = false;
      }
      else
      {
        s.Foreground = Brushes.Black;
        s.IsValid = true;
      }
    }

    #endregion

    #region Old Code

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

    #endregion

    #region Sounds

    private void PlaySound(Square t)
    {
      PlaySound(t.Sound);
    }
    private void PlaySound(string soundName)
    {
      player.Source = new Uri("Sounds/" + soundName, UriKind.Relative);
      player.Play();
    }

    #endregion

    public void NewGame(int row, int col, int difficulty)
    {
      ClearBoard();
      int numberToRemove;
      if (difficulty == 1)
        numberToRemove = 60;
      else if (difficulty == 2)
        numberToRemove = 65;
      else
        numberToRemove = 70;

      Solve(81);

      for (int n = 0; n <= numberToRemove; n++)
      {
        int n1 = random.Next(0, 9);
        int n2 = random.Next(0, 9);

        if (board[n1, n2].Number != null)
        {
          board[n1, n2].Number = null;
          board[n1, n2].IsChangable = true;
          board[n1, n2].Foreground = Brushes.Black;
        }
        else
          n--;
      }
    }

    private bool IsBoardValid()
    {
      System.Collections.IEnumerator i = board.GetEnumerator();
      Square s;
      while (i.MoveNext())
      {
        s = (Square)i.Current;
        if (s.IsValid == false)
          return false;
      }
      return true;
    }

    private void ClearBoard()
    {
      System.Collections.IEnumerator i = board.GetEnumerator();
      Square s;
      while (i.MoveNext())
      {
        s = (Square)i.Current;
        s.Reset();
      }
    }

    private bool Solve(int num)
    {
      Shuffle();
      numSquaresToSolve = num;
      if (SolveHelper(0, 0, 0))
      {
        return true;
      }
      return false;
    }

    private void Shuffle()
    {
      int temp;
      int next;
      for (int i = numbers.Count - 1; i > 0; i--)
      {
        next = random.Next(i + 1);
        temp = numbers[i];
        numbers[i] = numbers[next];
        numbers[next] = temp;
      }
    }

    public bool SolveHelper(int row, int col, int count)
    {
      if (count >= numSquaresToSolve)
      {
        return true;
      }
      //if it's out of range then we're done!
      if (row > 8 || col > 8)
      {
        return true;
      }
      int newCol = ((col + 1) % 9);
      if (board[row, col].Number == null)
      {
        //try each number from the shuffled list of numbers
        for (int n = 0; n < board.GetLength(1); n++)
        {
          if (col > 8) throw new Exception("WHAT THE CRAP?!?!");
          board[row, col].Number = numbers[n];
          if (CheckMove(row, col))
          {
            //numSquaresToSolve++;
            if (numSquaresToSolve == 1)
              board[row, col].Foreground = Brushes.Green;
            if (newCol > 8)
              throw new Exception("WHAT THE CRAP?!?!");
            if (SolveHelper(col == 8 ? row + 1 : row, newCol, count + 1))
              return true;
          }
        }
      }
      else//there is already a number in the square
      {
        return SolveHelper(col == 8 ? row + 1 : row, newCol, count);
      }
      //backtrack
      board[row, col].Number = null;
      if (numSquaresToSolve == 1)
      {
        board[row, col].Foreground = Brushes.Black;
        if (SolveHelper(col == 8 ? row + 1 : row, newCol, count))
          return true;
      }
      return false;
    }

    private bool CheckMove(int row, int col)
    {
      if (board[row, col].Number == null)
        return true;
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

    private Square FindUpperLeftOfGroup(int row, int col)
    {
      int upperLeftRow = row - (row % 3);
      int upperLeftCol = col - (col % 3);
      return board[upperLeftRow, upperLeftCol];
    }

    private bool CheckGroup(int row, int col)
    {
      Square s = FindUpperLeftOfGroup(row, col);
      for (int i = s.Row; i <= s.Row + 2; i++)
        for (int j = s.Col; j <= s.Col + 2; j++)
          if (i != row && j != col && board[i, j].Number == board[row, col].Number)
            return false;
      return true;
    }
  }
}