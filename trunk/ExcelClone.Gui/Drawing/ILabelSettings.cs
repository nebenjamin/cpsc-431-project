using System;
namespace ExcelClone.Gui
{
    public interface ILabelSettings
    {
        IBrushSettings Background { get; }
        IBrushSettings FontBrush { get; }
        IFontSettings Font { get; }
        IPenSettings BorderPen { get; }
        System.Drawing.ContentAlignment ContentAlignment { get; }
    }
}
