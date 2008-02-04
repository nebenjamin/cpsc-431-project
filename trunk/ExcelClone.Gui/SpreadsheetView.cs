using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using System.Windows.Forms;
using System.Drawing;

namespace ExcelClone.Gui
{
    public class SpreadsheetView : DataGridView
    {
        public SpreadsheetView()
        {

            for (int k = 0; k < 26; k++)
                Columns.Add(MakeColumnLabel(k), MakeColumnLabel(k));

            Rows.Add(50);
        }

        public void RefreshCell(CellKey key)
        {
            if (gridModel == null)
                return;

            this[key.C, key.R].Value = GridModel.Cells[key].Value;
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            
            while (strRowNumber.Length < this.RowCount.ToString().Length) 
                strRowNumber = " " + strRowNumber;

            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);

            if (this.RowHeadersWidth < (int)(size.Width + 20)) 
                this.RowHeadersWidth = (int)(size.Width + 20);

            Brush b = SystemBrushes.ControlText;

            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));

            base.OnRowPostPaint(e);
        }

        public void RefreshCell(int r, int c) { RefreshCell(new CellKey(r, c)); }

        public static string MakeColumnLabel(int col)
        {
            string send = (char)(col % 26 + 'A') + "";
            col /= 26;

            while (col > 0)
            {
                send = (char) (col % 26 - 1 + 'A') + send;
                col /= 26;
            }
            

            
            return send;
        }

        private IGridModel gridModel;
        public IGridModel GridModel { get { return gridModel; } set { gridModel = value; } }
    }
}
