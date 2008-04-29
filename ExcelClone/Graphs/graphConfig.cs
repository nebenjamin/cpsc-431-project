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

            listBox2.DataSource = gr.LegendColors;
            listBox1.DataSource = gr.LegendLabels;

        }

        private void setConfigTabAccess()
        {
            if (gr.LegendColors.Count < gr.LegendLabels.Count)
            {
                tabControl1.SelectedIndex = 0;
            }
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

        private void button3_Click(object sender, EventArgs e)
        {

            gr.LegendLabels[listBox1.SelectedIndex] = textBox2.Text;

            if (listBox1.SelectedIndex < gr.LegendLabels.Count-1)
                listBox1.SelectedIndex++;

            else if (listBox1.SelectedIndex == gr.LegendLabels.Count-1)
                listBox1.SelectedIndex = 0;

            listBox1.DataSource = null;
            listBox1.DataSource = gr.LegendLabels;

            textBox2.Text = gr.LegendLabels[listBox1.SelectedIndex];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if((listBox2.SelectedItem != null) & (gr.LegendColors.Count > gr.LegendLabels.Count))
            {
                gr.LegendColors.RemoveAt(listBox2.SelectedIndex);

                listBox2.DataSource = null;
                listBox2.DataSource = gr.LegendColors;
             }


            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {

                colorDialog1.ShowDialog();
                gr.LegendColors.Add(colorDialog1.Color);

                listBox2.DataSource = null;
                listBox2.DataSource = gr.LegendColors;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}