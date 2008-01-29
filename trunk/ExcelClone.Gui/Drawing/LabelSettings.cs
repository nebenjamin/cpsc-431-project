using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Gui
{
    public class LabelSettings : ILabelSettings
    {
        public LabelSettings(IBrushSettings background, IPenSettings borderPen, IFontSettings font, 
            IBrushSettings fontBrush, ContentAlignment contentAlignment)
        {
            Background = background;
            BorderPen = borderPen;
            Font = font;
            FontBrush = fontBrush;
            ContentAlignment = contentAlignment;
        }

        private IBrushSettings background;
        public IBrushSettings Background { get { return background; } private set { background = value; } }

        private IBrushSettings fontBrush;
        public IBrushSettings FontBrush { get { return fontBrush; } private set { fontBrush = value; } }

        private IPenSettings borderPen;
        public IPenSettings BorderPen { get { return borderPen; } private set { borderPen = value; } }

        private IFontSettings font;
        public IFontSettings Font { get { return font; } private set { font = value; } }

        private ContentAlignment contentAlignment;
        public ContentAlignment ContentAlignment { get { return contentAlignment; } private set { contentAlignment = value; } }
    }
}
