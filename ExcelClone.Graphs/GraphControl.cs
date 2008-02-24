using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK.OpenGL;

namespace ExcelClone.Graphs
{
    public partial class GraphControl : OpenTK.GLControl
    {
        private Graph gr;  //the displayed graph object

        public GraphControl( Graph_Type type)
        {
            switch (type)
            {
                case Graph_Type.Test:
                    gr = new test_graph();  //use default constructor for a test graph object
                    break;
                case Graph_Type.Column:
                    gr = new column_graph();
                    break;
                case Graph_Type.Bar:
                    gr = new bar_graph();
                    break;
                case Graph_Type.Line:
                    gr = new line_graph();
                    break;
                case Graph_Type.Scatter:
                    gr = new scatter_graph();
                    break;
                case Graph_Type.Pie:
                    gr = new pie_graph();
                    break;
                default:
                    gr = new test_graph();  //use default constructor for a test graph object
                    break;
            }
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            int w = this.Width;    //set up the projection for drawing graphs
            int h = this.Height;

            GL.MatrixMode(OpenTK.OpenGL.Enums.MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, w, h);
            GL.Ortho(0, 100, 0, 100, -50, 50);

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            gr.drawGraph(this.ClientRectangle);
            SwapBuffers();
            base.OnPaint(e);
        }
    }
}

