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
    class line_graph : Graph
    {
        private double[] dataX = { 10, 20, 30, 40, 50, 60, 70 };
        private double[] dataY = { 10, 35, 50, 40, 75, 95, 110 };

        private float xMin = 0, xMax = 100, yMin = 0, yMax = 150;

        private Color pointColor = Color.Blue;
        private Color lineColor = Color.Black;
        private int pointSize = 5;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Line_Graph(Form parent, Rectangle location, string[][] data)
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

            Graph gr = new line_graph(newData);

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

        public line_graph(List<List<double>> newData):base(newData){}

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
            if (draw_title)
                DrawTitle();
            if (LegendOn)
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

        public override void configTab(TabPage tb) { }
    }
}
