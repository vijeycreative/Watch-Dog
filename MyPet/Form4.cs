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
    public partial class Form4 : Form
    {
        public string ServiceNameFromForm4 = "Cancelled";
        public Form4()

        {
           
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
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
                            XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];
                            this.listBox1.Items.Add(cl2.InnerText);
                        }

                    }
                    rfile.Close();

                    if (this.listBox1.Items.Count == 0)
                    {
                        MessageBox.Show("There is no service available to start");
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
                }
   
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Service to Start", "Infomation");
            }
            else
            {
                
              ServiceNameFromForm4 = listBox1.SelectedItem.ToString();
              this.Hide();

            }
        }

    }
}
