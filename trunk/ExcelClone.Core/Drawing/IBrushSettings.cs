using System;
namespace ExcelClone.Core
{
    public interface IBrushSettings
    {
        System.Drawing.Color Color { get; }
        System.Drawing.Color BackColor { get; }
        BrushStyle BrushStyle { get; }
    }
}
