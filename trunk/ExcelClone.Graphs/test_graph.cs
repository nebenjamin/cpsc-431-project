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
        public override void drawGraph(Rectangle clientRect)
        {
            PointF UpR, LowL;
            //First, make sure the labels will be on screen
            CheckGraphArea(clientRect, out LowL, out UpR);

            Draw(UpR, LowL, clientRect);
            DrawTitle(clientRect);


            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();

            
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Triangles);
                GL.Color3(0.1f, 0.4f, 0.1f);
                GL.Vertex2(xOfGraph(0, 4, 1, UpR, LowL), yOfGraph(0, 4, 1, UpR, LowL));
                GL.Vertex2(xOfGraph(0, 4, 1, UpR, LowL), yOfGraph(0, 4, 3, UpR, LowL));
                GL.Vertex2(xOfGraph(0, 4, 3, UpR, LowL), yOfGraph(0, 4, 3, UpR, LowL));
            GL.End();


            //return matrix like it was
            GL.PopMatrix();
        }
    }
}
