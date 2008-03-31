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
        Graph gr;

        public graphConfig( Graph g, GraphControl gcont )
        {
            InitializeComponent();
            gr = g;
            gr.configTab(tabPage2);
            tabPage3.Controls.Add(gcont);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            gr.TitleString = textBox1.Text;
        }
    }
}