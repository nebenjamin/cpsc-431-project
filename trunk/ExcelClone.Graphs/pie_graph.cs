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
        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend(r);
        }
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = 100;
            minYVal = 0;
            maxYVal = 150;
        }
        public override void setDefaults()
        {
            nVertLines = 5;
            nHorzLines = 5;

            TitleString = "Pie Graph";

            vGrid = true;
            hGrid = true;
        }
    }
}

