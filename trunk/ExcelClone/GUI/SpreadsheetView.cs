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
        public readonly int RowCount = 50;
        public readonly int ColumnCount = 26;

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


            this.DefaultCellStyle.Font = new Font("Verdana", 10);

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
            Console.WriteLine("called");
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            
            SpreadsheetModel model = Controller.Instance.SpreadsheetModel;

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
                        c = Controller.Instance.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
                        if (c != null)
                        {
                            c.Value = "";
                            c.Formula = "";
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
                if(Controller.Instance.SpreadsheetModel.Cells[key] != null)
                  this[key.C, key.R].Value = Controller.Instance.SpreadsheetModel.Cells[key].Value;
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

        public static SpreadsheetView Instance
        {
            get { return SpreadsheetViewCreator.CreatorInstance; }
        }

        private sealed class SpreadsheetViewCreator
        {
            private static readonly SpreadsheetView _instance = new SpreadsheetView();

            public static SpreadsheetView CreatorInstance
            {
                get { return _instance; }
            }
        }



        private IGridModel gridModel;
        public IGridModel GridModel { get { return gridModel; } set { gridModel = value; } }
    }
}
