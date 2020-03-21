using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                var lines = File.ReadAllLines(slnPath[0]); // loading sln file to string array

                var csprojFilesRegex = new Regex(@"([^""]+.csproj)""");
                List<string> csprojFiles = new List<string>();               

                foreach (var line in lines)
                { 
                    Match match = csprojFilesRegex.Match(line);
                    if (match.Success)
                    {
                        csprojFiles.Add(match.Groups[1].Value);
                        //Console.WriteLine(match.Groups[1].Value);
                    }
                }

                
                var dirRegex = new Regex(@"(.+\\)");
                List<string> dirs = new List<string>();
                foreach(var file in csprojFiles)
                {
                    Match match = dirRegex.Match(file);
                    if (match.Success)
                    {
                        dirs.Add(match.Groups[1].Value);
                        //Console.WriteLine(match.Groups[1].Value);
                    }
                    else
                    {
                        dirs.Add("");
                    }
                }

                System.IO.Directory.CreateDirectory(String.Format("{0}\\Copy", slnFilePath));
                foreach(var dir in dirs)
                {
                    System.IO.Directory.CreateDirectory(String.Format("{0}\\Copy\\{1}", slnFilePath,dir));
                }


                var filesRegex = new Regex(@"(?<=(?:Compile|None|EmbeddedResource) Include=)""([^""]+)""");
                for (int i = 0; i < csprojFiles.Count; i++)
                {
                    var csprojLines = File.ReadAllLines(string.Format("{0}\\{1}",slnFilePath,csprojFiles[i]));
                    foreach(var line in csprojLines)
                    {
                        Match match = filesRegex.Match(line);
                        if (match.Success)
                        {
                            try
                            {
                                var additional = new Regex(@"(.+\\)");
                                Match match2 = additional.Match(match.Groups[1].Value);
                                if (match2.Success)
                                {
                                    System.IO.Directory.CreateDirectory(slnFilePath + "\\Copy\\" + dirs[i] + "\\" + match2.Groups[1]); //                                
                                }
                                
                                File.Copy((slnFilePath + "\\" + dirs[i] + match.Groups[1].Value),
                                    (slnFilePath + "\\Copy\\" + dirs[i] + match.Groups[1].Value), true);
                                
                            }
                            catch (IOException iox)
                            {
                                Console.WriteLine(iox.Message);
                            }                          
                        }
                    }
                    File.Copy(slnFilePath +"\\"+ csprojFiles[i], slnFilePath + "\\Copy\\" + csprojFiles[i], true); // copying sln files to Copy

                }

                var slnFileNameRegex = new Regex(@"(\\[^\\]+\.sln)");
                Match slnFileNameMatch = slnFileNameRegex.Match(slnPath[0]);
                if (slnFileNameMatch.Success)
                {
                    File.Copy(slnPath[0], slnFilePath + "\\Copy\\" + slnFileNameMatch.Groups[1].Value, true);
                }
                //File.Copy(slnPath[0], slnFilePath + "\\Copy\\" + slnPath[0], true);
                
                
                MessageBox.Show("Done.");
            }
        }
    }
}
