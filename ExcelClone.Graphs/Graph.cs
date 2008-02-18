using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenTK.OpenGL;
using OpenTK.Fonts;

//Note:  All constructors take arrays of string, holding the Cell contents

namespace ExcelClone.Graphs
{
    public class Graph  //Graph base class- draw a grid, labels, legend, and Title
    {
        private ArrayList data = new ArrayList(); //data in the graph

        public Point grLowLeft = new Point(15, 15);  //points of graph area
        public Point grUpRight = new Point(85, 85);

        public int nVertLines = 5;  //Vertical/Horizontal grid line count
        public int nHorzLines = 5;

        public bool vGrid;  //enable / disable grid lines
        public bool hGrid;

        public string TitleString = "Graph";  //Graph title

        private TextureFont LabelFont;  //Font for drawing labels, title, axes
        private TextureFont TitleFont;
        private TextureFont AxesFont;
        private List<TextHandle> XLabels = new List<TextHandle>();  //List of labels to draw for X, Y, title... with offsets
        private List<double> XLabelOffsets = new List<double>();
        private List<TextHandle> YLabels = new List<TextHandle>();
        private List<double> YLabelOffsets = new List<double>();

        private TextHandle XAxisLabel;  //Axis labels
        private TextHandle YAxisLabel;
        private TextHandle Title;  //Graph Title
        private double TitleOffset;

        ITextPrinter txp = new TextPrinter();  //Text printer - for drawing all text
        
        public Graph()
        {
            data.Add(new ArrayList());  //set data up with two rows of data
            data.Add(new ArrayList());

            Random ran = new Random();
            //fill in sample data for testing
            for (int i=0; i < 5; i++)
            {
                ((ArrayList)data[0]).Add(i);
                ((ArrayList)data[1]).Add(ran.Next());
            }

            vGrid = true;
            hGrid = true;

            //Init Fonts to defaults
            LabelFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 12.0f));
            TitleFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 18.0f));
            AxesFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold));

            InitLabels();
        }

        public void InitLabels()  //come up with labels based on data, line count
        {
            
            //X labels
            if (((ArrayList)data[0]).Count == nVertLines)  //label per datum case
            {
                foreach (object obj in ((ArrayList)data[0]))
                {
                    TextHandle th;
                    txp.Prepare(obj.ToString(), LabelFont, out th);
                    XLabels.Add(th);
                    XLabelOffsets.Add( ((obj.ToString().Length) * LabelFont.Width) / 2.0 - 2);
                }
            }
            //Y labels
            if (((ArrayList)data[1]).Count == nHorzLines)  //label per datum case
            {
                foreach (object obj in ((ArrayList)data[1]))
                {
                    TextHandle th;
                    txp.Prepare(obj.ToString(), LabelFont, out th);
                    YLabels.Add(th);
                    YLabelOffsets.Add((obj.ToString().Length - 2) * LabelFont.Width + 2);
                }
            }
            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);
        }

        public void DrawTitle(Rectangle clientRect)
        {
            //Draw title at top center
            txp.Begin();
            double titleX = clientRect.Right / 2.0 - TitleFont.Width * TitleString.Length / 2.0;
            double titleY = 0;
            GL.Translate(titleX, titleY, 0);
            txp.Draw(Title);
            txp.End();
        }

        public void Draw(Rectangle clientRect)  //Draw method, draw the grid since this is the base class
        {
            GL.ClearColor(Color.White);
            GL.Clear(OpenTK.OpenGL.Enums.ClearBufferMask.ColorBufferBit | OpenTK.OpenGL.Enums.ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(OpenTK.OpenGL.Enums.MatrixMode.Modelview);
            GL.LoadIdentity();

            //Draw a grid
            GL.Color3(Color.Gray);
            //Vertical
            if (vGrid)
            {
                for (int i = 0; i < nVertLines; i++)  //draw evenly spaced grid lines
                {
                    //Cast these to float for a nice smooth divide
                    GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Lines);
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)(nVertLines-1), grLowLeft.Y);
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)(nVertLines-1), grUpRight.Y);
                    GL.End();

                    //Draw X labels
                    txp.Begin();
                    //Kill texture filter for better looking text
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                    //Move to label location, convert object coords to pixels
                    double labelY = (1 - grLowLeft.Y / 100.0) * clientRect.Bottom;
                    double labelX = ((grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)(nVertLines - 1)) / 100.0) * clientRect.Right - XLabelOffsets[i];
                    GL.Translate(labelX, labelY, 0);
                    //Draw label using handle of text and end
                    txp.Draw(XLabels[i]);
                    txp.End();
                }
            }
            //Horizontal
            if (hGrid)
            {
                for (int i = 0; i < nHorzLines; i++)  //draw evenly spaced horizontal grid lines
                {
                    //Cast these to float for a nice smooth divide
                    GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Lines);
                    GL.Vertex2(grLowLeft.X, grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines-1));
                    GL.Vertex2(grUpRight.X, grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines-1));
                    GL.End();

                    //Draw Y labels
                    txp.Begin();
                    //Kill texture filter for better looking text
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                    //Move to label location, convert object coords to pixels
                    double labelY = (1 - (grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1)) / 100.0) * clientRect.Bottom - LabelFont.Height / 2.0;
                    double labelX = (grLowLeft.X/100.0) * clientRect.Right - YLabelOffsets[i];
                    GL.Translate(labelX, labelY, 0);
                    //Draw label using handle of text and end
                    txp.Draw(((TextHandle)YLabels[i]));
                    txp.End();
                }
            }
           
        }
    }
}
