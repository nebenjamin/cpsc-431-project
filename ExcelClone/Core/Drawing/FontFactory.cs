﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Core
{
    public static class FontFactory
    {    

        public static Font CreateDefaultFont()
        {
            return new Font(new FontFamily("Verdana"),10);
        }

        public static Font CreateFont(string family, float size, FontStyle s)
        {
            return new Font(new FontFamily(family), size, s);
        }
    }

}
