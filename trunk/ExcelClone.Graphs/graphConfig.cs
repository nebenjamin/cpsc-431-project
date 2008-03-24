using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExcelClone.Graphs
{
    public partial class graphConfig : Form
    {

        private GraphControl graphControl1;

        public graphConfig()
        {
            InitializeComponent();
            this.tabPage3.Controls.Add(this.graphControl1);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.tabPage3.Controls.Remove(this.graphControl1);
            switch (comboBox1.SelectedIndex)
            {
                // Bar Graph
                case 0:
                    this.graphControl1 = new GraphControl(Graph_Type.Bar);
                    break;
                // Column Graph
                case 1:
                    this.graphControl1 = new GraphControl(Graph_Type.Column);
                    break;
                // Line Graph
                case 2:
                    this.graphControl1 = new GraphControl(Graph_Type.Line);
                    break;
                // Scatter Graph
                case 3:
                    this.graphControl1 = new GraphControl(Graph_Type.Scatter);
                    break;
                // Pie Graph
                case 4:
                    this.graphControl1 = new GraphControl(Graph_Type.Pie);
                    break;
            }
            this.graphControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.graphControl1.BackColor = System.Drawing.Color.Black;
            this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl1.Location = new System.Drawing.Point(0, 0);
            this.graphControl1.Name = "graphControl1";
            this.graphControl1.Size = new System.Drawing.Size(292, 266);
            this.graphControl1.TabIndex = 2;
            this.graphControl1.VSync = true;
            this.tabPage3.Controls.Add(this.graphControl1);
        }
    }
}