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
using OpenTK.OpenGL.Enums;

namespace ExcelClone.Graphs
{
    public partial class GraphControl : OpenTK.GLControl, IXmlSerializable
    {
        private Graph gr;  //the displayed graph object

        //private vars used with mouse control
        private bool moving;
        private bool resizing;
        private Size oldSize;
        private double TriXCoord, TriYCoord;
        private int initX, initY;

        public bool inMenu;

        public GraphControl()  //Make control from an existing graph, but must init control before graph- need blank constructor
        {
            moving = false;
            resizing = false;
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
            //Draw a resize triangle if in focus
            if (this.Focused)
            {
                TriXCoord = 30.0 / (float)this.Width * 100;  //Get 30 pixel coords for a constant size triangle
                TriYCoord = 30.0 / (float)this.Height * 100;
                GL.Color3(Color.Black);
                GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Triangles);
                GL.Vertex2(100, TriYCoord);
                GL.Vertex2(100, 0);
                GL.Vertex2(100 - TriXCoord, 0);
                GL.End();
            }
            SwapBuffers();
            base.OnPaint(e);
        }

        private void GraphControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();
            if (this.Cursor == Cursors.Hand)
                moving = true;
            if (this.Cursor == Cursors.SizeAll)
            {
                resizing = true;
                oldSize = this.Size;
            }
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
            else if (resizing)
            {
                int deltaX = e.X - initX;
                int deltaY = e.Y - initY;

                this.Size = oldSize + new Size(deltaX, deltaY);
            }
            else
            {
                float Xcoord = (float)e.X / this.Size.Width * 100;
                float Ycoord = 100 - (float)e.Y / this.Size.Height * 100;
                double m = TriYCoord / (100 - TriXCoord);
                double my = m * (Xcoord - TriXCoord);

                if (Xcoord > (100 - TriXCoord) && Ycoord < my)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                else
                {
                    this.Cursor = Cursors.Hand;
                }
            }
        }

        private void GraphControl_MouseUp(object sender, MouseEventArgs e)
        {
            resizing = false;
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

        private void GraphControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                this.Parent.Controls.Remove(this);  //commit suicide when delete is pressed
        }

        private void GraphControl_Enter(object sender, EventArgs e)
        {
            this.Invalidate();  //Make sure we redraw so the user sees the resize triangle
        }

        private void GraphControl_Leave(object sender, EventArgs e)
        {
            resizing = false;
            moving = false;
            this.Invalidate();  //Make sure the resize triangle dissappears
        }

        private void GraphControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ReadXml(XmlReader reader)  //For loading a saved graph
        {
            //First, figure out what kind of graph needs to be loaded by getting the type (bar, pie...)
            Type graphType = System.Type.GetType("ExcelClone.Graphs." + reader.Name, false, true);

            //Now, create an XMLserializer for that type and deserialize it into this control
            XmlSerializer xms = new XmlSerializer(graphType);
            gr = (Graph)xms.Deserialize(reader);

            gr.InitFonts();  //init font and label objects, they cannot be saved
            gr.InitLabels();
        }

        public void WriteXml(XmlWriter writer)
        {
            //Make a serializer for the graph type, and serialize it
            XmlSerializer xms = new XmlSerializer(gr.GetType());
            xms.Serialize(writer, gr);

            //Now, add info about this control
            writer.WriteStartElement("GraphControl");
            writer.WriteStartAttribute("Position");
            writer.WriteValue(this.Location.ToString());
            writer.WriteEndAttribute();
            writer.WriteStartAttribute("Size");
            writer.WriteValue(this.Size.ToString());
            writer.WriteEndAttribute();
            writer.WriteEndElement();
        }

        #endregion
    }
}

