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
            SpreadsheetControl.Instance.MainForm = this;
            InitializeComponent();
            ExecuteInsertWorksheet(null, null);
            foreach (FontFamily f in FontFamily.Families)
                this.fontSelectionBox.Items.Add(f.Name);
            for (int i = 0; i < 30; i++)
            {
                this.fontSizeSelectionBox.Items.Add(""+i);
            }
            this.fontSelectionBox.SelectedIndex = 0;
            this.fontSizeSelectionBox.SelectedIndex = 0;
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
        }
        private void ExecuteInsertBarGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertBarGraph);
        }

        private void ExecuteInsertColumnGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertColumnGraph);
        }

        private void ExecuteInsertScatterGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertScatterGraph);
        }

        private void ExecuteInsertLineGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertLineGraph);
        }

        private void ExecuteInsertPieGraph(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.InsertPieGraph);            
        }
        private void increaseFont_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void decreaseFont_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void bold_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void italic_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void underline_Click(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCommand(sender, e, CommandType.FormatCells);
        }

        private void TextColor_Click(object sender, EventArgs e)
        {

        }

      private void selectTextColorbtn_Click(object sender, EventArgs e)
      {
        System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
        colorDialog.AnyColor = true;
        colorDialog.ShowHelp = false;

        if (colorDialog.ShowDialog() != DialogResult.Cancel)
        {
          
          Controller.Instance.ExecuteCommand(sender, new ColorEventArgs(colorDialog.Color), CommandType.SelectTextColor);
        }
      }

        private void selectCellColorbtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.AnyColor = true;
            colorDialog.ShowHelp = false;

            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {

                Controller.Instance.ExecuteCommand(sender, new ColorEventArgs(colorDialog.Color), CommandType.SelectCellColor);
            }
        }

        private void fontSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox fontBox = (sender as ToolStripComboBox);
            Controller.Instance.ExecuteCommand(sender, new FontEventArgs((fontBox.SelectedItem as string), -1), CommandType.ChangeFont);
        }

        private void fontSizeSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox sizeBox = (sender as ToolStripComboBox);
            if (sizeBox.SelectedItem != null)
            {
                float size = float.Parse((sizeBox.SelectedItem as string));
                Controller.Instance.ExecuteCommand(sender, new FontEventArgs(null, size), CommandType.ChangeFont);
            }
        }
    }
}