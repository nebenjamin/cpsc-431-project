using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using System.Windows.Forms;

namespace ExcelClone.Gui
{
    public class SpreadsheetView : DataGridView
    {
        public SpreadsheetView()
        {
            
        }

        public void RefreshCell(CellKey key)
        {
            if (gridModel == null)
                return;


        }

        public void RefreshCell(int r, int c) { RefreshCell(new CellKey(r, c)); }

        private IGridModel gridModel;
        public IGridModel GridModel { get { return gridModel; } set { gridModel = value; } }
    }
}
