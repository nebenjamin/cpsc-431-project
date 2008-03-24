using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using System.Drawing;

namespace ExcelClone.Core
{
    public class Cell : ICell
    {
        public Cell(string formula, CellFormat cellFormat)
        {
            Formula = formula;
            CellFormat = cellFormat;
        }

        public Cell(string formula)
            : this(formula, null) { }

        public Cell(CellFormat cellFormat)
            : this(null, cellFormat) { }

        public Cell() {
            CellFormat = new CellFormat(new Font("Verdana", 14), Color.Red, Color.Blue);
        }

        #region ICell Members

        private string formula;
        public string Formula { get { return formula; } set { formula = value; } }
        
        private string val;
        public string Value { get { return val; } set { val = value; valid = true; } }

        private bool valid;
        public bool Valid { get { return valid; } }

        public void Invalidate() { valid = false; }

        ICellFormat ICell.CellFormat { get { return CellFormat; } }

        #endregion

        private CellFormat cellFormat;
        public CellFormat CellFormat { get { return cellFormat; } set { cellFormat = value; } }
    }
}
