using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ExcelClone.Core;
using System.Collections;

namespace ExcelClone.Gui
{    

    [System.ComponentModel.ReadOnly(true)]
    public class SpreadsheetView : DataGridView
    {
        public readonly int RowCount = 50;
        public readonly int ColumnCount = 26;
        
        private SpreadsheetModel spreadsheetModel;
        public SpreadsheetModel SpreadsheetModel
        {
            get { return spreadsheetModel; }
            set { spreadsheetModel = value; }
        }
        public SpreadsheetView()
            : base()
        {
            SpreadsheetModel = new SpreadsheetModel(new CellCollection());            
            Dock = DockStyle.Fill;
            CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(SpreadsheetView_CellMouseDoubleClick);
            CellMouseClick += new DataGridViewCellMouseEventHandler(SpreadsheetView_CellMouseClick);
            RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(SpreadsheetView_RowHeaderMouseClick);
            RowHeightChanged += new DataGridViewRowEventHandler(SpreadsheetView_RowHeightChanged);
            ColumnWidthChanged += new DataGridViewColumnEventHandler(SpreadsheetView_ColumnWidthChanged);
            KeyDown += new KeyEventHandler(SpreadsheetView_KeyDown);
            KeyUp += new KeyEventHandler(SpreadsheetView_KeyUp);
            ParentChanged += delegate
            {
                Columns.Clear();

                for (int k = 0; k < ColumnCount; k++)
                {
                    Columns.Add(MakeColumnLabel(k), MakeColumnLabel(k));
                    Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                Rows.Add(RowCount);

                AllowUserToOrderColumns = false;
                SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            };

            CellEndEdit += new DataGridViewCellEventHandler(SpreadsheetView_CellEndEdit);
            CellBeginEdit += new DataGridViewCellCancelEventHandler(SpreadsheetView_CellBeginEdit);
            RowsRemoved += new DataGridViewRowsRemovedEventHandler(SpreadsheetView_RowsRemoved);

            this.DefaultCellStyle.Font = new Font("Times", 12);
        }

        void SpreadsheetView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)  
                && !CurrentCell.IsInEditMode)
            {
                SpreadsheetView_CellMouseClick(sender, new DataGridViewCellMouseEventArgs(SelectedCells[0].ColumnIndex, SelectedCells[0].RowIndex, 0, 0, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
            }
        }

        void SpreadsheetView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int col = e.Column.Index;

            SpreadsheetModel model = spreadsheetModel;

            for (int i = 0; i < RowCount; i++)
            {
                Cell cell = model.Cells[i, col];

                if (cell == null)
                {
                    cell = new Cell();
                    model.Cells[i,col] = cell;
                }
                cell.CellFormat.CellWidth = this.Columns[col].Width;
            }
        }

        void SpreadsheetView_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            int row = e.Row.Index;

            SpreadsheetModel model = spreadsheetModel;
            
            for(int i = 0; i < ColumnCount; i++) {
                Cell cell = model.Cells[row,i];

                if (cell == null)
                {
                    cell = new Cell();
                    model.Cells[row,i] = cell;
                }
                cell.CellFormat.CellHeight = this.Rows[row].Height;
            }
        }

        void SpreadsheetView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            char cellCol = (char)('A' + col);

            string cellPos = "";
            cellPos += cellCol;
            cellPos += (row+1);

            SpreadsheetModel model = spreadsheetModel;

            Cell cell = model.Cells[row, col];
            
            if (cell == null)
            {
                cell = new Cell();
                model.Cells[row, col] = cell;
            }

            Controller.Instance.MainForm.SelectedCellBox.Text = cellPos;
            Controller.Instance.MainForm.FormulaBox.Text = cell.Formula;
            Controller.Instance.MainForm.FontSizeSelectionBox.SelectedIndex = (int)cell.CellFormat.CellFont.Size;
            int i = Controller.Instance.MainForm.FontSelectionBox.Items.IndexOf(cell.CellFormat.CellFont.Name);
            Controller.Instance.MainForm.FontSelectionBox.SelectedIndex = i;
        }

        void SpreadsheetView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Console.WriteLine("row removed");
            //int row = e.RowIndex;
            //int col = e.
        }

        void SpreadsheetView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            SpreadsheetModel model = spreadsheetModel;

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
            Console.WriteLine("called");
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            if (row < 0 || col < 0)
                return;
            SpreadsheetModel model = spreadsheetModel;

            Cell cell = model.Cells[row, col];

            if (cell == null)
            {
                cell = new Cell();
                model.Cells[row, col] = cell;
            }

            cell.Formula = this.Rows[row].Cells[col].Value + "";
            SpreadsheetControl.Instance.CellChanged(new CellKey(row, col));
            //cell.Value = Controller.Instance.Parser.Parse(MakeColumnLabel(col) + row + ":" + cell.Formula);
            this.Rows[row].Cells[col].Value = model.Cells[row, col].Value;
        }
            
        void SpreadsheetView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearSelection();
            foreach(DataGridViewCell cell in Rows[e.RowIndex].Cells)
                cell.Selected = true;
        }

        void SpreadsheetView_KeyDown(object sender, KeyEventArgs e)
        {
            Cell c;
            if (e.KeyCode == Keys.Delete && !CurrentCell.IsInEditMode)
                foreach (DataGridViewCell cell in SelectedCells)
                {
                    cell.Value = "";
                    if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                    {
                        c = spreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
                        if (c != null)
                        {
                            c.Value = "";
                            c.Formula = "";
                            c.Error = false;
                            c.ErrorString = "";

                            SpreadsheetControl.Instance.CellChanged(new CellKey(cell.RowIndex, cell.ColumnIndex));
                        }
                    }
                }
        }

        void SpreadsheetView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //out of bounds double click
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            CurrentCell = this[e.ColumnIndex, e.RowIndex];
            BeginEdit(false);
        }

        public void RefreshCell(CellKey key)
        {
            //if null, we dont care!
            try
            {
                if (spreadsheetModel.Cells[key] != null)
                {
                    this[key.C, key.R].Value = spreadsheetModel.Cells[key].Value;
                    this[key.C, key.R].Style.Font = spreadsheetModel.Cells[key].CellFormat.CellFont;
                    this[key.C, key.R].Style.ForeColor = spreadsheetModel.Cells[key].CellFormat.TextColor;
                    this[key.C, key.R].Style.BackColor = spreadsheetModel.Cells[key].CellFormat.CellColor;
                    Controller.Instance.MainForm.FontSizeSelectionBox.SelectedIndex = (int)spreadsheetModel.Cells[key].CellFormat.CellFont.Size;
                    this.Columns[key.C].Width = spreadsheetModel.Cells[key].CellFormat.CellWidth;
                    this.Rows[key.R].Height = spreadsheetModel.Cells[key].CellFormat.CellHeight;
                }
                else
                    this[key.C, key.R].Value = null;
            }
            catch (NullReferenceException e)
            {
            }
        }

        public void RefreshView()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    RefreshCell(j, i);
                }
            }
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
