using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public interface ICellFormat : ICloneable
    {
        Font CellFont { get; set; }
        Color TextColor { get; set; }
        Color CellColor { get; set; }
        Boolean IsDefault { get; set;}
        Int32 CellWidth { get;set;}
        Int32 CellHeight { get;set;}
    }
}
