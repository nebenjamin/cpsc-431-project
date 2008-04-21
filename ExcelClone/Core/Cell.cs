using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using System.Drawing;

namespace ExcelClone.Core
{
    public class Cell : ICell, ICloneable
    {
        public Cell(string formula, CellFormat cellFormat)
        {
            Error = false;
            Formula = formula;
            CellFormat = cellFormat;
        }
        public object Clone()
        {
            Cell cell = new Cell();
            cell.CellFormat = this.CellFormat;
            cell.Formula = this.Formula;
            cell.valid = this.Valid;
            cell.Value = this.Value;
            cell.Error = this.Error;
            cell.ErrorString = this.ErrorString;
            return cell;
        }

        public Cell(string formula)
            : this(formula, null) { }

        public Cell(CellFormat cellFormat)
            : this(null, cellFormat) { }

        public Cell() {
            CellFormat = new CellFormat(new Font("Times", 12), Color.Black, Color.White);
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

        private bool error;
        public bool Error { get { return error; } set { error = value; } }

        private string errorString;
        public string ErrorString { get { return errorString; } set { errorString = value; } }

        private CellFormat cellFormat;
        public CellFormat CellFormat { get { return cellFormat; } set { cellFormat = value; } }
    }
}
