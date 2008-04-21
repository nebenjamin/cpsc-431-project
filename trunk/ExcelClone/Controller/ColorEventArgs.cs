using System.Drawing;
using System;
public class ColorEventArgs : EventArgs
{
  public ColorEventArgs(Color c)
  {
    this.Color = c;    
  }
  public Color Color;
}	

