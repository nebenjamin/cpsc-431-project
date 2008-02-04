using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FunctionParser
{
    public partial class Form1 : Form
    {
        Parser A;
        Functions B;

        public Form1()
        {
            InitializeComponent();

            A = new Parser();
            B = new Functions();
            ArrayList temp = B.Get_Functions();
            temp.Sort();
            for (int i = 0; i < temp.Count; i++)
            {
                listBox1.Items.Add(temp[i]);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = A.Parse("A1", textBox1.Text);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text += listBox1.SelectedItem.ToString() + "(";
        }
    }
}