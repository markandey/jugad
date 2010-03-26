using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;

namespace SpellCheck
{
    public partial class Form1 : Form
    {
        spell s = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


    
 
        }
        private void checkspell()
        {
                 
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                string dict=sr.ReadToEnd();
                s = new spell(dict);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = s.correct(textBox1.Text);
        }
    }
}