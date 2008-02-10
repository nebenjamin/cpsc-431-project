using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
  interface ISpreadsheetControl
  {
    void InvalidateCell(CellKey key);
    void InvalidateCell(int r, int c);
    void UpdateCellValue(CellKey key, string value);
    void UpdateCellFormula(CellKey key, string formula);
    void ClearCell(CellKey key);
  }
}
