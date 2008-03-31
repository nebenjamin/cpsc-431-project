using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public static class CellFormatFactory
    {
        public static CellFormat createCellFormat(string settings)
        {
            string[] s = settings.Split(',');
            CellFormat c = new CellFormat();
            FontStyle style =  (((int.Parse(s[(int)SettingPosition.Bold]) == 1)? FontStyle.Bold : 0) | 
                ((int.Parse(s[(int)SettingPosition.Italic]) == 1)? FontStyle.Italic : 0) |
                ((int.Parse(s[(int)SettingPosition.Underline]) == 1)? FontStyle.Underline : 0));
            c.CellColor = Color.FromArgb(int.Parse(s[(int)SettingPosition.CellColor]));
            c.TextColor = Color.FromArgb(int.Parse(s[(int)SettingPosition.TextColor]));
            
            c.CellFont = FontFactory.CreateFont(s[(int)SettingPosition.FontFamily], 
                float.Parse(s[(int)SettingPosition.FontSize]), style);
            return c;
        }
    }

    public enum SettingPosition
    {
        Bold = 0,
        Italic = 1,
        Underline = 2,
        CellColor = 3,
        TextColor = 4,
        FontFamily = 5,
        FontSize = 6
    }
}
