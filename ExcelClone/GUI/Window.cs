using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ExcelClone.Gui
{
    public partial class Window : Form
    {
        public Window()
        {
            Controller.Instance.MainForm = this;
            SpreadsheetView.Instance.KeyDown += new KeyEventHandler(SpreadsheetView_KeyDown);
            InitializeComponent();
        }
        private void ExecuteNew(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.New);
            //Controller.Instance.ExecuteNew(sender, e);
        }
        private void ExecuteOpen(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Open);
        }
        private void ExecuteClose(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Close);
        }
        private void ExecuteSave(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Save);
        }
        private void ExecuteSaveAs(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.SaveAs);
        }
        private void ExecuteCut(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Cut);
        }
        private void ExecuteCopy(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Copy);
        }
        private void ExecutePaste(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Paste);
        }
        private void ExecuteInsertWorksheet(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertWorksheet);
        }
        private void ExecuteInsertFunction(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertFunction);
        }
        private void ExecuteExit(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Exit);
        }
        private void ExecuteInsertBarGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertBarGraph);
        }

        private void ExecuteInsertColumnGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertColumnGraph);
        }

        private void ExecuteInsertScatterGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertScatterGraph);
        }

        private void ExecuteInsertLineGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertLineGraph);
        }

        private void ExecuteInsertPieGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertPieGraph);
        }
        private void SpreadsheetView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                ExecuteCopy(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                ExecuteCut(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                ExecutePaste(sender, e);
                e.Handled = true;
            }
        }

        private void increaseFont_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void decreaseFont_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void bold_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void italic_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void underline_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void TextColor_Click(object sender, EventArgs e)
        {

        }
    }
}