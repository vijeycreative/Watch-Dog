using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml;
using System.IO;

namespace MyPet
{
    public partial class Form6 : Form
    {
        private string filename3 = "C:/MyPet/AppSettings/MyPetSettings.xml";
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public Form6()
        {
            InitializeComponent();
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("MyPet.exe") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                checkBox1.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                checkBox1.Checked = true;
            }
            CheckAutoStartService();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("MyPet.exe", Application.ExecutablePath.ToString());
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("MyPet.exe", false);
            }

                UpdateAutoServiceStatus();
            

                this.Close();

        }


        private void UpdateAutoServiceStatus()
        {
            
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename3, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("AutoStart");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("AutoStartService")[i];
                    if (checkBox2.Checked == true)
                    {
                        cl.InnerText = "True";
                    }
                    else
                    {
                        cl.InnerText = "False";
                    }
                }
                rfile.Close();
                xdoc.Save(filename3);
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/AppSettings/MyPetSettings.xml\" not available");
            }
        }
        //
        private void CheckAutoStartService()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename3, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("AutoStart");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("AutoStartService")[i];
                    if (cl.InnerText == "True")
                    {
                        checkBox2.Checked = true;
                    }
                    if (cl.InnerText == "False")
                    {
                        checkBox2.Checked = false;
                    }
                }
                rfile.Close();
                xdoc.Save(filename3);
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/AppSettings/MyPetSettings.xml\" not available");
            }
        
        }



    }
}
