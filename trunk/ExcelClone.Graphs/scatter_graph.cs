using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class scatter_graph : Graph
    {

        private Color pointColor = Color.Blue;
        private Color lineColor = Color.Black;
        private int pointSize = 5;

        //populate these from data[] ArrayList(you should be able to do this today, no problem)
        private double[] dataX = { 10, 20, 30, 40, 50, 60, 70 };
        private double[] dataY = { 10, 35, 50, 40, 75, 95, 110 };
        
        public override void drawGraph(Rectangle r)
        {
            clientRect = r;
            //preprocessing section
            //calculate least squares params !!switch to Matricks.Solve(c) when available for ill-conditioning checks and workarounds

            double a0 = 0.0, b0 = 0.0, a1 = 0.0, b1 = 0.0, c0 = 0.0, c1 = 0.0;

            for (int i = 0; i < dataX.Length; i++)
            {
                a0 += (dataX[i] * dataX[i]);
                b0 += dataX[i];
                c0 += (dataX[i] * dataY[i]);
                c1 += dataY[i];
            }

            a1 = b0;
            b1 = (double) dataX.Length;
                                       
            //reduce row 2                        
            b1 -= ((b0 * a1 )/ a0);
            c1 -= ((c0 * a1 )/ a0);            
            //a1 = 0.0 at this point 
            //solve for b
            double b = c1 / b1;
            //solve for a
            double a = (c0 - (b0 * b))/a0;


            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend(r);


            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();

                //add a way to change these and the 
                GL.Color3(pointColor);
                GL.PointSize(pointSize);
            
                GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Points);
                                            
                    for (int i = 0; i < dataX.Length; i++)                   
                        GL.Vertex2(xOfGraph((float) dataX[i]), yOfGraph((float) dataY[i]));
                            
                GL.End();

               
                //draw regression fit line
                   
                GL.Color3(lineColor);
                GL.Begin(OpenTK.OpenGL.Enums.BeginMode.LineStrip);
                    //draw end points
                    GL.Vertex2(xOfGraph((float) dataX[0]), yOfGraph((float) ( b + dataX[0] * a)));
                    GL.Vertex2(xOfGraph((float) dataX[dataX.Length-1]), yOfGraph((float) ( b + dataX[dataX.Length-1] * a)));
                GL.End();            
            


            //return matrix like it was
            GL.PopMatrix();
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

           TitleString = "Scatter Graph";

           vGrid = true;
           hGrid = true;
       }
    }
}
