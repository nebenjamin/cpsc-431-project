using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using OpenTK.Fonts;

namespace ExcelClone.Graphs
{
    public class column_graph : Graph
    {
        private float barW;

        // objects that appear in the specific configuration tab
        private TextBox yAxis_tb;
        private CheckBox yAxis_cb;
        private Label yAxis_lb;
        private CheckBox xAxis_cb;
        private TextBox xAxis_tb;
        private Label xAxis_lb;
        private NumericUpDown nud_nHor;
        private Label nHorLabel;
        private Label maxYVlabel;
        private NumericUpDown nud_maxYV;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Column_Graph(Rectangle location, string[][] data)
        {
            //First, create the control graph will be in
            GraphControl gc = new GraphControl();
            List<List<double>> newData = new List<List<double>>();

            //parse the 2d string array
            for (int i = 0; i < data.Length; i++)
            {
                newData.Add(new List<double>());
                for (int j = 0; j < data[i].Length; j++)
                {
                    double parsedDouble;
                    if (!Double.TryParse(data[i][j], out parsedDouble))
                        throw new ArgumentException("Invalid data in Cells");
                    newData[i].Add(parsedDouble);
                }
            }

            // now create graph with data
            Graph gr = new column_graph(newData);

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

        public column_graph(List<List<double>> newData):base(newData){}

        //Parameterless constructor for XMLserialize
        public column_graph() : base() { }

        // the draw function that will be called any time the control needs to
        // be drawn
        public override void drawGraph(Rectangle r)
        {
            // clear buffers to start out fresh
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);
            //Axis labels
            txp.Prepare(XLabelString, AxesFont, out XAxisLabel);
            txp.Prepare(YLabelString, AxesFont, out YAxisLabel);

            //check the parameters of the graph area (min and max values)
            clientRect = r;
            CheckGraphArea();

            //draws title, axis and legend if neccesary
            GL.Color3(Color.Black);
            DrawAxis();
            if (draw_title)
                DrawTitle();
            if (LegendOn)
                DrawLegend(r);

            // Do this so that you don't mess around with base class matrix
            GL.PushMatrix();
            GL.LoadIdentity();

            //start drawing rectangles
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
                    GL.Vertex2(xOfGraph((float)currentBar*barW), yOfGraph((float)(num)));
                    GL.Vertex2(xOfGraph((float)(currentBar + 1) * barW), yOfGraph((float)(num)));
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

        // this is to make a new graph object equal to this one.
        public override Graph cloneGraph()
        {
            column_graph gr = new column_graph(data);
            gr.draw_title = draw_title;
            gr.TitleString = TitleString;
            gr.LegendOn = LegendOn;
            gr.draw_xLabel = draw_xLabel;
            gr.XLabelString = XLabelString;
            gr.draw_yLabel = draw_yLabel;
            gr.YLabelString = YLabelString;
            return gr;
        }

        // sets the max and min values x and y can be
        // this is used by the xOfGraph and yOfGraph functions in 
        // main Graph class
        public override void setMinMax()
        {
            // X is easy since its just 0 to the number of bars
            minXVal = 0;
            maxXVal = data.Count;

            // sets the first value as biggest
            maxYVal = data[0][0];
            //goes through all values updating biggest
            foreach(List<double> list in data)
            {
                foreach(double num in list)
                {
                    maxYVal = (num > maxYVal) ? num : maxYVal;
                }
            }
            // the smallest will always be zero
            minYVal = 0;
        }

        // sets the defaults on some attributes
        public override void setDefaults()
        {
            nVertLines = (int)(maxXVal+1);
            nHorzLines = 8;

            TitleString = "Column Graph";

            vGrid = true;
            hGrid = true;
        }

        // calculates the bar_with, puts a empty bar between
        // each set and the start and end of graph
        public void bar_width()
        {
            int totalBars = data.Count + 1;
            totalBars += data.Count * data[0].Count;
            barW = (float)(maxXVal - minXVal) / totalBars;
        }

        // populates the specific tab in the graph config window
        public override void configTab(TabPage tp)
        {
            yAxis_cb = new CheckBox();
            yAxis_cb.AutoSize = true;
            yAxis_cb.Location = new System.Drawing.Point(266, 102);
            yAxis_cb.Name = "checkBox3";
            yAxis_cb.Size = new System.Drawing.Size(100, 17);
            yAxis_cb.TabIndex = 8;
            yAxis_cb.Text = "Draw Y-axis Label";
            yAxis_cb.UseVisualStyleBackColor = true;
            tp.Controls.Add(yAxis_cb);
            yAxis_cb.CheckedChanged += new EventHandler(yAxis_cb_CheckedChanged);

            yAxis_tb = new TextBox();
            yAxis_tb.Location = new System.Drawing.Point(9, 100);
            yAxis_tb.Name = "textBox3";
            yAxis_tb.Size = new System.Drawing.Size(250, 20);
            yAxis_tb.TabIndex = 7;
            yAxis_tb.Text = "Create Y-axis Label";
            tp.Controls.Add(yAxis_tb);
            yAxis_tb.Click += new EventHandler(yAxis_tb_Click);
            yAxis_tb.TextChanged += new EventHandler(yAxis_tb_TextChanged);

            yAxis_lb = new Label();
            yAxis_lb.AutoSize = true;
            yAxis_lb.Location = new System.Drawing.Point(6, 83);
            yAxis_lb.Name = "label3";
            yAxis_lb.Size = new System.Drawing.Size(64, 13);
            yAxis_lb.TabIndex = 6;
            yAxis_lb.Text = "Y-axis Label";
            tp.Controls.Add(yAxis_lb);

            xAxis_cb = new CheckBox();
            xAxis_cb.AutoSize = true;
            xAxis_cb.Location = new System.Drawing.Point(266, 62);
            xAxis_cb.Name = "checkBox2";
            xAxis_cb.Size = new System.Drawing.Size(100, 17);
            xAxis_cb.TabIndex = 5;
            xAxis_cb.Text = "Draw X-axis Label";
            xAxis_cb.UseVisualStyleBackColor = true;
            tp.Controls.Add(xAxis_cb);
            xAxis_cb.CheckedChanged += new EventHandler(xAxis_cb_CheckedChanged);

            xAxis_tb = new TextBox();
            xAxis_tb.Location = new System.Drawing.Point(9, 60);
            xAxis_tb.Name = "textBox2";
            xAxis_tb.Size = new System.Drawing.Size(250, 20);
            xAxis_tb.TabIndex = 4;
            xAxis_tb.Text = "Create X-axis Label";
            tp.Controls.Add(xAxis_tb);
            xAxis_tb.Click += new EventHandler(xAxis_tb_Click);
            xAxis_tb.TextChanged += new EventHandler(xAxis_tb_TextChanged);

            xAxis_lb = new Label();
            xAxis_lb.AutoSize = true;
            xAxis_lb.Location = new System.Drawing.Point(6, 43);
            xAxis_lb.Name = "label2";
            xAxis_lb.Size = new System.Drawing.Size(64, 13);
            xAxis_lb.TabIndex = 3;
            xAxis_lb.Text = "X-axis Label";
            tp.Controls.Add(xAxis_lb);

            nud_nHor = new NumericUpDown();
            ((ISupportInitialize)(nud_nHor)).BeginInit();
            tp.Controls.Add(nud_nHor);
            nud_nHor.Location = new System.Drawing.Point(6, 369);
            nud_nHor.Name = "nud_nHor";
            nud_nHor.Size = new System.Drawing.Size(120, 20);
            nud_nHor.TabIndex = 6;
            nud_nHor.ValueChanged += new EventHandler(nud_nHor_ValueChanged);
            ((ISupportInitialize)(nud_nHor)).EndInit();

            nHorLabel = new Label();
            tp.Controls.Add(nHorLabel);
            nHorLabel.AutoSize = true;
            nHorLabel.Location = new System.Drawing.Point(133, 376);
            nHorLabel.Name = "nHorLabel";
            nHorLabel.Size = new System.Drawing.Size(134, 13);
            nHorLabel.TabIndex = 8;
            nHorLabel.Text = "Number of Horizontal Lines";

            maxYVlabel = new Label();
            tp.Controls.Add(maxYVlabel);
            maxYVlabel.AutoSize = true;
            maxYVlabel.Location = new System.Drawing.Point(133, 324);
            maxYVlabel.Name = "maxYVlabel";
            maxYVlabel.Size = new System.Drawing.Size(67, 13);
            maxYVlabel.TabIndex = 11;
            maxYVlabel.Text = "Max Y Value";

            nud_maxYV = new NumericUpDown();
            ((ISupportInitialize)(nud_maxYV)).BeginInit();
            tp.Controls.Add(nud_maxYV);
            nud_maxYV.Location = new System.Drawing.Point(6, 317);
            nud_maxYV.Name = "nud_maxYV";
            nud_maxYV.Size = new System.Drawing.Size(120, 20);
            nud_maxYV.TabIndex = 13;
            nud_maxYV.ValueChanged += new EventHandler(nud_maxYV_ValueChanged);
            ((ISupportInitialize)(nud_maxYV)).EndInit();

            nud_nHor.Value = nHorzLines;
            nud_maxYV.Value = new Decimal(maxYVal);
            nud_maxYV.Minimum = new Decimal(maxYVal);
            xAxis_cb.Checked = draw_xLabel;
            xAxis_tb.Text = XLabelString;
            yAxis_cb.Checked = draw_yLabel;
            yAxis_tb.Text = YLabelString;
        }

        void nud_maxYV_ValueChanged(object sender, EventArgs e)
        {
            maxYVal = decimal.ToDouble(nud_maxYV.Value);
            InitLabels();
        }

        void nud_nHor_ValueChanged(object sender, EventArgs e)
        {
            nHorzLines = (int)nud_nHor.Value;
            InitLabels();
        }

        void yAxis_tb_Click(object sender, EventArgs e)
        {
            yAxis_tb.Text = "";
            yAxis_cb.Checked = true;
        }

        void xAxis_tb_Click(object sender, EventArgs e)
        {
            xAxis_tb.Text = "";
            xAxis_cb.Checked = true;
        }

        void xAxis_cb_CheckedChanged(object sender, EventArgs e)
        {
            draw_xLabel = xAxis_cb.Checked;
        }

        void xAxis_tb_TextChanged(object sender, EventArgs e)
        {
            XLabelString = xAxis_tb.Text;
        }

        void yAxis_cb_CheckedChanged(object sender, EventArgs e)
        {

            draw_yLabel = yAxis_cb.Checked;
        }

        void yAxis_tb_TextChanged(object sender, EventArgs e)
        {
            YLabelString = yAxis_tb.Text;
        }
    }
}
