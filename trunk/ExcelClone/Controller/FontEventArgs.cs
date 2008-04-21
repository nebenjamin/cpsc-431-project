using System.Drawing;
using System;
public class FontEventArgs : EventArgs
{
    public FontEventArgs(string familyName, float size)
    {
        this.FamilyName = familyName;
        this.Size = size;
    }

    public string FamilyName;
    public float Size;


}

