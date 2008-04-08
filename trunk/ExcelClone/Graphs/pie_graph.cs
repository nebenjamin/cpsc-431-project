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
        private CheckBox outLining_cb;
        private bool outline;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Pie_Graph(Form parent, Rectangle location, string[][] data)
        {
            //First, make a bar graph and add the data
            GraphControl gc = new GraphControl();
            List<List<double>> newData = new List<List<double>>();


            newData.Add(new List<double>());
            for (int j = 0; j < data.Length; j++)
            {
                double parsedDouble;
                if (!Double.TryParse(data[j][0], out parsedDouble))
                    throw new ArgumentException("Invalid data in Cells");
                newData[0].Add(parsedDouble);
            }

            Graph gr = new pie_graph(newData);

            gc.Location = new Point(0, 0);  //gc.loc is a point, not rect
            gc.Size = new Size(450, 400);
            gc.SetGraph(gr);

            graphConfig gConf = new graphConfig(gr, gc);
            gConf.ShowDialog();

            if (gConf.DialogResult == DialogResult.OK)
            {
                gr = gConf.gr;
                gc = new GraphControl();
                gc.Location = new Point(location.X, location.Y);  //gc.loc is a point, not rect
                gc.Size = location.Size;
                gc.SetGraph(gr);
                gr.InitFonts();
                gr.InitLabels();

                parent.Controls.Add(gc);
                gc.BringToFront();

                return gr;
            }

            return null;
        }

        public pie_graph(List<List<double>> newData):base(newData)
        {
            outline = false;
        }

        public override void drawGraph(Rectangle r)
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);

            clientRect = r;
            CheckGraphArea();

            GL.Color3(Color.Black);
            if (draw_title)
                DrawTitle();
            else
            {
                TitleString = "";
                DrawTitle();
            }
            if (LegendOn)
                DrawLegend(r);
            
            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(50.0, 50.0, 0.0);

            double total = 0;
            double heading = 0; // Start pie pieces at 12 noon on the clock.
            double currentsweep = 0;
            int nqo1 = Glu.NewQuadric();
            double innerradius = 0;
            double outerradius = 20;
            int slices = 32;
            int loops = 1;

            GL.Enable(EnableCap.PolygonSmooth);

            foreach (double num in data[0])
                total += num;

            int currentColor = 0;
            foreach (double num in data[0])
            {
                GL.Color3(LegendColors[currentColor]);

                currentsweep = (num / total) * 360;

                //Drawing Filled Partialdisk pie piece.
                GL.Color3(LegendColors[currentColor]);
                Glu.QuadricDrawStyle(nqo1, QuadricDrawStyle.Fill);
                Glu.PartialDisk(nqo1, innerradius, outerradius, slices, loops, heading, currentsweep);

                if (outLining_cb.Checked)
                {
                    //Drawing Black Outline to Pie Piece
                    GL.Color3(Color.Black);
                    Glu.QuadricDrawStyle(nqo1, QuadricDrawStyle.Silhouette);
                    Glu.PartialDisk(nqo1, innerradius, outerradius, slices, loops, heading, currentsweep);
                }
                currentColor++;
                heading = heading + currentsweep;
            }

            Glu.DeleteQuadric(nqo1);

            //return matrix like it was
            GL.PopMatrix();
        }
        public override Graph cloneGraph()
        {
            pie_graph gr = new pie_graph(data);
            gr.draw_title = draw_title;
            gr.TitleString = TitleString;
            gr.LegendOn = LegendOn;
            return gr;
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

            vGrid = false;
            hGrid = false ;
        }

        public override void configTab(TabPage tb) 
        {
            outLining_cb = new CheckBox();
            outLining_cb.AutoSize = true;
            outLining_cb.Location = new System.Drawing.Point(266, 102);
            outLining_cb.Name = "checkBoxOL";
            outLining_cb.Size = new System.Drawing.Size(100, 17);
            outLining_cb.TabIndex = 8;
            outLining_cb.Text = "Draw Black Outlines";
            outLining_cb.UseVisualStyleBackColor = true;
            tb.Controls.Add(outLining_cb);
            outLining_cb.CheckedChanged += new EventHandler(outLining_cb_CheckedChanged);
        }

        void outLining_cb_CheckedChanged(object sender, EventArgs e)
        {
            outline = outLining_cb.Checked;
        }
    }
}

