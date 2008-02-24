using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class pie_graph : Graph
    {
        public override void drawGraph(Rectangle clientRect)
        {
            PointF UpR, LowL;
            //First, make sure the labels will be on screen
            CheckGraphArea(clientRect, out LowL, out UpR);

            Draw(UpR, LowL, clientRect);
            DrawTitle(clientRect);
        }
    }
}

