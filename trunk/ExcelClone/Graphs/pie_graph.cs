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
    public class pie_graph : Graph
    {
        private bool outline;

        // objects that appear in the specific configuration tab
        private CheckBox outLining_cb;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Pie_Graph(Rectangle location, string[][] data)
        {
            //First, create the control graph will be in
            GraphControl gc = new GraphControl();

            //parse the 2d string array
            List<List<double>> newData = new List<List<double>>();
            newData.Add(new List<double>());
            for (int j = 0; j < data.Length; j++)
            {
                double parsedDouble;
                if (!Double.TryParse(data[j][0], out parsedDouble))
                    throw new ArgumentException("Invalid data in Cells");
                newData[0].Add(parsedDouble);
            }

            // now create graph with data
            Graph gr = new pie_graph(newData);

            // location, size of graph in preview tab
            gc.Location = new Point(0, 0);
            gc.Size = new Size(450, 400);
            gc.SetGraph(gr);

            // open up a graph configuration window
            graphConfig gConf = new graphConfig(gr, gc);
            gConf.ShowDialog();

            // if press ok then put graph control in main form
            if (gConf.DialogResult == DialogResult.OK)
            {
                // put in the graph with the changes
                gr = gConf.gr;
                // create new control to reset any parameters
                gc = new GraphControl();
                // location and size of control inside main form
                gc.Location = new Point(location.X, location.Y);
                gc.Size = location.Size;
                gc.SetGraph(gr);
                gr.InitFonts();
                gr.InitLabels();

                Controller.Instance.MainForm.Controls.Add(gc);
                gc.BringToFront();

                return gr;
            }

            return null;
        }

        public pie_graph(List<List<double>> newData):base(newData)
        {
            outline = false;
        }

        //Parameterless constructor for XMLserialize
        public pie_graph() : base() { }

        // the draw function that will be called any time the control needs to
        // be drawn
        public override void drawGraph(Rectangle r)
        {
            // for some odd reason must reput the clear color again in this 
            // graph
            GL.ClearColor(Color.White);
            // clear buffers to start out fresh
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);

            //check the parameters of the graph area (min and max values)
            clientRect = r;
            CheckGraphArea();

            // no need for axis
            GL.Color3(Color.Black);
            if (draw_title)
                DrawTitle();

            if (LegendOn)
                DrawLegend(r);
            
            // Do this so that you don't mess around with base class matrix
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(50.0, 50.0, 0.0);

            double total = 0;
            // Start pie pieces at 12 noon on the clock.
            double heading = 0;
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

                if (outline)
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

        // this is to make a new graph object equal to this one.
        public override Graph cloneGraph()
        {
            pie_graph gr = new pie_graph(data);
            gr.draw_title = draw_title;
            gr.TitleString = TitleString;
            gr.LegendOn = LegendOn;
            return gr;
        }

        // sets the max and min values x and y can be
        // this is used by the xOfGraph and yOfGraph functions in 
        // main Graph class
        public override void setMinMax()
        {
            minXVal = 0;
            maxXVal = 100;
            minYVal = 0;
            maxYVal = 100;
        }

        // sets the defaults on some attributes
        public override void setDefaults()
        {
            nVertLines = 0;
            nHorzLines = 0;

            TitleString = "Pie Graph";

            vGrid = false;
            hGrid = false ;
        }

        // populates the specific tab in the graph config window
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

