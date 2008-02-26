using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class bar_graph : Graph
    {
        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend();
            DrawLegend();

            //foreach (List<double> list in data)
            //{
            //    data.Sort();
            //}

            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();


            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Quads);

            GL.Color3(Color.CadetBlue);
            GL.Vertex2(xOfGraph(0.5f), yOfGraph(0));
            GL.Vertex2(xOfGraph(0.5f), yOfGraph(3));
            GL.Vertex2(xOfGraph(1f), yOfGraph(3));
            GL.Vertex2(xOfGraph(1f), yOfGraph(0));


            GL.Color3(Color.BurlyWood);
            GL.Vertex2(xOfGraph(1f), yOfGraph(0));
            GL.Vertex2(xOfGraph(1f), yOfGraph(2.5f));
            GL.Vertex2(xOfGraph(1.5f), yOfGraph(2.5f));
            GL.Vertex2(xOfGraph(1.5f), yOfGraph(0));

            GL.End();


            //return matrix like it was
            GL.PopMatrix();
        }
        public override void setMinMax()
        {
            minYVal = 0;
            maxYVal = data.Count;

            maxXVal = data[0][0];
            foreach(List<double> list in data)
            {
                foreach(double num in list)
                {
                    maxXVal = (num > maxXVal) ? num : maxXVal;
                }
            }
            minXVal = data[0][0];
            foreach (List<double> list in data)
            {
                foreach (double num in list)
                {
                    minXVal = (num < minXVal) ? num : minXVal;
                }
            }
        }
    }
}
