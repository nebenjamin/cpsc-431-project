using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ExcelClone.Core
{
    class SpreadsheetControl : ISpreadsheetControl
    {
        private SpreadsheetModel m;
        private Queue<CellKey> InvalidCells;

        public SpreadsheetControl()
        {
            m = new SpreadsheetModel(new CellCollection());
            InvalidCells = new Queue<CellKey>();
        }

        #region ISpreadsheetControl Members

        public void InvalidateCell(CellKey key)
        {
            InvalidCells.Enqueue(key);
        }

        public void CellChanged(CellKey key)
        {
            InvalidateCell(key);
            while (InvalidCells.Count > 0)
            {
                //updateCellValue(key, parse(key, value)); call the parse function for this cell key
            }
        }

        public void InvalidateCell(int r, int c)
        {
            this.InvalidateCell(new CellKey(r, c));
        }

        public void UpdateCellValue(CellKey key, string value)
        {
            m.Cells[key].Value = value;
        }

        public void UpdateCellFormula(CellKey key, string formula)
        {
            m.Cells[key].Formula = formula;
            //repaint
        }

        public void ClearCell(CellKey key)
        {
            m.Cells[key].Formula = null;
            m.Cells[key].Value = "";
            //repaint
        }

        public void LoadSheet(ICellCollection c)
        {
            m.Cells = (CellCollection)c;
            //change  view!           
        }

        #endregion
    }
}
