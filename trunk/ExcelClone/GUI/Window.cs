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
        private void ExecuteInsertGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertGraph);
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
            //Controller.Instance.ExecuteCommand(sender, e, CommandType.Exit);
        }

        private void barToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);

            //Done by David, Caleb, & Scott
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                if (rI < min_row)
                    min_row = rI;
                if (rI > max_row)
                    max_row = rI;
                if (cI < min_col)
                    min_col = cI;
                if (cI > max_col)
                    max_col = cI;
            }
            int colCount = max_col - min_col + 1;
            int rowCount = max_row - min_row + 1;

            if (colCount >= 2 && rowCount >= 1)
            {
                string[][] data = new string[rowCount][];
                for (int i = 0; i < data.Length; i++)
                    data[i] = new string[colCount];
                for (int i = 0; i < cellCount; i++)
                {
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
                }

                Graphs.Graph gr = Graphs.bar_graph.Create_Bar_Graph(this, r, data);
            }
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);
            string[][] data = Graphs.Graph.sampleData();

            Graphs.Graph gr = Graphs.column_graph.Create_Column_Graph(this, r, data);
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);
            string[][] data = Graphs.Graph.sampleData();

            Graphs.Graph gr = Graphs.line_graph.Create_Line_Graph(this, r, data);
        }

        private void scatterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);
            string[][] data = Graphs.Graph.sampleData();

            Graphs.Graph gr = Graphs.scatter_graph.Create_Scatter_Graph(this, r, data);
        }

        private void pieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);
            string[][] data = Graphs.Graph.sampleData();

            Graphs.Graph gr = Graphs.pie_graph.Create_Pie_Graph(this, r, data);
        }      
    }
}