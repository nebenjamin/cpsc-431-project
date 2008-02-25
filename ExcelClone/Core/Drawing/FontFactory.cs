using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public static class FontFactory
    {
        public static Font CreateFont(IFontSettings fontSettings)
        {
            FontStyle style = (fontSettings.Bold ? FontStyle.Bold : 0) | 
                (fontSettings.Italics ? FontStyle.Italic : 0) | 
                (fontSettings.Underlined ? FontStyle.Underline : 0);

            return new Font(new FontFamily(fontSettings.FontFamily), fontSettings.EmSize, style);
        }
    }

}
