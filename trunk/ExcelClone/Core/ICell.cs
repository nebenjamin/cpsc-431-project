using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface ICell : ICloneable
    {
        string Formula { get; }
        string Value { get; }
        void Invalidate();
        ExcelClone.Core.ICellFormat CellFormat { get; }
    }
}
