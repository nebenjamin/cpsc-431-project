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
            InitializeComponent();
        }
        private void ExecuteNew(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.New);
            //Controller.Instance.ExecuteNew(sender, e);
        }
        private void ExecuteOpen(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteOpen(sender, e);
        }
        private void ExecuteClose(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteClose(sender, e);
        }
        private void ExecuteSave(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteSave(sender, e);
        }
        private void ExecuteSaveAs(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteSaveAs(sender, e);
        }
        private void ExecuteCut(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCut(sender, e);
        }
        private void ExecuteCopy(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCopy(sender, e);
        }
        private void ExecutePaste(object sender, EventArgs e)
        {
            Controller.Instance.ExecutePaste(sender, e);
        }
        private void ExecuteChart(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteChart(sender, e);
        }
        private void ExecuteInsertWorksheet(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteInsertWorksheet(sender, e);
        }
        private void ExecuteInsertFunction(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteInsertFunction(sender, e);
        }
        private void ExecuteExit(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.Exit);
            //Controller.Instance.ExecuteExit(sender, e);
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