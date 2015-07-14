using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Ini;

namespace SendtoPam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
        }
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            PopulateTreeView(treeView1.Nodes, textBox1.Text);
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (Directory.Exists(files[0]))
                {
                    this.textBox1.Text = files[0];
                }
            }
        }
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            updateCommandText();
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            updateCommandText();
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            updateCommandText();
        }
       

        private void textBox9_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (File.Exists(files[0]))
                {
                    this.textBox9.Text = files[0];
                }
            }
        }
        private void textBox9_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void textBox10_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (File.Exists(files[0]))
                {
                    string extension = Path.GetExtension(files[0]).Replace(".", "");
                    this.textBox10.Text = extension;
                    this.textBox5.Text = extension;
                }
            }
        }
        private void textBox10_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (Directory.Exists(files[0]))
                {
                    this.textBox2.Text = files[0];
                }
            }
        }
        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void PopulateTreeView(TreeNodeCollection nodes, string path)
        {
            try
            {
                nodes.Clear();
                string[] dirs = Directory.GetDirectories(path);
                
                
                foreach (string dir in dirs)
                {
               
                    nodes.Add(dir, Path.GetFileName(dir));
                   
                }

                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file).Replace(".", "");
                    string filenamepart1 = Path.GetFileNameWithoutExtension(file);
                    bool b = false;
                    //string filtertext = this.textBox11.Text;
                    //if (filtertext == "_Original")
                    //{
                    //    b = true;

                    //}
                    //MessageBox.Show(filtertext);
                    //enumerateFilters(filenamepart1);
                    if (radioButton3.Checked)
                    {
                        if (extension == textBox4.Text || extension == textBox5.Text || extension == textBox3.Text)
                        {
                            
                            string fullPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);
                            string projectName = Path.GetFileName(fullPath);

                            //MessageBox.Show(projectName);
                            //string filenamepart1 = Path.GetFileNameWithoutExtension(file);
                            if (extension == textBox3.Text)
                            {
                                if (!(filenamepart1 == projectName))
                                {

                                    b = enumerateFilters(filenamepart1);
                                    if (!(b))
                                    {
                                        nodes.Add(file, Path.GetFileName(file));
                                    }

                                }
                            }
                            else
                            {
                                b = enumerateFilters(filenamepart1);
                                if (!(b))
                                {
                                    nodes.Add(file, Path.GetFileName(file));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (extension == textBox4.Text || extension == textBox5.Text)
                        {
                            b = enumerateFilters(filenamepart1);
                            if (!(b))
                            {
                                nodes.Add(file, Path.GetFileName(file));
                            }

                        }
                    }
                }
                    
                
                foreach (TreeNode node in nodes)
                {
                    PopulateTreeView(node.Nodes, node.Name);
                    
                }
                treeView1.ExpandAll();
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = "Send to PAM - WORKING";
            if (radioButton1.Checked)
            { 
                foreach (TreeNode node in treeView1.Nodes)
                {
                    string filename = "";
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox4.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            filename = filename1;
                            //MessageBox.Show("renaming " + "\n" + filename1 + "." + extension + " to: " + "\n"  +filename1 + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename1 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename1 + "." + textBox6.Text;
                               
                            File.Move(sourceFile,destinationFile); 
                        }
                        
                        

                    }
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox5.Text)
                        {
                            string filename2 = Path.GetFileNameWithoutExtension(child.Name);
                            //MessageBox.Show("renaming " + "\n" + filename2 + "." + extension + " to: " + "\n" +filename + "." + extension);
                            string sourceFile = child.Parent.Name + "\\" + filename2 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename + "." + textBox7.Text;

                            File.Move(sourceFile, destinationFile);
                        }



                    }
                }
            }
            if (radioButton2.Checked)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    string filename = "";
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox5.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            filename = filename1;
                            //MessageBox.Show("renaming " + filename1 + "." + extension + " to: " + filename1 + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename1 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename1 + "." + textBox7.Text;
                            File.Move(sourceFile, destinationFile);
                        }



                    }
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox4.Text)
                        {
                            string filename2 = Path.GetFileNameWithoutExtension(child.Name);
                            //MessageBox.Show("renaming " + "\n" + filename2 + "." + extension + " to: " + "\n" +filename + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename2 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename + "." + textBox6.Text;
                            File.Move(sourceFile, destinationFile);

                        }



                    }
                }
            }
            if (radioButton3.Checked)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    string filename = "";
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox3.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            filename = filename1;
                            //MessageBox.Show("renaming " + filename1 + "." + extension + " to: " + filename1 + ".271");
                            
                        }



                    }

                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox5.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            //filename = filename1;
                            //MessageBox.Show("renaming " + filename1 + "." + extension + " to: " + filename1 + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename1 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename + "." + textBox7.Text;
                            File.Move(sourceFile, destinationFile);
                        }



                    }
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox4.Text)
                        {
                            string filename2 = Path.GetFileNameWithoutExtension(child.Name);
                            //MessageBox.Show("renaming " + "\n" + filename2 + "." + extension + " to: " + "\n" +filename + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename2 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + filename + "." + textBox6.Text;
                            File.Move(sourceFile, destinationFile);

                        }



                    }
                }
            }
            if (radioButton4.Checked)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    string filename = "";
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox4.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            filename = filename1;
                            //MessageBox.Show("renaming " + "\n" + filename1 + "." + extension + " to: " + "\n"  +filename1 + ".271");
                            string sourceFile = child.Parent.Name + "\\" + filename1 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + node.Text + "." + textBox6.Text;
                            //MessageBox.Show(node.Text);
                            File.Move(sourceFile, destinationFile);
                        }



                    }
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox5.Text)
                        {
                            string filename2 = Path.GetFileNameWithoutExtension(child.Name);
                            //MessageBox.Show("renaming " + "\n" + filename2 + "." + extension + " to: " + "\n" +filename + "." + extension);
                            string sourceFile = child.Parent.Name + "\\" + filename2 + "." + extension;
                            string destinationFile = textBox2.Text + "\\" + node.Text + "." + textBox7.Text;
                            //MessageBox.Show(node.Text);
                            File.Move(sourceFile, destinationFile);
                        }



                    }
                }

            }
            PopulateTreeView(treeView1.Nodes, textBox1.Text);
            this.Text = "Send to PAM";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox3.Enabled = true;
                //PopulateTreeView(treeView1.Nodes, textBox1.Text);
            }
            else
            {
                textBox3.Enabled = false;
               // PopulateTreeView(treeView1.Nodes, textBox1.Text);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            PopulateTreeView(treeView1.Nodes, textBox1.Text);
            button6.BackColor = SystemColors.Control;
            button6.ForeColor = Color.Black;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = "X12";
            textBox5.Text = "PAM";
            textBox6.Text = "271";
            textBox7.Text = "PAM";
            textBox3.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            button6.BackColor = SystemColors.Control;
            button6.ForeColor = Color.Black;

            this.radioButton1.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private TreeNode m_OldSelectNode;
        private TreeNode m_SelectedNode;
        private void openFileLocationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            
            string filename = "\"" + textBox1.Text + "\\" + Convert.ToString(m_SelectedNode.FullPath) + "\"";
            //MessageBox.Show(filename);
            Process.Start("explorer.exe", "/select," + filename);
        }

        
        private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {

                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {

                    // Select the node the user has clicked.
                    // The node appears selected until the menu is displayed on the screen.
                    m_OldSelectNode = treeView1.SelectedNode;
                    treeView1.SelectedNode = node;
                    m_SelectedNode = treeView1.SelectedNode;
                    // Find the appropriate ContextMenu depending on the selected node.
                    //switch (Convert.ToString(node.Tag))
                    //{
                        //case "TextFile":
                    contextMenuStrip1.Show(treeView1, p);
                           // break;
                       // case "File":
                         //   mnuFile.Show(treeView1, p);
                           // break;
                   // }

                    // Highlight the selected node.
                    treeView1.SelectedNode = node;
                    m_OldSelectNode = null;
                }
            }
        }

        private void updateCommandText()
        {
            textBox8.Text = comboBox1.Text + " \"" + textBox9.Text + "\" \"*." + textBox10.Text + "\"";
        }
    

        private void button5_Click(object sender, EventArgs e)
        {
            this.Text = "Send to PAM - RUNNING COMMAND";
            
                foreach (TreeNode node in treeView1.Nodes)
                {
                    string filename = "";
                    foreach (TreeNode child in node.Nodes)
                    {
                        string extension = Path.GetExtension(child.Name).Replace(".", "");
                        if (extension == textBox10.Text)
                        {
                            string filename1 = Path.GetFileNameWithoutExtension(child.Name);
                            //filename = filename1;
                            string sourceFile = child.Parent.Name + "\\" + filename1 + "." + extension;
                            //textBox8.Text = comboBox1.Text + " \"" + textBox9.Text + "\" \"" + sourceFile + "\"";
                            filename = "\"" + textBox9.Text + "\" \"" + sourceFile + "\"";
                            //Process.Start("explorer.exe", "/select," + filename);
                            //Process.Start("cmd.exe", "/c start " + filename);
                            //Process.Start(filename);
                            //File.Move(sourceFile, destinationFile);
                            Process.Start("transformer", filename);
                        }
                   
                    }
            
                }
            this.Text = "Send to PAM";
            this.textBox5.Text = "Fix";
        
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            button6.BackColor = Color.Red;
            button6.ForeColor = Color.White;

        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            //enumerateFilters(filenamepart1);


          // MessageBox.Show(filtersText);
        }

        private bool enumerateFilters(string filenamepart1)
        {
            bool b = false;
            string filterString = this.textBox11.Text;
            if (filterString == "" || filterString == null)
            {
                filterString = "\\";
            }

            string[] stringSeparators = new string[] { "|" };
            string[] filters;
            //string filtersText = "";
            

            filters = filterString.Split(stringSeparators, StringSplitOptions.None);
            foreach (string s in filters)
            {
                //filtersText = filtersText + "\n" + s;
                if (filenamepart1.Contains(s))
                {
                    b = true;
                }
            }
            return b;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ReadIniFile(string iniFilename)
        {
            IniFile iniSTP = new IniFile(@iniFilename);
            radioButton1.Checked = Boolean.Parse(iniSTP.IniReadValue("Settings", "UseFileName"));
            radioButton2.Checked = Boolean.Parse(iniSTP.IniReadValue("Settings", "UseJobNumber"));
            radioButton3.Checked = Boolean.Parse(iniSTP.IniReadValue("Settings", "UseFromExt"));
            radioButton4.Checked = Boolean.Parse(iniSTP.IniReadValue("Settings", "UseFolderName"));
            textBox3.Text = iniSTP.IniReadValue("Settings", "fileExt1");
            textBox4.Text = iniSTP.IniReadValue("Settings", "inFile1");
            textBox5.Text = iniSTP.IniReadValue("Settings", "inFile2");
            textBox6.Text = iniSTP.IniReadValue("Settings", "outFile1");
            textBox7.Text = iniSTP.IniReadValue("Settings", "outFile2");
            textBox11.Text = iniSTP.IniReadValue("Settings", "filterString");
            textBox12.Text = iniSTP.IniReadValue("Settings", "configName");
           
        }

        private void WriteIniFile(string iniFilename)
        {
            IniFile iniSTP = new IniFile(@iniFilename);
            iniSTP.IniWriteValue("Settings", "UseFileName", Convert.ToString(radioButton1.Checked));
            iniSTP.IniWriteValue("Settings", "UseJobNumber", Convert.ToString(radioButton2.Checked));
            iniSTP.IniWriteValue("Settings", "UseFromExt", Convert.ToString(radioButton3.Checked));
            iniSTP.IniWriteValue("Settings", "UseFolderName", Convert.ToString(radioButton4.Checked));
            iniSTP.IniWriteValue("Settings", "fileExt1", textBox3.Text);
            iniSTP.IniWriteValue("Settings", "inFile1", textBox4.Text);
            iniSTP.IniWriteValue("Settings", "inFile2", textBox5.Text);
            iniSTP.IniWriteValue("Settings", "outFile1", textBox6.Text);
            iniSTP.IniWriteValue("Settings", "outFile2", textBox7.Text);
            iniSTP.IniWriteValue("Settings", "filterString", textBox11.Text);
            iniSTP.IniWriteValue("Settings", "configName", textBox12.Text);


        }

        private void button7_Click(object sender, EventArgs e)
        {
            String input = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "ini files (*.ini)|*.ini|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = "%user%\\Desktop";
            dialog.Title = "Select an ini file";
            if (dialog.ShowDialog() == DialogResult.OK)
                input = dialog.FileName;
            if (input == String.Empty)
                return; 

            ReadIniFile(input);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            String output = string.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"%user%\\Desktop";
            saveFileDialog1.Title = "Save Configuration File";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "ini";
            saveFileDialog1.Filter = "Configuration files (*.ini)|*.ini|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                output = saveFileDialog1.FileName;
            }




            WriteIniFile(output);
        }


    }
}
