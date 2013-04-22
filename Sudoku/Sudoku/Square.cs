using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Media;

namespace Sudoku
{
  public class Square : Button
  {
    private int? number;
    public int? Number
    {
      get
      {
        return number;
      }
      set
      {
        number = value;
        this.Content = value.ToString();//== null ? "" : value.ToString();
      }
    }
    public readonly string Sound;
    public readonly int Row;
    public readonly int Col;
    /// <summary>
    /// True if this Square was part of the original Sudoku board, false otherwise.
    /// </summary>
    public bool IsChangable {get; set;}

    public Square(int r, int c, string soundName)
    {
      Number = null;
      Height = 48;
      Width = 48;
      FontSize = 32;
      Foreground = Brushes.Black;
      FontWeight = FontWeights.Bold;
      Sound = soundName;
      Row = r;
      Col = c;
      IsChangable = false;
    }
  }
}
