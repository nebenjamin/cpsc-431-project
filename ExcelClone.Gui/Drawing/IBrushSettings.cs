using System;
namespace ExcelClone.Gui
{
    public interface IBrushSettings
    {
        System.Drawing.Color Color { get; }
        System.Drawing.Color BackColor { get; }
        BrushStyle BrushStyle { get; }
    }
}
