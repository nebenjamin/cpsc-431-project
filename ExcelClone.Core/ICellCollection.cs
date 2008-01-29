using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface ICellCollection
    {
        ICell this[int r, int c] { get; }
        int Rows { get; }
        int Columns { get; }
    }
}
