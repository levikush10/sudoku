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
        private TileStatus status;
        public int NumberOfMines { get; private set; }
        public readonly string Sound;
        public readonly int Row;
        public readonly int Col;
        public bool IsFixed;

        public Square(int numOfMines, int r, int c, string soundName)
        {
            NumberOfMines = numOfMines;
            Height = 48;
            Width = 48;
            FontSize = 32;
            Foreground = GetColor();
            FontWeight = FontWeights.Bold;
            Status = TileStatus.Covered;
            Sound = soundName;

            Row = r;
            Col = c;

        }

        private Brush GetColor()
        {
            Brush brush;
            switch (NumberOfMines)
            {
                case 1:
                    brush = (Brush)Brushes.Blue;
                    break;
                case 2:
                    brush = (Brush)Brushes.Green;
                    break;
                case 3:
                    brush = (Brush)Brushes.Red;
                    break;
                case 4:
                    brush = (Brush)Brushes.DarkBlue;
                    break;
                case 5:
                    brush = (Brush)Brushes.DarkGreen;
                    break;
                case 6:
                    brush = (Brush)Brushes.DarkRed;
                    break;
                default:
                    brush = (Brush)Brushes.Black;
                    break;
            }

            return brush;

        }
        //protected ImageBrush BackFromName(string name)
        //{
        //    return new ImageBrush((ImageSource)FindResource(name));
        //}

        public TileStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                var name = "";
                switch (value)
                {
                    case TileStatus.Covered:
                        name = "tile";
                        break;
                    case TileStatus.Uncovered:
                        name = NumberOfMines < 0 ? "mine" : "viewedTile";
                        if (NumberOfMines >= 0) Content = NumberOfMines;
                        break;
                    case TileStatus.Flag:
                        name = "flag";
                        break;
                    case TileStatus.Question:
                        name = "questionmark";
                        break;
                }
            }
        }
    }

    public enum TileStatus
    {
        Covered,
        Uncovered,
        Flag,
        Question
    }
}
