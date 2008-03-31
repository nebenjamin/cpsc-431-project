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
        }
        private void ExecuteOpen(object sender, EventArgs e)
        {
        }
        private void ExecuteClose(object sender, EventArgs e)
        {
        }
        private void ExecuteSave(object sender, EventArgs e)
        {
        }
        private void ExecuteSaveAs(object sender, EventArgs e)
        {
        }
        private void ExecuteCut(object sender, EventArgs e)
        {
        }
        private void ExecuteCopy(object sender, EventArgs e)
        {
        }
        private void ExecutePaste(object sender, EventArgs e)
        {
        }
        private void ExecuteChart(object sender, EventArgs e)
        {
        }
        private void ExecuteInsertWorksheet(object sender, EventArgs e)
        {
        }
        private void ExecuteInsertFunction(object sender, EventArgs e)
        {
        }
        private void ExecuteExit(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteExit(sender, e);
        }
    }
}