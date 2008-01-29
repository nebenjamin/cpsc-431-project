﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelClone.Gui
{
    public class PenFactory
    {
        public static Pen CreatePen(IPenSettings penSettings)
        {
            return new Pen(penSettings.Color);
        }
    }
}
