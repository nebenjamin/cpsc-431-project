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
        // the graph that will be modified
        public Graph gr;

        public graphConfig( Graph g, GraphControl gcont )
        {
            InitializeComponent();
            // will not allow to open another config window in this config window
            gcont.inMenu = true;
            // copies graph so that can modify else where
            gr = g;
            // put in the graph specific settings in tab 2
            gr.configTab(tabPage2);
            // put a preview graph in tab 3
            tabPage3.Controls.Add(gcont);
            // will put on settings that were chosen before on the graph
            InitConfig();
        }

        // will put on settings that were chosen before on the graph
        private void InitConfig()
        {
            if (gr.draw_title)
            {
                textBox1.Text = gr.TitleString;
                checkBox1.Checked = true;
            }

            checkBox2.Checked = gr.LegendOn;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            gr.TitleString = textBox1.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            gr.draw_title = checkBox1.Checked;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            checkBox1.Checked = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            gr.LegendOn = checkBox2.Checked;
        }
    }
}