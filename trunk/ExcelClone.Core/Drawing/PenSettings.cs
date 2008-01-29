using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public class PenSettings : IPenSettings
    {
        public PenSettings(Color color)
        {
            Color = color;
        }

        private Color color;
        public Color Color { get { return color; } private set { color = value; } }
    }
}
