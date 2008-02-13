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

        public GraphControl()
        {
            gr = new Graph();  //use default constructor for a test graph object
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
            gr.Draw();

            SwapBuffers();
            base.OnPaint(e);
        }
    }
}

