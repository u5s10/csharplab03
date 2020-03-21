using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csharplab03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var slnFilePath = fbd.SelectedPath;

                string[] slnPath = Directory.GetFiles(slnFilePath, "*.sln"); // finding the sln file
                string[] lines = File.ReadAllLines(slnPath[0]); // loading sln file to string array

                foreach(var line in lines)
                {
                    Console.WriteLine(line);
                }

                MessageBox.Show("Done.");

            }
        }
    }
}
