SpreadsheetModel : IGridModel
Add(Cell, int, int);

IGridModel
-ICellCollection

ICellCollection
-ICell this[r](int.md)[c](int.md)


ICell
-string Formula { get; }
-string Value { get; }
-void Invalidate();
-ICellFormat Format { get; }

ICellFormat
-IFontSettings Font { get; }
-IBrushSettings FontBrush { get; }
-IBrushSettings BackgroundBrush { get; }
-IPenSettings BorderPen { get; }