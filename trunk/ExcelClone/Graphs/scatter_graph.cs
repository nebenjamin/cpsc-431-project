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
    public class scatter_graph : Graph
    {
        private Color lineColor = Color.Black;
        private int pointSize = 5;

        // objects that appear in the specific configuration tab
        private TextBox yAxis_tb;
        private CheckBox yAxis_cb;
        private Label yAxis_lb;
        private CheckBox xAxis_cb;
        private TextBox xAxis_tb;
        private Label xAxis_lb;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Scatter_Graph(Rectangle location, string[][] data)
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
            Graph gr = new scatter_graph(newData);

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

        public scatter_graph(List<List<double>> newData):base(newData){}

        //Parameterless constructor for XMLserialize
        public scatter_graph() : base() { }

        // the draw function that will be called any time the control needs to
        // be drawn
        public override void drawGraph(Rectangle r)
        {
            //preprocessing section
            //calculate least squares params 
            //!!switch to Matricks.Solve(c) when
            //available for ill-conditioning checks and workarounds
            double a0 = 0.0, b0 = 0.0, a1 = 0.0, b1 = 0.0, c0 = 0.0, c1 = 0.0;

            for (int i = 0; i < data.Count; i++)
            {
                a0 += (data[i][0] * data[i][0]);
                b0 += data[i][0];
                c0 += (data[i][0] * data[i][0]);
                c1 += data[i][0];
            }

            a1 = b0;
            b1 = (double) data.Count;
                                       
            //reduce row 2                        
            b1 -= ((b0 * a1 )/ a0);
            c1 -= ((c0 * a1 )/ a0);            
            //a1 = 0.0 at this point 
            //solve for b
            double b = c1 / b1;
            //solve for a
            double a = (c0 - (b0 * b))/a0;

            //start
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
            // point size of the points
            GL.PointSize(pointSize);

            // data[row][column]
            // x | y1 | y2 | y3 | ...
            // -----------------------
            // 6 | 2  | 3  | 4  | ...
            // 5 | 6  | 7  | 8  | ...
            // ....

            //for each y column (starting at 1)
            for (int i = 1; i < data[0].Count; i++)
            {
                GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Points);

                GL.Color3(LegendColors[i - 1]);
                // for each row
                for (int j = 0; j < data.Count; j++)
                {
                    GL.Vertex2(xOfGraph((float)data[j][0]), yOfGraph((float)data[j][i]));
                }

                GL.End();
            }
               
            //draw regression fit line
            GL.Color3(lineColor);
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.LineStrip);
                //draw end points
                GL.Vertex2(xOfGraph((float) data[0][0]), yOfGraph((float) ( b + data[0][0] * a)));
                GL.Vertex2(xOfGraph((float) data[data.Count-1][0]), yOfGraph((float) ( b + data[data.Count-1][0] * a)));
            GL.End();            

            //return matrix like it was
            GL.PopMatrix();
        }

        // this is to make a new graph object equal to this one.
        public override Graph cloneGraph()
        {
            scatter_graph gr = new scatter_graph(data);
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
            // data[row][column]
            // x | y1 | y2 | y3 | ...
            // -----------------------
            // 6 | 2  | 3  | 4  | ...
            // 5 | 6  | 7  | 8  | ...
            // ....

            // sort data
            data.Sort(
                delegate(List<double> l1, List<double> l2)
                {
                    return l1[0].CompareTo(l2[0]);
                });

            // min X after sort is first x and max is the last x
            minXVal = data[0][0];
            maxXVal = data[data.Count - 1][0];

            // have to go through all y values to find smallest
            minYVal = data[0][1];
            for (int i = 0; i < data.Count; i++)
                for (int j = 1; j < data[0].Count; j++)
                    minYVal = (data[i][j] < minYVal) ? data[i][j] : minYVal;

            // have to go through all y values to find smallest
            maxYVal = data[0][1];
            for (int i = 0; i < data.Count; i++)
                for (int j = 1; j < data[0].Count; j++)
                    maxYVal = (data[i][j] > maxYVal) ? data[i][j] : maxYVal;
       }

       // sets the defaults on some attributes
       public override void setDefaults()
       {
           nVertLines = 5;
           nHorzLines = 5;

           TitleString = "Scatter Graph";

           vGrid = true;
           hGrid = true;
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

           xAxis_cb.Checked = draw_xLabel;
           xAxis_tb.Text = XLabelString;
           yAxis_cb.Checked = draw_yLabel;
           yAxis_tb.Text = YLabelString;
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
