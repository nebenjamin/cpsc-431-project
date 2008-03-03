using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class line_graph : Graph
    {
        private double[] dataX = { 10, 20, 30, 40, 50, 60, 70 };
        private double[] dataY = { 10, 35, 50, 40, 75, 95, 110 };

        private float xMin = 0, xMax = 100, yMin = 0, yMax = 150;

        private Color pointColor = Color.Blue;
        private Color lineColor = Color.Black;
        private int pointSize = 5;
        
        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend(r);

            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Color3(pointColor);
            GL.PointSize(pointSize);

            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.LineStrip);

                for (int i = 0; i < dataX.Length; i++)
                    GL.Vertex2(xOfGraph((float)dataX[i]), yOfGraph((float)dataY[i]));

            GL.End();

            //return matrix like it was
            GL.PopMatrix();
        }
        public override void setDefaults()
        {
            nVertLines = 5;
            nHorzLines = 5;

            TitleString = "Line Graph";

            vGrid = true;
            hGrid = true;
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
