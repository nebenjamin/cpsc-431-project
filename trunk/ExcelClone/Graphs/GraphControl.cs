using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using OpenTK.OpenGL;

namespace ExcelClone.Graphs
{
    public partial class GraphControl : OpenTK.GLControl
    {
        private Graph gr;  //the displayed graph object
        private bool moving;
        private int initX, initY;

        public bool inMenu;

        public GraphControl()  //Make control from an existing graph, but must init control before graph- need blank constructor
        {
            moving = false;
            inMenu = false;
            InitializeComponent();
        }

        public void SetGraph(Graph newGraph)
        {
            gr = newGraph;
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

        private void GraphControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();
            moving = true;
            initX = e.X;
            initY = e.Y;
        }

        private void GraphControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                int deltaX = e.X - initX;
                int deltaY = e.Y - initY;

                this.Location = new Point(this.Location.X + deltaX, this.Location.Y + deltaY);
            }
        }

        private void GraphControl_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inMenu)
                return;

            Point prevLoc = this.Location;
            Size prevSize = this.Size;
            Form parent = this.ParentForm;
            Graph prevGr = gr.cloneGraph();

            this.Location = new Point(0, 0);  //gc.loc is a point, not rect
            this.Size = new Size(450, 400);

            graphConfig gConf = new graphConfig(gr, this);
            gConf.ShowDialog();

            GraphControl gc = new GraphControl();
            gc.Location = prevLoc;
            gc.Size = prevSize;
            if (gConf.DialogResult == DialogResult.OK)
                gc.SetGraph(gr);
            else
                gc.SetGraph(prevGr);
            gr.InitFonts();
            gr.InitLabels();

            parent.Controls.Add(gc);
            gc.BringToFront();
        }
    }
}

