using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ExcelClone.Gui
{
    public partial class WindowNoSpreadSheet : Form
    {
        public WindowNoSpreadSheet()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickOpen();
        }

        private void closeMenuItem1_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickClose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickSaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCopy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickPaste();
        }

        private void worksheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickInsertWorksheet();
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickChart();
        }

        private void functionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickInsertFunction();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickNew();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickOpen();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickClose();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickSave();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickSaveAs();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCut();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCopy();
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickPaste();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickChart();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickCopy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Controller.Instance.ClickPaste();
        }
    }
}