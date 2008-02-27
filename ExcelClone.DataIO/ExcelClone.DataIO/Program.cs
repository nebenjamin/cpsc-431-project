using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExcelClone.TestHarness
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      ExcelClone.DataIO.DataIO data = new ExcelClone.DataIO.DataIO(@"c:\Documents and Settings\" + Environment.UserName.ToString() + @"\Desktop\blah.xml");
      bool Success = data.SaveBook();
      MessageBox.Show(Success.ToString());
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new ExcelClone.Gui.Window());
    }
  }
}