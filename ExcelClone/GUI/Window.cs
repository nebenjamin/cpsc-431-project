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
            Rectangle r = new Rectangle( 0,50,400,300 );
            string[][] data = Graphs.Graph.sampleData();

            Graphs.Graph gr = Graphs.bar_graph.Create_Bar_Graph(this, r, data);
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