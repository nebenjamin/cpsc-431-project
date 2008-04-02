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
        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Pie_Graph(Form parent, Rectangle location, string[][] data)
        {
            //First, make a bar graph and add the data
            GraphControl gc = new GraphControl();
            List<List<double>> newData = new List<List<double>>();

            foreach (string[] strarray in data)  //Fill in and parse incoming cell data
            {
                newData.Add(new List<double>());
                foreach (string s in strarray)
                {
                    double parsedDouble;
                    if (!Double.TryParse(s, out parsedDouble))
                        throw new ArgumentException("Invalid data in Cells");
                    newData[0].Add(parsedDouble);
                }
            }

            Graph gr = new pie_graph(newData);

            gc.Location = new Point(0, 0);  //gc.loc is a point, not rect
            gc.Size = new Size(450, 400);
            gc.SetGraph(gr);

            graphConfig gConf = new graphConfig(gr, gc);
            gConf.ShowDialog();

            if (gConf.DialogResult == DialogResult.OK)
            {
                gc = new GraphControl();
                gc.Location = new Point(location.X, location.Y);  //gc.loc is a point, not rect
                gc.Size = location.Size;
                gc.SetGraph(gr);
                gr.InitFonts();
                gr.InitLabels();

                parent.Controls.Add(gc);
            }

            parent.Controls.Add(gc);

            return gr;
        }

        public pie_graph(List<List<double>> newData):base(newData){}

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
            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);
            //Axis labels
            txp.Prepare(XLabelString, AxesFont, out XAxisLabel);
            txp.Prepare(YLabelString, AxesFont, out YAxisLabel);

            
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Color3(Color.Black);
            if (draw_title)
                DrawTitle();
            if (LegendOn)
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

