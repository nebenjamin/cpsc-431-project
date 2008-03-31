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
            Controller.Instance.ExecuteNew(sender, e);
        }
        private void ExecuteOpen(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteOpen(sender, e);
        }
        private void ExecuteClose(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteClose(sender, e);
        }
        private void ExecuteSave(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteSave(sender, e);
        }
        private void ExecuteSaveAs(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteSaveAs(sender, e);
        }
        private void ExecuteCut(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCut(sender, e);
        }
        private void ExecuteCopy(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteCopy(sender, e);
        }
        private void ExecutePaste(object sender, EventArgs e)
        {
            Controller.Instance.ExecutePaste(sender, e);
        }
        private void ExecuteChart(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteChart(sender, e);
        }
        private void ExecuteInsertWorksheet(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteInsertWorksheet(sender, e);
        }
        private void ExecuteInsertFunction(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteInsertFunction(sender, e);
        }
        private void ExecuteExit(object sender, EventArgs e)
        {
            Controller.Instance.ExecuteExit(sender, e);
        }
    }
}