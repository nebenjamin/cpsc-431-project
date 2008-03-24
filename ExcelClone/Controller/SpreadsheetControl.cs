using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ExcelClone.Core;
using ExcelClone.Gui;

namespace ExcelClone
{
    public class SpreadsheetControl : ISpreadsheetControl
    {        
        private Queue<CellKey> InvalidCells;

        public static SpreadsheetControl Instance
        {
            get { return SpreadsheetControlCreator.CreatorInstance; }
        }

        private sealed class SpreadsheetControlCreator
        {
            private static readonly SpreadsheetControl _instance = new SpreadsheetControl();

            public static SpreadsheetControl CreatorInstance
            {
                get { return _instance; }
            }
        }

        public SpreadsheetControl()
        {           
            InvalidCells = new Queue<CellKey>();
        }

        #region ISpreadsheetControl Members

        //enqueue invalid cell
        public void InvalidateCell(CellKey key)
        {
            InvalidCells.Enqueue(key);
        }

        //call on cell changes
        public void CellChanged(CellKey key)
        {
            CellKey lastUpdated = null;
            string val = null;
            InvalidateCell(key);
            while (InvalidCells.Count > 0)
            {
                key = InvalidCells.Dequeue();

                if (lastUpdated == key)
                {
                    //circular Reference
                }
                //val == null or value in new cell
                //val = parse(key, m.Cells[key].Value);
                if (val != null)
                {
                    UpdateCellValue(key, val);
                    lastUpdated = key;
                }
                val = Controller.Instance.Parser.Parse(SpreadsheetView.MakeColumnLabel(key.C) + key.R + ":" + Controller.Instance.SpreadsheetModel.Cells[key].Formula);
                UpdateCellValue(key, val);
                //updateCellValue(key, parse(key, m.Cells[key].value)); call the parse function for this cell key
            }
        }

        public void InvalidateCell(int r, int c)
        {
            this.InvalidateCell(new CellKey(r, c));
        }

        public void UpdateCellValue(CellKey key, string value)
        {
            Controller.Instance.SpreadsheetModel.Cells[key].Value = value;
            SpreadsheetView.Instance.RefreshCell(key);
        }

        public void UpdateCellFormula(CellKey key, string formula)
        {
            Controller.Instance.SpreadsheetModel.Cells[key].Formula = formula;
            SpreadsheetView.Instance.RefreshCell(key);
        }

        public void ClearCell(CellKey key)
        {
            Controller.Instance.SpreadsheetModel.Cells[key].Formula = null;
            Controller.Instance.SpreadsheetModel.Cells[key].Value = "";
            //repaint
        }

        public void LoadSheet(ICellCollection c)
        {
            Controller.Instance.SpreadsheetModel.Cells = (CellCollection)c;
            //change  view!           
        }

        #endregion
    }
}
