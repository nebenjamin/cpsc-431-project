using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;
using System.Windows.Forms;

namespace ExcelClone.Graphs
{
    public class bar_graph : Graph
    {
        private float barW;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Bar_Graph(Form parent, Rectangle location, string[][] data)
        {
            //First, make a bar graph and add the data
            GraphControl gc = new GraphControl();
            List<List<double>> newData = new List<List<double>>();

            foreach (string[] strarray in data)  //Fill in and parse incoming cell data
            {
                newData.Add(new List<double>() );
                foreach (string s in strarray)
                {
                    double parsedDouble;
                    if (!Double.TryParse(s, out parsedDouble))
                        throw new ArgumentException("Invalid data in Cells");
                    newData[0].Add(parsedDouble);
                }
            }

            Graph gr = new bar_graph(newData);

            gc.Location = new Point(0, 0);  //gc.loc is a point, not rect
            gc.Size = new Size(450,400);
            gc.SetGraph(gr);

            graphConfig gConf = new graphConfig(gr,gc);
            gConf.ShowDialog();

            //gC.Dispose();
            //gc = new GraphControl();
            gc.Location = new Point(location.X, location.Y);  //gc.loc is a point, not rect
            gc.Size = location.Size;
            gc.SetGraph(gr);

            parent.Controls.Add(gc);            

            return gr;
        }

        public bar_graph(List<List<double>> newData):base(newData){}

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
                    GL.Vertex2(xOfGraph(0), yOfGraph((float)currentBar*barW));
                    GL.Vertex2(xOfGraph((float)(num)), yOfGraph((float)currentBar*barW));
                    GL.Vertex2(xOfGraph((float)(num)), yOfGraph((float)(currentBar + 1) * barW));
                    GL.Vertex2(xOfGraph(0), yOfGraph((float)(currentBar + 1) * barW));
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
            minXVal = 0;
        }
        public override void setDefaults()
        {
            nHorzLines = (int)(maxYVal+1);
            nVertLines = (int)Math.Ceiling(maxXVal / 10);

            TitleString = "Bar Graph";

            vGrid = true;
            hGrid = true;
        }
        public void bar_width()
        {
            int totalBars = data.Count + 1;
            totalBars += data.Count * data[0].Count;
            barW = (float)(maxYVal - minYVal) / totalBars;
        }

        public override void configTab(TabPage tb) { }
    }
}
