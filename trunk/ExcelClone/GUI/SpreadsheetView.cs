using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ExcelClone.Core;

namespace ExcelClone.Gui
{
    [System.ComponentModel.ReadOnly(true)]
    public class SpreadsheetView : DataGridView
    {
        public SpreadsheetView()
            : base()
        {
            Dock = DockStyle.Fill;
            CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(SpreadsheetView_CellMouseDoubleClick);
            RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(SpreadsheetView_RowHeaderMouseClick);
            
            KeyDown += new KeyEventHandler(SpreadsheetView_KeyDown);
            ParentChanged += delegate
            {
                Columns.Clear();

                for (int k = 0; k < 26; k++)
                {
                    Columns.Add(MakeColumnLabel(k), MakeColumnLabel(k));
                    Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                Rows.Add(50);

                AllowUserToOrderColumns = false;
                SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            };

            CellEndEdit += new DataGridViewCellEventHandler(SpreadsheetView_CellEndEdit);
            CellBeginEdit += new DataGridViewCellCancelEventHandler(SpreadsheetView_CellBeginEdit);
        }

        void SpreadsheetView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            SpreadsheetModel model = Controller.Instance.SpreadsheetModel;

            Cell cell = model.Cells[row, col];

            if (cell == null)
            {
                cell = new Cell();
                model.Cells[row, col] = cell;
            }

            this.Rows[row].Cells[col].Value = cell.Formula;
        }

        void SpreadsheetView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            SpreadsheetControl.Instance.CellChanged(new CellKey(row, col));
            SpreadsheetModel model = Controller.Instance.SpreadsheetModel;

            Cell cell = model.Cells[row, col];

            if (cell == null)
            {
                cell = new Cell();
                model.Cells[row, col] = cell;
            }

            cell.Formula = this.Rows[row].Cells[col].Value + "";

            cell.Value = Controller.Instance.Parser.Parse(MakeColumnLabel(col) + row + ":" + cell.Formula);
            this.Rows[row].Cells[col].Value = cell.Value;

        }
            
        void SpreadsheetView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearSelection();
            foreach(DataGridViewCell cell in Rows[e.RowIndex].Cells)
                cell.Selected = true;
        }

        void SpreadsheetView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && !CurrentCell.IsInEditMode)
                foreach (DataGridViewCell cell in SelectedCells)
                    cell.Value = "";
        }

        void SpreadsheetView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CurrentCell = this[e.ColumnIndex, e.RowIndex];
            BeginEdit(false);
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
