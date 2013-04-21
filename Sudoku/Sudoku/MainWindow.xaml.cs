using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      SudokuGame game = new SudokuGame(BoardGrid, NewGameButton, HintButton, SolveButton);
    }

    private void HintButton_Click(object sender, RoutedEventArgs e)
    {
      //TODO: Give a valid next move
    }

    private void SolveButton_Click(object sender, RoutedEventArgs e)
    {
      //TODO: solve the entire puzzle and display solution
    }

    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
      //TODO: Create new game
    }
  }
}
