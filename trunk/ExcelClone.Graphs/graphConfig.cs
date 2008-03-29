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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    gr.TitleString = textBox1.Text;
                    gr.XLabelString = textBox2.Text;
                    gr.YLabelString = textBox3.Text;
                    break;
            }

        }
    }
}