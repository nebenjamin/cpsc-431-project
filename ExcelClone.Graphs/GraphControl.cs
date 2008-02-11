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
        public GraphControl()
        {
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
            GL.Ortho(0, 100, 0, 100, -2 * h, 2 * h);

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            GL.ClearColor(Color.White);
            GL.Clear(OpenTK.OpenGL.Enums.ClearBufferMask.ColorBufferBit | OpenTK.OpenGL.Enums.ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(OpenTK.OpenGL.Enums.MatrixMode.Modelview);
            GL.LoadIdentity();

            int[] points = {0,10,3,10,45,42, 5, 7};

            GL.Color3(Color.Gray);
            GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Lines);
            GL.Vertex2(10, 10);
            GL.Vertex2(10, 90);
            GL.End();

            SwapBuffers();
            base.OnPaint(e);
        }
    }
}

