using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExcelClone.Graphs
{
    public partial class Form1 : Form
    {
        Graph gr;

        public Form1()
        {
            InitializeComponent();
        }

        public static void Main()
        {
            Application.Run(new Form1());
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle( 0,50,400,300 );
            string[][] data = sampleData();

            switch (comboBox1.SelectedIndex)
            {
                // Bar Graph
                case 0:
                    gr = bar_graph.Create_Bar_Graph(this, r, data );
                    break;
                // Column Graph
                case 1:
                    gr = column_graph.Create_Column_Graph(this, r, data);
                    break;
                // Line Graph
                case 2:
                    gr = line_graph.Create_Line_Graph(this, r, data);
                    break;
                // Scatter Graph
                case 3:
                    gr = scatter_graph.Create_Scatter_Graph(this, r, data);
                    break;
                // Pie Graph
                case 4:
                    gr = pie_graph.Create_Pie_Graph(this, r, data);
                    break;
            }
        }

        public string[][] sampleData()
        {
            Random ran = new Random();
            int columns = 1;//ran.Next(2,4);
            int rows = 7;//ran.Next(3, 7);

            string[][] data = new string[columns][];

            for (int i = 0; i < columns; i++)
            {
                data[i] = new string[rows];
                for (int j = 0; j < rows; j++)
                {
                    data[i][j] = ran.Next(5, 80).ToString();
                }
            }

            return data;
        }
    }
}