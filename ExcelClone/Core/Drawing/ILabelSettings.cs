using System;
namespace ExcelClone.Core
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
