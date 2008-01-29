using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public class SpreadsheetModel : IGridModel
    {
        public SpreadsheetModel(CellCollection cells)
        {
            Cells = cells;
        }

        private CellCollection cells;
        public CellCollection Cells { get { return cells; } set { cells = value; } }

        #region IGridModel Members

        ICellCollection IGridModel.Cells
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
