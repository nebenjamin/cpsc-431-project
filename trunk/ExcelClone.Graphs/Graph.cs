using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK.OpenGL;

namespace ExcelClone.Graphs
{
    public class Graph  //Graph base class- draw a grid, labels, legend, and Title
    {
        private object data; //data in the graph

        public Point grLowLeft = new Point(15, 15);  //points of graph area
        public Point grUpRight = new Point(85, 85);

        public int nVertLines = 5;  //Vertical/Horizontal grid line count
        public int nHorzLines = 5;

        public bool vGrid;  //enable / disable grid lines
        public bool hGrid;
        
        public Graph()
        {
            data = new int[2,5];
            Random ran = new Random();
            //fill in sample data for testing
            for (int i=0; i < 5; i++)
            {
                ((int[,])data)[0, i] = i;
                ((int[,])data)[1,i] = ran.Next();
            }

            vGrid = true;
            hGrid = true;
        }

        public void Draw()  //Draw method, draw the grid since this is the base class
        {
            GL.ClearColor(Color.White);
            GL.Clear(OpenTK.OpenGL.Enums.ClearBufferMask.ColorBufferBit | OpenTK.OpenGL.Enums.ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(OpenTK.OpenGL.Enums.MatrixMode.Modelview);
            GL.LoadIdentity();

            //Draw a grid
            GL.Color3(Color.Gray);
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Lines);
            //Vertical
            if (vGrid)
            {
                for (int i = 0; i <= nVertLines; i++)  //draw evenly spaced grid lines
                {
                    //Cast these to float for a nice smooth divide
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)nVertLines, grLowLeft.Y);
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)nVertLines, grUpRight.Y);
                }
            }
            //Horizontal
            if (hGrid)
            {
                for (int i = 0; i <= nHorzLines; i++)  //draw evenly spaced horizontal grid lines
                {
                    //Cast these to float for a nice smooth divide
                    GL.Vertex2(grLowLeft.X, grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)nHorzLines);
                    GL.Vertex2(grUpRight.X, grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)nHorzLines);
                }
            }
            GL.End();
        }
    }
}
