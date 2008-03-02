using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class test_graph : Graph
    {
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

            
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Triangles);
                GL.Color3(0.1f, 0.4f, 0.1f);
                GL.Vertex2(xOfGraph(1), yOfGraph(1));
                GL.Vertex2(xOfGraph(1), yOfGraph(3));
                GL.Vertex2(xOfGraph(3), yOfGraph(3));
            GL.End();


            //return matrix like it was
            GL.PopMatrix();
        }
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = 4;
            minYVal = 0;
            maxYVal = 4;
        }
        public override void setDefaults()
        {
            nVertLines = 5;
            nHorzLines = 5;

            TitleString = "Test Graph";

            vGrid = true;
            hGrid = true;
        }
    }
}
