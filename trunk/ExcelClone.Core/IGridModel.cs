using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface IGridModel
    {
        ICellCollection Cells { get; }
    }
}
