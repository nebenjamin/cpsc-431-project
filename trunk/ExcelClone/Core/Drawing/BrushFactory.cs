﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ExcelClone.Core
{
    public static class BrushFactory
    {
        public static Brush CreateBrush(IBrushSettings brushSettings)
        {
            if (brushSettings.BrushStyle == BrushStyle.Solid)
                return new SolidBrush(brushSettings.Color);
            else
                return new HatchBrush((HatchStyle)brushSettings.BrushStyle,
                    brushSettings.Color, brushSettings.BackColor);
        }
    }

    public enum BrushStyle
    {
        Horizontal = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3,
        Cross = 4,
        LargeGrid = 4,
        DiagonalCross = 5,
        Percent05 = 6,
        Percent10 = 7,
        Percent20 = 8,
        Percent25 = 9,
        Percent30 = 10,
        Percent40 = 11,
        Percent50 = 12,
        Percent60 = 13,
        Percent70 = 14,
        Percent75 = 15,
        Percent80 = 16,
        Percent90 = 17,
        LightDownwardDiagonal = 18,
        LightUpwardDiagonal = 19,
        DarkDownwardDiagonal = 20,
        DarkUpwardDiagonal = 21,
        WideDownwardDiagonal = 22,
        WideUpwardDiagonal = 23,
        LightVertical = 24,
        LightHorizontal = 25,
        NarrowVertical = 26,
        NarrowHorizontal = 27,
        DarkVertical = 28,
        DarkHorizontal = 29,
        DashedDownwardDiagonal = 30,
        DashedUpwardDiagonal = 31,
        DashedHorizontal = 32,
        DashedVertical = 33,
        SmallConfetti = 34,
        LargeConfetti = 35,
        ZigZag = 36,
        Wave = 37,
        DiagonalBrick = 38,
        HorizontalBrick = 39,
        Weave = 40,
        Plaid = 41,
        Divot = 42,
        DottedGrid = 43,
        DottedDiamond = 44,
        Shingle = 45,
        Trellis = 46,
        Sphere = 47,
        SmallGrid = 48,
        SmallCheckerBoard = 49,
        LargeCheckerBoard = 50,
        OutlinedDiamond = 51,
        SolidDiamond = 52,
        Solid = 100,
    }
}
