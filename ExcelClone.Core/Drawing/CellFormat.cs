using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public class CellFormat : ICellFormat
    {
        #region ICellFormat Members

        IFontSettings ICellFormat.Font { get { return Font; } }

        IBrushSettings ICellFormat.FontBrush { get { return FontBrush; } }

        IBrushSettings ICellFormat.BackgroundBrush { get { return BackgroundBrush; } }

        IPenSettings ICellFormat.BorderPen { get { return BorderPen; } }

        #endregion

        private FontSettings font;
        public FontSettings Font { get { return font; } set { font = value; } }

        private BrushSettings fontBrush;
        public BrushSettings FontBrush { get { return fontBrush; } set { fontBrush = value; } }

        private BrushSettings backgroundBrush;
        public BrushSettings BackgroundBrush { get { return backgroundBrush; } set { backgroundBrush = value; } }

        private PenSettings borderPen;
        public PenSettings BorderPen { get { return borderPen; } set { borderPen = value; } }
    }
}
