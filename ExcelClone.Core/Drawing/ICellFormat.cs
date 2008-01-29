using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface ICellFormat
    {
        IFontSettings Font { get; }
        IBrushSettings FontBrush { get; }
        IBrushSettings BackgroundBrush { get; }
        IPenSettings BorderPen { get; }
    }
}
