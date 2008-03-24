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
            TextColor = textColor;
            CellColor = cellColor;
        }

        private Font _cellFont;
        public Font CellFont
        {
            get
            {
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
    }
}
