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
            // Sets graph title if checkDrawTitle is checked.

            if (gr.draw_title)
            {
                GraphTitleBox.Text = gr.TitleString;
                checkDrawTitle.Checked = true;
            }

            checkDrawLegend.Checked = gr.LegendOn;

            listBoxGraphColors.DataSource = gr.LegendColors;
            listBoxGraphSeries.DataSource = gr.LegendLabels;

        }


        private void GraphTitleBox_TextChanged(object sender, EventArgs e)
        {
            gr.TitleString = GraphTitleBox.Text;
        }

        private void checkDrawGraphTitle_CheckedChanged(object sender, EventArgs e)
        {
            gr.draw_title = checkDrawTitle.Checked;
        }

        private void checkDrawLegend_CheckedChanged(object sender, EventArgs e)
        {
            gr.LegendOn = checkDrawLegend.Checked;
        }

        private void buttonRenameSeries_Click(object sender, EventArgs e)
        {

            gr.LegendLabels[listBoxGraphSeries.SelectedIndex] = textBoxRename.Text;

            if (listBoxGraphSeries.SelectedIndex < gr.LegendLabels.Count-1)
                listBoxGraphSeries.SelectedIndex++;

            else if (listBoxGraphSeries.SelectedIndex == gr.LegendLabels.Count-1)
                listBoxGraphSeries.SelectedIndex = 0;

            listBoxGraphSeries.DataSource = null;
            listBoxGraphSeries.DataSource = gr.LegendLabels;

            textBoxRename.Text = gr.LegendLabels[listBoxGraphSeries.SelectedIndex];
        }

        private void buttonRemoveColor_Click(object sender, EventArgs e)
        {

            // Check to ensure LegendColors does not underrun LegendLabels
            if((listBoxGraphColors.SelectedItem != null) & (gr.LegendColors.Count > gr.LegendLabels.Count))
            {
                //Removes the selected color from LegendColors, updates listBoxGraphColors
                gr.LegendColors.RemoveAt(listBoxGraphColors.SelectedIndex);

                listBoxGraphColors.DataSource = null;
                listBoxGraphColors.DataSource = gr.LegendColors;
             }
            
        }

        private void buttonAddColor_Click(object sender, EventArgs e)
        {
            if (listBoxGraphSeries.SelectedItem != null)
            {
                //Opens Add Color Dialog, Adds Result, Updates listBoxGraphColors.
                colorDialog1.ShowDialog();
                gr.LegendColors.Add(colorDialog1.Color);

                listBoxGraphColors.DataSource = null;
                listBoxGraphColors.DataSource = gr.LegendColors;

            }
        }

        private void GraphTitleBox_Click(object sender, EventArgs e)
        {
            GraphTitleBox.Text = "";
            checkDrawTitle.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // OK Button Click
        }

    }
}