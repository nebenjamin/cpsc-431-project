using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class pie_graph : Graph
    {
        public override void drawGraph(Rectangle r)
        {
            double total = 0;
            double heading = 0; // Start pie pieces at 12 noon on the clock.
            double currentsweep = 0;
            int nqo1;
            double innerradius = 0;
            double outerradius = 20;
            int slices = 32;
            int loops = 1;
            

            clientRect = r;
            CheckGraphArea();


            
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Color3(Color.Black);
            DrawTitle();
            DrawLegend(r);

            GL.LoadIdentity();
            GL.Translate(50.0, 50.0, 0.0);

            GL.Enable(EnableCap.PolygonSmooth);
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Quads);

            nqo1 = Glu.NewQuadric();

                    
            //foreach (List<double> list in data)
            List<double> list = data[0];
            {
                 total = 0;
                 heading = 0; // Start pie pieces at 12 noon on the clock.
                 currentsweep = 0;
                int currentColor = 0;
                foreach (double num in list)
                {
                    total += num;
                }
                foreach (double num in list)
                {
                    GL.Color3(LegendColors[currentColor]);

                    currentsweep = (num / total) * 360; //(int)(Math.Ceiling((num / total) * 360));
                    GL.Color3(LegendColors[currentColor]);
                    Glu.PartialDisk(nqo1, innerradius, outerradius, slices, loops, heading, currentsweep);
                    currentColor++;
                    heading = heading + currentsweep;

                }

            }

            Glu.DeleteQuadric(nqo1);
            GL.End();
            
            
            
        }
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = 100;
            minYVal = 0;
            maxYVal = 100;
        }
        public override void setDefaults()
        {
            nVertLines = 0;
            nHorzLines = 0;

            TitleString = "Pie Graph";

            vGrid = true;
            hGrid = true;


        }

        public override void configTab(TabPage tb) { }
    }
}

