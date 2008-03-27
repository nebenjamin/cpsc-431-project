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
            string[][] data = new string[1][];
            data[0] = new string[7];

            for (int i = 0; i < 7; i++)
                data[0][i] = "7";

            switch (comboBox1.SelectedIndex)
            {
                // Bar Graph
                case 0:
                    Rectangle r = new Rectangle( 0,50,400,300 );
                    gr = bar_graph.Create_Bar_Graph(this, r, data );
                    break;
                // Column Graph
                case 1:
                    break;
                // Line Graph
                case 2:
                    break;
                // Scatter Graph
                case 3:
                    break;
                // Pie Graph
                case 4:
                    break;
            }
        }
    }

            /*this.graphControl1 = new GraphControl(Graph_Type.Bar);
            this.graphControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.graphControl1.BackColor = System.Drawing.Color.Black;
            this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl1.Location = new System.Drawing.Point(0, 0);
            this.graphControl1.Name = "graphControl1";
            this.graphControl1.Size = new System.Drawing.Size(292, 266);
            this.graphControl1.TabIndex = 0;
            this.graphControl1.VSync = true;
            
            private GraphControl graphControl1;*/
}