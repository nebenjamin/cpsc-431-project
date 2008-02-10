using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
  class SpreadsheetControl : ISpreadsheetControl
  {
    private Stack<CellKey> InvalidCells;

    public SpreadsheetControl()
    {
      InvalidCells = new Stack<CellKey>();
    }

    #region ISpreadsheetControl Members

    public void InvalidateCell(CellKey key)
    {
      InvalidCells.Push(key);
      Enumerator e = InvalidCells.GetEnumerator();
      
      while (e.MoveNext())
      {
        //while has next, call the process function
        //in the parser.  Parser will invalidate a group
        //of cells and this function will continue

      }
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
