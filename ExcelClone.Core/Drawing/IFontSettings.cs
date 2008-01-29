using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public interface IFontSettings
    {
        string FontFamily { get; }
        bool Bold { get; }
        bool Italics { get; }
        bool Underlined { get; }
        float EmSize { get; }
    }
}
