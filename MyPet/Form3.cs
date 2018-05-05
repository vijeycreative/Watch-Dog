using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace MyPet
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select service name to edit", "Infomation");
            }
            else
            {
                Form2 aa = new Form2();
                bool IsEdit = true;
                aa.IsItEdit(this.listBox1.SelectedItem.ToString(), IsEdit);
                aa.ShowDialog();
                this.Close();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string filename = "C:/MyPet/Config/MyPetConfig.xml";
            {
              try
              { 
                
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceStatus")[i];
                    if (cl.InnerText == "False")
                    {
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];
                        this.listBox1.Items.Add(cl1.InnerText);
                    }
                }
                rfile.Close();

                if (this.listBox1.Items.Count == 0)
                {
                    MessageBox.Show("There is no service available to edit.");
                }

              }
              catch (System.IO.FileNotFoundException)
              {
                  MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
              }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



            

        
    }
}
