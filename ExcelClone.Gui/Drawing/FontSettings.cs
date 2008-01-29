using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Gui
{
    public class FontSettings : IFontSettings
    {
        public FontSettings(string fontFamily, float emSize, bool bold, bool italics, bool underlined)
        {
            FontFamily = fontFamily;
            EmSize = emSize;
            Bold = bold;
            Italics = italics;
            Underlined = underlined;
        }

        private string fontFamily;
        public string FontFamily { get { return fontFamily; } private set { fontFamily = value; } }

        private float emSize;
        public float EmSize { get { return emSize; } private set { emSize = value; } }

        private bool bold;
        public bool Bold { get { return bold; } private set { bold = value; } }

        private bool italics;
        public bool Italics { get { return italics; } private set { italics = value; } }

        private bool underlined;
        public bool Underlined { get { return underlined; } private set { underlined = value; } }
    }
}
