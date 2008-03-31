using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public class CellFormat : ICellFormat
    {

        public CellFormat(Font f, Color textColor, Color cellColor) {
            CellFont = f;
            Console.WriteLine("FONT!!!! " + f.Bold);
            TextColor = textColor;
            CellColor = cellColor;
        }

        public CellFormat()
        {
        }

        private Font _cellFont;
        public Font CellFont
        {
            get
            {
                if (_cellFont == null)
                    _cellFont = FontFactory.CreateDefaultFont();
                return _cellFont;
            }
            set
            {
                _cellFont = value;
            }
        }

        private Color _textColor;
        public Color TextColor
        {
            get
            {
                if (_textColor == null)
                    _textColor = Color.Black;
                return _textColor;
            }
            set
            {
                _textColor = value;
            }
        }

        private Color _cellColor;
        public Color CellColor
        {
            get
            {
                if (_cellColor == null)
                    _cellColor = Color.White;
                return _cellColor;
            }
            set
            {
                _cellColor = value;
            }

        }

        private Boolean _isDefault;
        public Boolean IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                _isDefault = value;
            }
        }

        public string serialize()
        {
            /*IF YOU CHANGE THIS, you MUST change the order of the settings
             * in the SettingsPosition enum in the CellFormatFactory to match.
             * Then, you must change the factory method if you add a setting.
             */
            string o = "";
            o += ((CellFont.Bold)? 1:0) + ",";
            o += ((CellFont.Italic)? 1:0) + ",";
            o += ((CellFont.Underline)? 1:0) + ",";
            o += (int)CellColor.ToArgb() + ",";
            o += (int)TextColor.ToArgb() + ",";
            o += CellFont.FontFamily.Name + ",";
            o += CellFont.Size + "";
            return o;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is CellFormat)) 
                return false;
            else
            {
                CellFormat internalObject = (CellFormat)obj;
                if(internalObject.CellFont.Style == this.CellFont.Style &&
                    internalObject.CellFont.FontFamily.Name == this.CellFont.FontFamily.Name &&
                    internalObject.CellFont.Size == this.CellFont.Size &&
                    internalObject.CellColor.ToArgb() == this.CellColor.ToArgb() && 
                    internalObject.TextColor.ToArgb() == this.TextColor.ToArgb())
                    return true;
                else
                    return false;
            }            
        }
    }
}
