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
        private TextBox yAxis_tb;
        private CheckBox yAxis_cb;
        private Label yAxis_lb;
        private CheckBox xAxis_cb;
        private TextBox xAxis_tb;
        private Label xAxis_lb;

        //Create a bar graph, add it to a parent form, fill in data
        public static Graph Create_Bar_Graph(Form parent, Rectangle location, string[][] data)
        {
            //First, make a bar graph and add the data
            GraphControl gc = new GraphControl();
            List<List<double>> newData = new List<List<double>>();

            for( int i = 0; i < data.Length; i++ )
            {
                newData.Add(new List<double>());
                for( int j = 0; j < data[i].Length; j++ )
                {
                    double parsedDouble;
                    if (!Double.TryParse(data[i][j], out parsedDouble))
                        throw new ArgumentException("Invalid data in Cells");
                    newData[i].Add(parsedDouble);
                }
            }

            Graph gr = new bar_graph(newData);

            gc.Location = new Point(0, 0);  //gc.loc is a point, not rect
            gc.Size = new Size(450,400);
            gc.SetGraph(gr);

            graphConfig gConf = new graphConfig(gr,gc);
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
            if(draw_title)
                DrawTitle();
            if(LegendOn)
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
        public override Graph cloneGraph()
        {
            bar_graph gr = new bar_graph(data);
            gr.draw_title = draw_title;
            gr.TitleString = TitleString;
            gr.LegendOn = LegendOn;
            gr.draw_xLabel = draw_xLabel;
            gr.XLabelString = XLabelString;
            gr.draw_yLabel = draw_yLabel;
            gr.YLabelString = YLabelString;
            return gr;
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
