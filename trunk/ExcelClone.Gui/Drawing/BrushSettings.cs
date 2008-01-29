using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Gui
{
    public class BrushSettings : IBrushSettings
    {
        public BrushSettings(Color color)
            : this(color, BrushStyle.Solid) { }

        public BrushSettings(Color color, BrushStyle brushStyle)
            : this(color, Color.White, brushStyle) { }
        
        public BrushSettings(Color color, Color backColor, BrushStyle brushStyle)
        {
            Color = color;
            BackColor = backColor;
            BrushStyle = brushStyle;
        }

        private Color color;
        public Color Color { get { return color; } private set { color = value; } }

        private BrushStyle brushStyle;
        public BrushStyle BrushStyle { get { return brushStyle; } private set { brushStyle = value; } }

        private Color backColor;
        public Color BackColor { get { return backColor; } private set { backColor = value; } }

    }
}
