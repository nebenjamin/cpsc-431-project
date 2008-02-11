using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ExcelClone.Core
{
  class SpreadsheetControl : ISpreadsheetControl
  {    
    public SpreadsheetControl()
    {
    }

    #region ISpreadsheetControl Members

    public void InvalidateCell(CellKey key)
    {
      //call the parse function for this cell key
    }

    public void InvalidateCell(int r, int c)
    {
      this.InvalidateCell(new CellKey(r, c));
    }    

    public void UpdateCellValue(CellKey key, string value)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void UpdateCellFormula(CellKey key, string formula)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ClearCell(CellKey key)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
