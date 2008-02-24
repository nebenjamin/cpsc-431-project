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

        public PointF grLowLeft = new PointF(15, 15);  //points of graph area
        public PointF grUpRight = new PointF(85, 85);

        public int nVertLines = 5;  //Vertical/Horizontal grid line count
        public int nHorzLines = 5;

        public bool vGrid;  //enable / disable grid lines
        public bool hGrid;

        public int XLabelOffset = 8;  //label offsets- distance from edge of graph area to axis label
        public int YLabelOffset = 8;
        private int MaxYOffset = 0;  //The widest Y label, used for resizing the graph area

        public string TitleString = "Graph";  //Graph title
        public string XLabelString = "X axis";  //Axis labels
        public string YLabelString = "Y axis";

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

            InitFonts();
            InitLabels();
        }

        public void InitFonts()
        {
            //Init Fonts to defaults
            LabelFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 10.0f));
            TitleFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 18.0f));
            AxesFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold));
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
                    XLabelOffsets.Add( ((obj.ToString().Length) * LabelFont.Width) / 2.0 + 3);
                }
            }
            //Y labels
            if (((ArrayList)data[1]).Count == nHorzLines)  //label per datum case
            {
                foreach (object obj in ((ArrayList)data[1]))
                {
                    TextHandle th;
                    float w, h;
                    txp.Prepare(obj.ToString(), LabelFont, out th);
                    YLabels.Add(th);
                    LabelFont.MeasureString(obj.ToString(), out w, out h);
                    YLabelOffsets.Add(w + 4);

                    if ((w + 4) > MaxYOffset)  //track max offset
                        MaxYOffset = (int)(w + 4);
                }
            }
            //Graph Title
            txp.Prepare(TitleString, TitleFont, out Title);
            //Axis labels
            txp.Prepare(XLabelString, AxesFont, out XAxisLabel);
            txp.Prepare(YLabelString, AxesFont, out YAxisLabel);
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

        //This method checks if the graph needs to shrink so the labels can fit
        public bool CheckGraphArea(Rectangle rect, out PointF LowLeft, out PointF UpRight)
        {
            //Convert X and Y widths to pixels
            int Xpix = (int)(grLowLeft.Y / 100.0 * rect.Bottom);
            int Ypix = (int)(grLowLeft.X / 100.0 * rect.Right);

            //Now get the width of the labels in each area
            int Xwidth, Ywidth;
            Xwidth = (int)(LabelFont.Height + AxesFont.Height + XLabelOffset + 2);

            //Need to use the widest Y label, width varies with character count
            Ywidth = (int)(MaxYOffset + LabelFont.Height + YLabelOffset + 2);

            if (Xpix < Xwidth || Ypix < Ywidth)
            {
                //Need to resize graph area
                LowLeft = new PointF();
                LowLeft.X = (float)Ywidth / (float)rect.Right * 100;
                LowLeft.Y = (float)Xwidth / (float)rect.Bottom * 100;
                UpRight = grUpRight;
                return false;
            }
            else
            {
                //Everything OK
                LowLeft = grLowLeft;
                UpRight = grUpRight;
                return true;
            }
        }

        public void Draw(Rectangle clientRect)  //Draw method, draw the grid/legend/title since this is the base class
        {
            PointF UpR, LowL;
            //First, make sure the labels will be on screen
            CheckGraphArea(clientRect, out LowL, out UpR);
            
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
                    GL.Vertex2(LowL.X + i * (UpR.X - LowL.X) / (float)(nVertLines-1), LowL.Y);
                    GL.Vertex2(LowL.X + i * (UpR.X - LowL.X) / (float)(nVertLines-1), UpR.Y);
                    GL.End();

                    //Draw X labels
                    txp.Begin();
                    //Kill texture filter for better looking text
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                    //Move to label location, convert object coords to pixels
                    double labelY = (1 - LowL.Y / 100.0) * clientRect.Bottom;
                    double labelX = ((LowL.X + i * (UpR.X - LowL.X) / (float)(nVertLines - 1)) / 100.0) * clientRect.Right - XLabelOffsets[i];
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
                    GL.Vertex2(LowL.X, LowL.Y + i * (UpR.Y - LowL.Y) / (float)(nHorzLines-1));
                    GL.Vertex2(UpR.X, LowL.Y + i * (UpR.Y - LowL.Y) / (float)(nHorzLines-1));
                    GL.End();

                    //Draw Y labels
                    txp.Begin();
                    //Kill texture filter for better looking text
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                    //Move to label location, convert object coords to pixels
                    double labelY = (1 - (LowL.Y + i * (UpR.Y - LowL.Y) / (float)(nHorzLines - 1)) / 100.0) * clientRect.Bottom - LabelFont.Height / 2.0;
                    double labelX = (LowL.X/100.0) * clientRect.Right - YLabelOffsets[i];
                    GL.Translate(labelX, labelY, 0);
                    //Draw label using handle of text and end
                    txp.Draw(((TextHandle)YLabels[i]));
                    txp.End();
                }
            }
           //Draw axis labels
            
            txp.Begin();
            //X
            float w, h;
            AxesFont.MeasureString(XLabelString, out w, out h);
            double X = ((UpR.X - LowL.X) / 2 + LowL.X) / 100.0 * clientRect.Right - w/2;
            double Y = clientRect.Bottom - AxesFont.Height - XLabelOffset;
            GL.Translate(X, Y, 0.0);
            txp.Draw(XAxisLabel);
            txp.End();

            //Y
            txp.Begin();
            AxesFont.MeasureString(YLabelString, out w, out h);
            X = YLabelOffset;
            Y = ((UpR.Y - LowL.Y) / 2 + LowL.Y) / 100.0 * clientRect.Bottom + w/2;
            GL.Translate(X, Y, 0.0);
            GL.Rotate(-90.0, 0.0, 0.0, 1.0);
            txp.Draw(YAxisLabel);
            txp.End();
        }
    }
}
