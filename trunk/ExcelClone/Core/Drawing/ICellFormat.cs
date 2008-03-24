using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public interface ICellFormat
    {
        Font CellFont { get; set; }
        Color TextColor { get; set; }
        Color CellColor { get; set; }
    }
}
