using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;

namespace FunctionParser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            /*Parser a = new Parser();
            System.Console.WriteLine(a.Parse("C5", "=ADD(2,3,1)*2+4/2"));
            System.Console.WriteLine(a.Parse("C5", "=2/(3*2)+4/2"));
            System.Console.WriteLine(a.Parse("C5", "=2/((3*2)+4/2)"));
            System.Console.WriteLine(a.Parse("C5", "=SUB(2,3,ADD(1,2))*2+(4+4)/2"));
            System.Console.WriteLine(a.Parse("C5", "=(2+)+2"));
            System.Console.WriteLine(a.Parse("C5", "=PI()*2"));*/
        }
    }
}