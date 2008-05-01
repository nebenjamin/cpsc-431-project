using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using OpenTK.OpenGL;
using OpenTK.Fonts;

//Note:  All constructors take arrays of string, holding the Cell contents



namespace ExcelClone.Graphs
{
    public enum Graph_Type { Test, Bar, Column, Scatter, Line, Pie }

    public abstract class Graph  //Graph base class- draw a grid, labels, legend, and Title
    {
        public List<List<double>> Data  //For serialization, all fields must be public or have a public property like this one
        {
            set 
            {
                /*if (data.Count == 0)
                    data = new List<List<double>>();
                foreach( List<double> dlist in Data)
                {
                    data.Add(new List<double>(dlist.Count));
                }*/
                data = Data;
            }
            get { return data; }
        }

        protected List<List<double>> data = new List<List<double>>(); //data in the graph, a list of lists of doubles

        public Rectangle clientRect;

        public PointF grLowLeft = new PointF(15, 15);  //points of graph area
        public PointF grUpRight = new PointF(85, 85);
        public double minXVal, maxXVal, minYVal, maxYVal;

        // number of horizontal and vertical lines
        public int nVertLines, nHorzLines;

        // enable/disable grid lines
        public bool vGrid; 
        public bool hGrid;

        public int XLabelOffset = 8;  //label offsets- distance from edge of graph area to axis label
        public int YLabelOffset = 8;
        protected int MaxYOffset = 0;  //The widest Y label, used for resizing the graph area

        public string TitleString = "Graph";  //Graph title
        public string XLabelString = "X axis";  //Axis labels
        public string YLabelString = "Y axis";

        protected TextureFont LabelFont;  //Font for drawing labels, title, axes
        protected TextureFont TitleFont;
        protected TextureFont AxesFont;
        protected List<TextHandle> XLabels = new List<TextHandle>();  //List of labels to draw for X, Y, title... with offsets
        protected List<double> XLabelOffsets = new List<double>();
        protected List<TextHandle> YLabels = new List<TextHandle>();
        protected List<double> YLabelOffsets = new List<double>();

        protected TextHandle XAxisLabel;  //Axis labels
        protected TextHandle YAxisLabel;
        protected TextHandle Title;  //Graph Title
        protected double TitleOffset;

        public List<Color> LegendColors = new List<Color>();  //Legend stuff- Colors, labels, label handles
        public List<String> LegendLabels = new List<string>();
        protected List<TextHandle> LegendTxtHandles = new List<TextHandle>();
        protected float LegendY;  //Legend Y location, for scaling purposes

        public bool LegendOn = false;
        public bool draw_xLabel = false;
        public bool draw_yLabel = false;
        public bool draw_title = false;

        protected ITextPrinter txp = new TextPrinter();  //Text printer - for drawing all text

        public Graph() { }

        // This is called before the subclass constructor
        public Graph(List<List<double>> newData)
        {
            LegendY = (grUpRight.Y - grLowLeft.Y) / 2 + grLowLeft.Y;  //Init legend stuff

            data = newData;

            setMinMax();
            setDefaults();

            InitFonts();
            InitLabels();
            InitColors();
        }

        public void InitColors()
        {
            LegendColors = new List<Color>();

            int red = 0, green = 0, blue = 0;
            System.Random RandNum = new System.Random();



            LegendColors.Add(Color.CadetBlue);
            LegendColors.Add(Color.BurlyWood);
            LegendColors.Add(Color.OrangeRed);
            LegendColors.Add(Color.Yellow);
            LegendColors.Add(Color.Silver);
            LegendColors.Add(Color.Purple);
            LegendColors.Add(Color.SeaGreen);
            LegendColors.Add(Color.MistyRose);

            // Fill LegendColors to match LegendLabels List Capacity. Prevents Data error

            while (LegendLabels.Capacity + 1 > LegendColors.Capacity)
            {
                red = RandNum.Next(255);
                green = RandNum.Next(255);
                blue = RandNum.Next(255);


                LegendColors.Add(Color.FromArgb(red, green, blue));
            }

        }

        public void InitFonts()
        {
            //Init Fonts to defaults
            LabelFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 10.0f));
            LabelFont.RebuildTextures();
            TitleFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 18.0f));
            AxesFont = new TextureFont(new Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold));
        }

        public void InitLabels()  //come up with labels based on data, line count
        {
            int i = 0;

            //X labels
            XLabels = new List<TextHandle>();
            double delta = (maxXVal-minXVal)/(nVertLines-1);
            double currX = minXVal;
            for (i = 0; i < nVertLines; i++, currX += delta )
            {
                TextHandle th;
                txp.Prepare(currX.ToString("####.##"), LabelFont, out th);
                XLabels.Add(th);
                XLabelOffsets.Add(((currX.ToString("####.##").Length) * LabelFont.Width) / 2.0 + 3);
            }
            //Y labels
            YLabels = new List<TextHandle>();
            delta = (maxYVal - minYVal) / (nHorzLines-1);
            double currY = minYVal;
            for (i = 0; i < nHorzLines; i++, currY += delta )
            {
                TextHandle th;
                float w, h;
                txp.Prepare(currY.ToString("####.##"), LabelFont, out th);
                YLabels.Add(th);
                LabelFont.MeasureString(currY.ToString("####.##"), out w, out h);
                YLabelOffsets.Add(w + 4);

                if ((w + 4) > MaxYOffset)  //track max offset
                    MaxYOffset = (int)(w + 4);
            }

            LegendLabels = new List<string>();
            if (this is scatter_graph || this is line_graph)
                i = 1;
            else
                i = 0;
            for ( ; i < data[0].Count; i++ )
            {
                LegendLabels.Add("Series" + (i+1).ToString());
            }

        }

        public void DrawTitle()
        {
            //Draw title at top center
            GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
            GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                
            txp.Begin();
            double titleX = clientRect.Right / 2.0 - TitleFont.Width * TitleString.Length / 2.0;
            double titleY = 0;
            GL.Translate(titleX, titleY, 0);
            txp.Draw(Title);
            txp.End();
        }

        public void DrawLegend(Rectangle ClientR) //draw a legend
        {
            float X = grUpRight.X + 2;
            float Y = LegendY;

            GL.MatrixMode(OpenTK.OpenGL.Enums.MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            // draw an item for each legend label
            int i = 0;
            foreach (string s in LegendLabels)
            {
                float w, h;
                w = LabelFont.Width;
                h = LabelFont.Height;
                //Convert measurement from pixels to relative coords
                w = w / ClientR.Right * 100;
                h = h / ClientR.Bottom * 100;
                //First, the colored box
                GL.Color3(LegendColors[i]); 
                i++;
                GL.Begin(OpenTK.OpenGL.Enums.BeginMode.Quads);
                GL.Vertex2(X, Y);
                GL.Vertex2(X+w,  Y);
                GL.Vertex2(X + w, Y - h);
                GL.Vertex2(X, Y - h);
                GL.End();
                //Now print the series name next to the colored box
                txp.Begin();
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                TextHandle th;
                GL.Translate( ((X + w)/100) * ClientR.Right, (1 - Y/100) * ClientR.Bottom, 0.0);
                txp.Prepare(s, LabelFont, out th);
                txp.Draw(th);
                txp.End();
                Y -= h + 2;
            }
            GL.PopMatrix();
        }

        //This method checks if the graph needs to shrink so the labels can fit
        public void CheckGraphArea()
        {
            //Reset to defaults
            grLowLeft = new PointF(15, 15);
            grUpRight = new PointF(85, 85);
            LegendY = (grUpRight.Y - grLowLeft.Y) / 2 + grLowLeft.Y;

            //Convert X and Y widths to pixels
            int Xpix = (int)(grLowLeft.Y / 100.0 * clientRect.Bottom);
            int Ypix = (int)(grLowLeft.X / 100.0 * clientRect.Right);
            int TitlePix = (int)((100 - grUpRight.Y) / 100.0 * clientRect.Bottom);
            int LegendPix = (int)((100 - grUpRight.X) / 100.0 * clientRect.Right);

            //Now get the width of the labels in each area
            int Xwidth, Ywidth, TitleWidth, LegendWidth;
            float LegendHeight;

            Xwidth = (int)(LabelFont.Height + AxesFont.Height + XLabelOffset + 2);
            //Need to use the widest Y label, width varies with character count
            Ywidth = (int)(MaxYOffset + LabelFont.Height + YLabelOffset + 2);
            TitleWidth = (int)(TitleFont.Height + 2);
            LegendWidth = 0;
            foreach (String s in LegendLabels)
            {
                float w, h;
                LabelFont.MeasureString(s, out w, out h);
                if (w > LegendWidth)
                    LegendWidth = (int)w;
            }
            LegendWidth += (int)LabelFont.Width + (int) (2/100.0 * clientRect.Right) + 5;  //the colored square is one character big

            LegendHeight = (LabelFont.Height / clientRect.Bottom * 100 + 2)  * LegendLabels.Count; //each label has a spacing of 2

            //check each area, correct if necessary to keep everything on screen
                if (Xpix < Xwidth)
                {
                    //Need to resize graph area for X labels
                    grLowLeft.Y = (float)Xwidth / (float)clientRect.Bottom * 100;
                }
                if (Ypix < Ywidth)
                {
                    //Need to resize area to fit Y labels
                    grLowLeft.X = (float)Ywidth / (float)clientRect.Right * 100;
                }
                if (TitlePix < TitleWidth)
                {
                    //Need to adjust for title area
                    grUpRight.Y = 100 - (float)TitleWidth / (float)clientRect.Bottom * 100;
                }
                if (LegendPix < LegendWidth)
                {
                    grUpRight.X = 100 - (float)LegendWidth / (float)clientRect.Right * 100;
                }
            if( LegendY + LegendHeight > 100 )
            {
                LegendY = LegendHeight;
            }
        }

        public void DrawAxis()  //Draw method, draw the grid/legend/title since this is the base class
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
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)(nVertLines - 1), grLowLeft.Y);
                    GL.Vertex2(grLowLeft.X + i * (grUpRight.X - grLowLeft.X) / (float)(nVertLines - 1), grUpRight.Y);
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
                    GL.Vertex2(grLowLeft.X, grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1));
                    GL.Vertex2(grUpRight.X, grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1));
                    GL.End();

                    //Draw Y labels
                    txp.Begin();
                    //Kill texture filter for better looking text
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                    GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                    //Move to label location, convert object coords to pixels
                    double labelY = (1 - (grLowLeft.Y + i * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1)) / 100.0) * clientRect.Bottom - LabelFont.Height / 2.0;
                    double labelX = (grLowLeft.X / 100.0) * clientRect.Right - YLabelOffsets[i];
                    if (this is bar_graph)  //offset bar graph labels to be under the bars, for prettiness
                    {
                        labelY -= (((1 - (grLowLeft.Y + (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1)) / 100.0) * clientRect.Bottom - LabelFont.Height / 2.0) - ((1 - (grLowLeft.Y + 0 * (grUpRight.Y - grLowLeft.Y) / (float)(nHorzLines - 1)) / 100.0) * clientRect.Bottom - LabelFont.Height / 2.0)) / 2.0;
                    }
                    GL.Translate(labelX, labelY, 0);
                    //Draw label using handle of text and end
                    txp.Draw(((TextHandle)YLabels[i]));
                    txp.End();
                }
            }
           //Draw axis labels

            if (draw_xLabel)
            {
                //X
                txp.Begin();
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                float w, h;
                AxesFont.MeasureString(XLabelString, out w, out h);
                double X = ((grUpRight.X - grLowLeft.X) / 2 + grLowLeft.X) / 100.0 * clientRect.Right - w / 2;
                double Y = clientRect.Bottom - AxesFont.Height - XLabelOffset;
                GL.Translate(X, Y, 0.0);
                txp.Draw(XAxisLabel);
                txp.End();
            }

            if (draw_yLabel)
            {
                //Y
                txp.Begin();
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMinFilter, (int)OpenTK.OpenGL.Enums.TextureMinFilter.Nearest);
                GL.TexParameter(OpenTK.OpenGL.Enums.TextureTarget.Texture2d, OpenTK.OpenGL.Enums.TextureParameterName.TextureMagFilter, (int)OpenTK.OpenGL.Enums.TextureMagFilter.Nearest);
                float w, h;
                AxesFont.MeasureString(YLabelString, out w, out h);
                double X = YLabelOffset;
                double Y = ((grUpRight.Y - grLowLeft.Y) / 2 + (100 - grUpRight.Y)) / 100.0 * clientRect.Bottom + w / 2;
                GL.Translate(X, Y, 0.0);
                GL.Rotate(-90.0, 0.0, 0.0, 1.0);
                txp.Draw(YAxisLabel);
                txp.End();
            }
        }

        public float xOfGraph(float x_value)
        {
            float number = grUpRight.X - grLowLeft.X;
            number *= (x_value-(float)minXVal);
            number /= (float)(maxXVal - minXVal);

            return number + grLowLeft.X;
        }

        public float yOfGraph(float y_value)
        {
            float number = grUpRight.Y - grLowLeft.Y;
            number *= (y_value - (float)minYVal);
            number /= (float)(maxYVal - minYVal);

            return number + grLowLeft.Y;
        }

        public static string[][] sampleData()
        {
            Random ran = new Random();
            int columns = ran.Next(2,4);
            int rows = ran.Next(3, 7);

            string[][] data = new string[columns][];

            for (int i = 0; i < columns; i++)
            {
                data[i] = new string[rows];
                for (int j = 0; j < rows; j++)
                {
                    data[i][j] = ran.Next(5, 80).ToString();
                }
            }

            return data;
        }



        public abstract void drawGraph(Rectangle r);
        public abstract void setMinMax();
        public abstract void setDefaults();
        public abstract void configTab(TabPage tb);
        public abstract Graph cloneGraph();
    }
}
