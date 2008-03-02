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
        private float barW;

        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend();
            DrawLegend();

            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Quads);

            bar_width();
            int currentBar = 1;
            foreach (List<double> list in data)
            {
                int currentColor = 0;
                foreach (double num in list)
                {
                    GL.Color3(LegendColors[currentColor]);
                    GL.Vertex2(xOfGraph((float)currentBar*barW), yOfGraph(0));
                    GL.Vertex2(xOfGraph((float)currentBar*barW), yOfGraph((float)(num)));//-minYVal)));
                    GL.Vertex2(xOfGraph((float)(currentBar + 1) * barW), yOfGraph((float)(num)));//-minYVal)));
                    GL.Vertex2(xOfGraph((float)(currentBar + 1) * barW), yOfGraph(0));
                    currentColor++;
                    currentBar++;
                }
                currentBar++;
            }

            GL.End();


            //return matrix like it was
            GL.PopMatrix();
        }
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = data.Count;

            maxYVal = data[0][0];
            foreach(List<double> list in data)
            {
                foreach(double num in list)
                {
                    maxYVal = (num > maxYVal) ? num : maxYVal;
                }
            }
            minYVal = 0;
        }
        public override void setDefaults()
        {
            nVertLines = (int)(maxXVal+1);
            nHorzLines = (int)Math.Ceiling(maxYVal / 10);

            TitleString = "Column Graph";

            vGrid = true;
            hGrid = true;
        }
        public void bar_width()
        {
            int totalBars = data.Count + 1;
            totalBars += data.Count * data[0].Count;
            barW = (float)(maxXVal - minXVal) / totalBars;
        }
    }
}
