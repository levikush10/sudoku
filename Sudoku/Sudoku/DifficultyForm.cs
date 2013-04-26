using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sudoku
{
  public partial class DifficultyForm : Form
  {
    public int dif = 0;

    public DifficultyForm()
    {
      InitializeComponent();
    }

    public void button1_Click(object sender, EventArgs e)
    {
      dif = 1;
      this.DialogResult = System.Windows.Forms.DialogResult.No;

    }

    public void button2_Click(object sender, EventArgs e)
    {
      dif = 2;
      this.DialogResult = System.Windows.Forms.DialogResult.No;
    }

    public void button3_Click(object sender, EventArgs e)
    {
      dif = 3;
      this.DialogResult = System.Windows.Forms.DialogResult.No;
    }
  }
}
