using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using OpenTK.OpenGL;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    class column_graph : Graph
    {
        private float barW;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Column_Graph(Form parent, Rectangle location, string[][] data)
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

            Graph gr = new column_graph(newData);

            gc.Location = new Point(0, 0);  //gc.loc is a point, not rect
            gc.Size = new Size(450, 400);
            gc.SetGraph(gr);

            graphConfig gC = new graphConfig(gr, gc);
            gC.ShowDialog();

            //gC.Dispose();
            //gc = new GraphControl();
            gc.Location = new Point(location.X, location.Y);  //gc.loc is a point, not rect
            gc.Size = location.Size;
            gc.SetGraph(gr);

            parent.Controls.Add(gc);

            return gr;
        }

        public column_graph(List<List<double>> newData):base(newData){}

        public override void drawGraph(Rectangle r)
        {
            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);
            //Axis labels
            txp.Prepare(XLabelString, AxesFont, out XAxisLabel);
            txp.Prepare(YLabelString, AxesFont, out YAxisLabel);
            clientRect = r;
            CheckGraphArea();
            DrawAxis();
            DrawTitle();
            DrawLegend(r);

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

        public override void configTab(TabPage tb) { }
    }
}
