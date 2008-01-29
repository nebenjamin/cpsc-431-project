using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface ICell
    {
        string Formula { get; }
        string Value { get; }
        void Invalidate();
        ExcelClone.Gui.ICellFormat CellFormat { get; }
    }
}
