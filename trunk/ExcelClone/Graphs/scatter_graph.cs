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
        private NumericUpDown nud_nHor;
        private NumericUpDown nud_nVer;
        private Label nHorLabel;
        private Label nVerLable;
        private Label minYVlabel;
        private Label maxYVlabel;
        private NumericUpDown nud_minYV;
        private NumericUpDown nud_maxYV;
        private Label minXVlabel;
        private Label maxXVlabel;
        private NumericUpDown nud_minXV;
        private NumericUpDown nud_maxXV;

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

                Controller.Instance.MainForm.WorksheetsTabControl.SelectedTab.Controls.Add(gc);
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
            gr.nHorzLines = nHorzLines;
            gr.nVertLines = nVertLines;
            gr.maxXVal = maxXVal;
            gr.minXVal = minXVal;
            gr.maxYVal = maxYVal;
            gr.minYVal = minYVal;
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
           yAxis_cb.TabIndex = 0;
           yAxis_cb.Text = "Draw Y-axis Label";
           yAxis_cb.UseVisualStyleBackColor = true;
           tp.Controls.Add(yAxis_cb);
           yAxis_cb.CheckedChanged += new EventHandler(yAxis_cb_CheckedChanged);

           yAxis_tb = new TextBox();
           yAxis_tb.Location = new System.Drawing.Point(9, 100);
           yAxis_tb.Name = "textBox3";
           yAxis_tb.Size = new System.Drawing.Size(250, 20);
           yAxis_tb.TabIndex = 1;
           yAxis_tb.Text = "Create Y-axis Label";
           tp.Controls.Add(yAxis_tb);
           yAxis_tb.Click += new EventHandler(yAxis_tb_Click);
           yAxis_tb.TextChanged += new EventHandler(yAxis_tb_TextChanged);

           yAxis_lb = new Label();
           yAxis_lb.AutoSize = true;
           yAxis_lb.Location = new System.Drawing.Point(6, 83);
           yAxis_lb.Name = "label3";
           yAxis_lb.Size = new System.Drawing.Size(64, 13);
           yAxis_lb.TabIndex = 2;
           yAxis_lb.Text = "Y-axis Label";
           tp.Controls.Add(yAxis_lb);

           xAxis_cb = new CheckBox();
           xAxis_cb.AutoSize = true;
           xAxis_cb.Location = new System.Drawing.Point(266, 62);
           xAxis_cb.Name = "checkBox2";
           xAxis_cb.Size = new System.Drawing.Size(100, 17);
           xAxis_cb.TabIndex = 3;
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
           xAxis_lb.TabIndex = 5;
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


           nud_nVer = new NumericUpDown();
           ((ISupportInitialize)(nud_nVer)).BeginInit();
           tp.Controls.Add(nud_nVer);
           nud_nVer.Location = new System.Drawing.Point(6, 343);
           nud_nVer.Name = "nud_nVer";
           nud_nVer.Size = new System.Drawing.Size(120, 20);
           nud_nVer.TabIndex = 7;
           nud_nVer.ValueChanged += new EventHandler(nud_nVer_ValueChanged);
           ((ISupportInitialize)(nud_nVer)).EndInit();

           nHorLabel = new Label();
           tp.Controls.Add(nHorLabel);
           nHorLabel.AutoSize = true;
           nHorLabel.Location = new System.Drawing.Point(133, 376);
           nHorLabel.Name = "nHorLabel";
           nHorLabel.Size = new System.Drawing.Size(134, 13);
           nHorLabel.TabIndex = 8;
           nHorLabel.Text = "Number of Horizontal Lines";

           nVerLable = new Label();
           tp.Controls.Add(nVerLable);
           nVerLable.AutoSize = true;
           nVerLable.Location = new System.Drawing.Point(132, 350);
           nVerLable.Name = "nVerLable";
           nVerLable.Size = new System.Drawing.Size(122, 13);
           nVerLable.TabIndex = 9;
           nVerLable.Text = "Number of Vertical Lines";

           minYVlabel = new Label();
           tp.Controls.Add(minYVlabel);
           minYVlabel.AutoSize = true;
           minYVlabel.Location = new System.Drawing.Point(132, 298);
           minYVlabel.Name = "minYVlabel";
           minYVlabel.Size = new System.Drawing.Size(64, 13);
           minYVlabel.TabIndex = 10;
           minYVlabel.Text = "Min Y Value";

           maxYVlabel = new Label();
           tp.Controls.Add(maxYVlabel);
           maxYVlabel.AutoSize = true;
           maxYVlabel.Location = new System.Drawing.Point(133, 324);
           maxYVlabel.Name = "maxYVlabel";
           maxYVlabel.Size = new System.Drawing.Size(67, 13);
           maxYVlabel.TabIndex = 11;
           maxYVlabel.Text = "Max Y Value";

           nud_minYV = new NumericUpDown();
           ((ISupportInitialize)(nud_minYV)).BeginInit();
           tp.Controls.Add(nud_minYV);
           nud_minYV.Location = new System.Drawing.Point(6, 291);
           nud_minYV.Name = "nud_minYV";
           nud_minYV.Size = new System.Drawing.Size(120, 20);
           nud_minYV.TabIndex = 12;
           nud_minYV.ValueChanged += new EventHandler(nud_minYV_ValueChanged);
           ((ISupportInitialize)(nud_minYV)).EndInit();

           nud_maxYV = new NumericUpDown();
           ((ISupportInitialize)(nud_maxYV)).BeginInit();
           tp.Controls.Add(nud_maxYV);
           nud_maxYV.Location = new System.Drawing.Point(6, 317);
           nud_maxYV.Name = "nud_maxYV";
           nud_maxYV.Size = new System.Drawing.Size(120, 20);
           nud_maxYV.TabIndex = 13;
           nud_maxYV.ValueChanged += new EventHandler(nud_maxYV_ValueChanged);
           ((ISupportInitialize)(nud_maxYV)).EndInit();

           minXVlabel = new Label();
           tp.Controls.Add(minXVlabel);
           minXVlabel.AutoSize = true;
           minXVlabel.Location = new System.Drawing.Point(132, 246);
           minXVlabel.Name = "minXVlabel";
           minXVlabel.Size = new System.Drawing.Size(64, 13);
           minXVlabel.TabIndex = 14;
           minXVlabel.Text = "Min X Value";

           maxXVlabel = new Label();
           tp.Controls.Add(maxXVlabel);
           maxXVlabel.AutoSize = true;
           maxXVlabel.Location = new System.Drawing.Point(133, 272);
           maxXVlabel.Name = "maxXVlabel";
           maxXVlabel.Size = new System.Drawing.Size(67, 13);
           maxXVlabel.TabIndex = 15;
           maxXVlabel.Text = "Max X Value";

           nud_minXV = new NumericUpDown();
           ((ISupportInitialize)(nud_minXV)).BeginInit();
           tp.Controls.Add(nud_minXV);
           nud_minXV.Location = new System.Drawing.Point(6, 239);
           nud_minXV.Name = "nud_minXV";
           nud_minXV.Size = new System.Drawing.Size(120, 20);
           nud_minXV.TabIndex = 16;
           nud_minXV.ValueChanged += new EventHandler(nud_minXV_ValueChanged);
           ((ISupportInitialize)(nud_minXV)).EndInit();

           nud_maxXV = new NumericUpDown();
           ((ISupportInitialize)(nud_maxXV)).BeginInit();
           tp.Controls.Add(nud_maxXV);
           nud_maxXV.Location = new System.Drawing.Point(6, 265);
           nud_maxXV.Name = "nud_maxXV";
           nud_maxXV.Size = new System.Drawing.Size(120, 20);
           nud_maxXV.TabIndex = 17;
           nud_maxXV.ValueChanged += new EventHandler(nud_maxXV_ValueChanged);
           ((ISupportInitialize)(nud_maxXV)).EndInit();

           nud_nHor.Value = nHorzLines;
           nud_nVer.Value = nVertLines;
           nud_maxYV.Maximum = new Decimal(maxYVal + 100);
           nud_maxYV.Value = new Decimal(maxYVal);
           nud_maxYV.Minimum = new Decimal(minYVal - 100);

           nud_minYV.Maximum = new Decimal(maxYVal + 100);
           nud_minYV.Value = new Decimal(minYVal);
           nud_minYV.Minimum = new Decimal(minYVal - 100);

           nud_maxXV.Maximum = new Decimal(maxXVal + 100);
           nud_maxXV.Value = new Decimal(maxXVal);
           nud_maxXV.Minimum = new Decimal(minXVal - 100);

           nud_minXV.Maximum = new Decimal(maxXVal + 100);
           nud_minXV.Value = new Decimal(minXVal);
           nud_minXV.Minimum = new Decimal(minXVal - 100);
           xAxis_cb.Checked = draw_xLabel;
           xAxis_tb.Text = XLabelString;
           yAxis_cb.Checked = draw_yLabel;
           yAxis_tb.Text = YLabelString;
       }

       void nud_maxXV_ValueChanged(object sender, EventArgs e)
       {
           maxXVal = decimal.ToDouble(nud_maxXV.Value);
           InitLabels();
       }

       void nud_minXV_ValueChanged(object sender, EventArgs e)
       {
           minXVal = decimal.ToDouble(nud_minXV.Value);
           InitLabels();
       }

       void nud_maxYV_ValueChanged(object sender, EventArgs e)
       {
           maxYVal = decimal.ToDouble(nud_maxYV.Value);
           InitLabels();
       }

       void nud_minYV_ValueChanged(object sender, EventArgs e)
       {
           minYVal = decimal.ToDouble(nud_minYV.Value);
           InitLabels();
       }

       void nud_nVer_ValueChanged(object sender, EventArgs e)
       {
           nVertLines = (int)nud_nVer.Value;
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
