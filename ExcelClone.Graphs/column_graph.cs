using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class column_graph : Graph
    {
        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend();
            DrawLegend();
        }
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = 100;
            minYVal = 0;
            maxYVal = 150;
        }
    }
}
