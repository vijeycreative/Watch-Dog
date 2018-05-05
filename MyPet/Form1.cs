#region Using System
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Globalization;
using System.Management;
using Microsoft.Win32;
using System.Threading;

#endregion

namespace MyPet
{
    public partial class Form1 : Form
    {   
        #region variables
        private bool ServiceStatus;
        
        private string filename = "C:/MyPet/Config/MyPetConfig.xml";
        public static string filename1;
        private string filename3 = "C:/MyPet/AppSettings/MyPetSettings.xml";
        private int NumberOfServices;
        private string servicename;
        private string foldertomonitor;
        private string includesubfolder;
        private string ServiceNameF4toF1;
        private string ServiceNameF5toF1;
        //Variable for remove service name.
        private string RemoveServiceName;
        //Variable for timer1.
        private bool MinusSign1;
        int TotalTimeIntervalInSecTimer1;
        string date;
        //Varibale for report file location.
        private string numberofhours;
        public static string folderlocation;
        //Varibale for Sending Email.
        private string emailaddress1;
        private string emailaddress2;
        private string emailaddress3;
        private string emailaddress4;
        private string fromemailaddress;
        private string hashpw;
        private string smtpserver;
        private string smtpserverport;
        //Variable for mail monitoring
        private bool MailStatus = false;
        private int CreatedNumber =0;
        private int DeletedNumber =0;
        private int RenamedNumber =0;
        //Variable for Actions
        private string escreate;
        private string esdelete;
        private string esrename;
        private string esmove;
        //Varibale for email time setting.
        private string emailtimesetting;
        private bool Timer234MinusSign;
        int TotalTimeIntervalInSecTimer234;
        //Variable for Username and computer name.
        private string PCName;
        private string PCUserName;
        //Variable for ShowGridStatus
        //Autostart
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        // varibale for fileprocessor
        static FileProcessor processor;
        //Variable for auto service startup
        private bool AutoServiceStartupStatus;
        //public static event EventHandler Idle;

        //Event
     

        
        #endregion

        public Form1()
        {
            InitializeComponent();
            CreateMyPetSettingsXML();
            CheckAutoServiceStartup();          
            if (AutoServiceStartupStatus == true)
            {
                CreateFileName();
            }
            if (AutoServiceStartupStatus == true)
            {
                CheckIsCurrent();
                if (servicename == null)
                { }
                else
                {
                    StartServiceAfterStartup();
                    
                }
            }
        }

        #region EventHandlers
        //private void OnApplicationIdle(object sender, EventArgs e)
        //{
         //   if (ShowGridStatus ==true)
            //{
              //  ShowInDataGridView();
                //ShowGridStatus = false;
            //}

        //}
        private void configureServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 a = new Form2();
            bool IsConfig = true;
            a.IsItConfig(IsConfig);
            a.ShowDialog();
        }

        private void startMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 a = new Form4();
            a.ShowDialog();
            ServiceNameF4toF1 = a.ServiceNameFromForm4;
            a.Close();
            if (ServiceNameF4toF1 == "Cancelled")
            {
            }
            else
            {
                if (NumberOfServices == 1)
                {
                    MessageBox.Show("Can not start new service. My pet already is monitoring a folder");
                }
                else
                {
                    NumberOfServices = NumberOfServices + 1;
                    servicename = ServiceNameF4toF1;
                    FindServiceInformation();
                    
                    if (includesubfolder == "True")
                    {
                        Watcher1.IncludeSubdirectories = true;
                    }
                    if (includesubfolder == "False")
                    {
                        Watcher1.IncludeSubdirectories = false;
                    }

                    Watcher1.Path = foldertomonitor;
                    Watcher1.EnableRaisingEvents = true;
                    UpdateServiceStatusTrue();
                    CreateFileName();
                    CreateNewXML();
                    AddDataToXML("My pet started monitoring the folder", Watcher1.Path, DateTime.Now.ToString(), "-", "-");
                    AddDataToXML("My pet created new file for logging", filename1.ToString(), DateTime.Now.ToString(), "-", "-");
                    ShowInDataGridView();
                    processor = new FileProcessor();
                    this.label6.Text = servicename;
                    this.label7.Text = foldertomonitor;
                    this.label8.Text = ServiceStatus.ToString();

                    //Code here for timer.

                    timer1.Enabled = true;
                    TimeDiff();
                    if (MinusSign1 == false)
                    {
                        timer1.Interval = ((24 * 60 * 60 * 1000) - (TotalTimeIntervalInSecTimer1 * 1000));
                    }
                    else
                    {
                        timer1.Interval = TotalTimeIntervalInSecTimer1 * 1000;
                    }
                }

            }
        }

        private void stopMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ServiceStatus == true)
            {
                Watcher1.EnableRaisingEvents.Equals(false);
                UpdateServiceStatusFalse();
                NumberOfServices = NumberOfServices - 1;
                AddDataToXML("My pet Stopped monitoring the folder", Watcher1.Path, DateTime.Now.ToString(), "-", "-");
                ShowInDataGridView();
                this.label6.Text = servicename;
                this.label7.Text = foldertomonitor;
                this.label8.Text = ServiceStatus.ToString();
            }
            else
            {
                MessageBox.Show("My pet is not monitoring any folder at this moment");
            }

            //Code here for timer.

            timer1.Enabled = false;
        }

        #endregion


      private void Watcher1_Created(object sender, FileSystemEventArgs e)
       {
          if (ServiceStatus == true)
           {
               //Read_Current_Session();
               if (escreate == "True")
                { 
                   if (MailStatus == false)
                   {
                       CreatedNumber = CreatedNumber + 1;
                       MailStatus = true;
                       timer2.Enabled = true;
                       timer234Interval();
                   }
                   else
                   {
                       CreatedNumber = CreatedNumber + 1;
                   }
                }
              
            }
          
          processor.QueueInput(e.ChangeType.ToString(), e.FullPath, DateTime.Now.ToString());
      }

        private void Watcher1_Deleted(object sender, FileSystemEventArgs e)
       {
            if (ServiceStatus == true)
          {
                //Read_Current_Session();
               if (esdelete == "True")
               {
                   if (MailStatus == false)
                   {
                       DeletedNumber = DeletedNumber + 1;
                       MailStatus = true;
                       timer2.Enabled = true;
                       timer234Interval();
                   }
                   else
                   {
                       DeletedNumber = DeletedNumber + 1;
                   }
               }
            }
         processor.QueueInput(e.ChangeType.ToString(), e.FullPath, DateTime.Now.ToString());
         //ShowGridStatus = true;
         
       }

       private void Watcher1_Renamed(object sender, RenamedEventArgs e)
        {
           if (ServiceStatus == true)
           {
               // Read_Current_Session();
               if (esrename == "True")
                {
                    if (MailStatus == false)
                    {
                        RenamedNumber = RenamedNumber + 1;
                        MailStatus = true;
                        timer2.Enabled = true;
                        timer234Interval();
                    }
                    else
                    {
                        RenamedNumber = RenamedNumber + 1;
                    }
               }
           }
           processor.QueueInput(e.ChangeType.ToString(), e.FullPath, DateTime.Now.ToString());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.ShowDialog();
        }

        private void editServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 a = new Form3();
            a.ShowDialog();
        }

        public void ReadMyPetConfig()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];
                }
                rfile.Close();
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }
        }

        public void CheckServiceStatus()
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
                        ServiceStatus = false;
                    }
                    if (cl.InnerText == "True")
                    {
                        ServiceStatus = true;
                    }
                }
                rfile.Close();
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }
        }

        public void FindServiceInformation()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];

                    if (cl.InnerText == servicename)
                    {
                        XmlElement cl17 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                        emailaddress1 = cl17.InnerText;
                        XmlElement cl14 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                        emailaddress2 = cl14.InnerText;
                        XmlElement cl15 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                        emailaddress3 = cl15.InnerText;
                        XmlElement cl16 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                        emailaddress4 = cl16.InnerText;
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("FolderToMonitor")[i];
                        foldertomonitor = cl1.InnerText;
                        XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("IncludeSubFolder")[i];
                        includesubfolder = cl2.InnerText;
                        XmlElement cl3 = (XmlElement)xdoc.GetElementsByTagName("NumberOfHours")[i];
                        numberofhours = cl3.InnerText;
                        XmlElement cl4 = (XmlElement)xdoc.GetElementsByTagName("FolderLocation")[i];
                        folderlocation = cl4.InnerText;
                        XmlElement cl6 = (XmlElement)xdoc.GetElementsByTagName("ESCreate")[i];
                        escreate = cl6.InnerText;
                        XmlElement cl7 = (XmlElement)xdoc.GetElementsByTagName("ESDelete")[i];
                        esdelete = cl7.InnerText;
                        XmlElement cl8 = (XmlElement)xdoc.GetElementsByTagName("ESRename")[i];
                        esrename = cl8.InnerText;
                        XmlElement cl9 = (XmlElement)xdoc.GetElementsByTagName("ESMove")[i];
                        esmove = cl9.InnerText;
                        XmlElement cl10 = (XmlElement)xdoc.GetElementsByTagName("FromEmailAddress")[i];
                        fromemailaddress = cl10.InnerText;
                        XmlElement cl11 = (XmlElement)xdoc.GetElementsByTagName("HashPw")[i];
                        hashpw = cl11.InnerText;
                        XmlElement cl12 = (XmlElement)xdoc.GetElementsByTagName("SMTPserver")[i];
                        smtpserver = cl12.InnerText;
                        XmlElement cl13 = (XmlElement)xdoc.GetElementsByTagName("SMTPserverport")[i];
                        smtpserverport = cl13.InnerText;
                        XmlElement cl20 = (XmlElement)xdoc.GetElementsByTagName("EmailTimeSetting")[i];
                        emailtimesetting = cl20.InnerText;
                
                    }
                }
                rfile.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }
        }

        public void UpdateServiceStatusTrue()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];

                    if (cl.InnerText == servicename)
                    {
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("ServiceStatus")[i];

                        cl1.InnerText = "True";
                        ServiceStatus = true;
                        break;
                    }
                }
                rfile.Close();
                xdoc.Save(filename);
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }

        }

        public void UpdateServiceStatusFalse()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];

                    if (cl.InnerText == servicename)
                    {
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("ServiceStatus")[i];
                        cl1.InnerText = "False";
                        ServiceStatus = false;
                        XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("IsCurrent")[i];
                        cl2.InnerText = "Yes";
                        break;
                    }

                }
                rfile.Close();
                xdoc.Save(filename);
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }
        }

        private void deleteServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 a = new Form5();
            a.ShowDialog();
            ServiceNameF5toF1 = a.ServiceNameFromForm5;
            a.Close();
            if (ServiceNameF5toF1 == "Cancelled")
            {
            }
            else
            {
                RemoveServiceName = ServiceNameF5toF1;
                RemoveItem();
                MessageBox.Show("\"" + RemoveServiceName + "\"" + " Service Deleted");
            }
        }

        public void RemoveItem()
        {
            try
            {
                FileStream rfile = new FileStream(filename, FileMode.Open);
                XmlDocument tdoc = new XmlDocument();
                tdoc.Load(rfile);
                XmlNode rootConfig = tdoc.SelectSingleNode("//Configuration");
                XmlNodeList nodes = rootConfig.SelectNodes("Service");

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].SelectSingleNode("ServiceName").InnerText == RemoveServiceName)
                    {
                        rootConfig.RemoveChild(nodes[i]);
                        break;
                    }
                }
                rfile.Close();
                tdoc.Save(filename);
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }

        }

        public void CreateNewXML()
        {
            XmlDocument xmldoc;
            XmlNode xmlnode;
            XmlElement xmlelem;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(filename1);
                }
                catch (System.IO.FileNotFoundException)
                {

                    xmldoc = new XmlDocument();
                    //let's add the XML declaration section
                    xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                    xmldoc.AppendChild(xmlnode);

                    //let's add the root element
                    xmlelem = xmldoc.CreateElement("", "Report", "");
                    xmldoc.AppendChild(xmlelem);
                    XmlProcessingInstruction pi = xmldoc.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"C:\\Mypet\\LogFiles\\MyReport.xsl\"");
                    xmldoc.InsertBefore(pi, xmlelem);

                    //let's try to save the XML document in a file: C:\pavel.xml
                    try
                    {
                        xmldoc.Save(filename1);
                    }
                    catch (Exception e)
                    {
                        WriteError(e.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }
        }


        public void WriteError(string str)
        {
            MessageBox.Show(str);
        }


        public void AddDataToXML(string action, string folderfilename, string dateandtime, string comname, string username)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                FileStream lfile = new FileStream(filename1, FileMode.Open);
                xd.Load(lfile);
                XmlElement PR = xd.CreateElement("PartReport");

                XmlElement A = xd.CreateElement("Action");
                XmlText Atext = xd.CreateTextNode(action);
                A.AppendChild(Atext);

                XmlElement FF = xd.CreateElement("FolderFileName");
                XmlText FFtext = xd.CreateTextNode(folderfilename);
                FF.AppendChild(FFtext);

                XmlElement DT = xd.CreateElement("DateAndTime");
                XmlText DTtext = xd.CreateTextNode(dateandtime);
                DT.AppendChild(DTtext);

                XmlElement CN = xd.CreateElement("ComputerName");
                XmlText CNtext = xd.CreateTextNode(comname);
                CN.AppendChild(CNtext);

                XmlElement US = xd.CreateElement("UserName");
                XmlText UStext = xd.CreateTextNode(username);
                US.AppendChild(UStext);

                xd.DocumentElement.AppendChild(PR);
                PR.AppendChild(A);
                PR.AppendChild(FF);
                PR.AppendChild(DT);
                PR.AppendChild(CN);
                PR.AppendChild(US);

                lfile.Close();
                xd.Save(filename1);
            }
            catch
            {
                MessageBox.Show("\".....AddDatatoxmlForm1-Mypet_File_Folder_Monitoring_Report.xml\" not available");
            }
        }

        private void ShowInDataGridView()
        {
            try
            {
                dataGridView1.Rows.Clear();

                XmlDocument xdoc = new XmlDocument();
                FileStream rfile = new FileStream(filename1, FileMode.Open);
                xdoc.Load(rfile);
                XmlNodeList xmlPartReport = xdoc.GetElementsByTagName("PartReport");
                int m = xmlPartReport.Count - 1;
                int n = xmlPartReport.Count - 35;

                for (int i = m; i > n; i--)
                {
                    if (i < 0)
                    {
                        break;
                    }
                    else
                    {
                        String[] data = new String[5];
                        XmlElement cl0 = (XmlElement)xdoc.GetElementsByTagName("Action")[i];
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("FolderFileName")[i];
                        XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("DateAndTime")[i];
                        XmlElement cl3 = (XmlElement)xdoc.GetElementsByTagName("ComputerName")[i];
                        XmlElement cl4 = (XmlElement)xdoc.GetElementsByTagName("UserName")[i];
                        data[0] = cl0.InnerText;
                        data[1] = cl1.InnerText;
                        data[2] = cl2.InnerText;
                        data[3] = cl3.InnerText;
                        data[4] = cl4.InnerText;
                        
                        
                        dataGridView1.Rows.Add(data[0], data[1], data[2], data[3], data[4]);
                    }
                }

                rfile.Close();
            }
            catch
            {
                MessageBox.Show("\".....Showindatagridviewform1-Mypet_File_Folder_Monitoring_Report.xml\" not available");
            }

        }

        //Methods starts here for timer.
        public void TimeDiff()
        {
            //Code here to calculate the time difference between the application start and file creation.
            string CurTime = DateTime.Now.ToString("HH:mm:ss");
            DateTime dt1 = DateTime.ParseExact(CurTime, "HH:mm:ss", new DateTimeFormatInfo());
            string ConfigTime = "00:00:00";
            if (numberofhours == "9")
            {
                ConfigTime = "09:00:00";
            }
            if (numberofhours == "12")
            {
                ConfigTime = "12:00:00";
            }
            if (numberofhours == "3")
            {
                ConfigTime = "15:00:00";
            }
            if (numberofhours == "6")
            {
                ConfigTime = "18:00:00";
            }
            DateTime dt2 = DateTime.ParseExact(ConfigTime, "HH:mm:ss", new DateTimeFormatInfo());
            TimeSpan ts1 = dt1.Subtract(dt2);

            string IntervalTime = ts1.ToString();
            string[] intervaltime = IntervalTime.Split(':');
            string intervaltime3;

            string MinusSign = intervaltime[0].Substring(0, 1);
            if (MinusSign == "-")
            {
                //MessageBox.Show("string contains minus sign");
                intervaltime3 = intervaltime[0].Substring(1, 2);
                MinusSign1 = true;
            }
            else
            {
                //MessageBox.Show("string does not contain minus sign");
                intervaltime3 = intervaltime[0];
                MinusSign1 = false;

            }


            TotalTimeIntervalInSecTimer1 = (int.Parse(intervaltime3) * 60 * 60) + (int.Parse(intervaltime[1]) * 60) + (int.Parse(intervaltime[2]));

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CreateFileName();
            CreateNewXML();
            timer1.Interval = ((24 * 60 * 60 * 1000));
            AddDataToXML("My pet created new file for logging", filename1.ToString(), DateTime.Now.ToString(), "-", "-");
            DeleteOldfile();
        }





        public void CreateFileName()
        {

            DateTime dateTime = DateTime.Now;
            date = dateTime.ToString("s");
            date = date.Replace(":", "-");
            filename1 = folderlocation + "\\" + date + " Mypet_File_Folder_Monitoring_Report.xml";
        }

        public void DeleteOldfile()
        {
            DirectoryInfo dir = new DirectoryInfo(folderlocation);
            if (dir.Exists)
            {
                if (dir.GetFiles().Length > 0)
                {
                    FileInfo[] fi_array = dir.GetFiles();
                    foreach (FileInfo fi in fi_array)
                    {
                        string s = fi.Name;
                        DateTime DateTime = DateTime.Now;
                        string filedate = s.Remove(10);
                        DateTime FileDate = Convert.ToDateTime(filedate);
                        TimeSpan ts = FileDate - DateTime;
                        int days = ts.Days;

                        if (days < -31)
                        {
                            FileInfo MyFile = new FileInfo(folderlocation + "\\" + s);
                            MyFile.Delete();

                            AddDataToXML("Old mypet report file had been deleted", (s + " on "), DateTime.Now.ToString(), "-", "-");
                        }
                    }


                }
            }

        }

        //
        //
        private void Form1_FormClosing(object sender, EventArgs e)
        {
            DialogResult result;
            if (ServiceStatus == true)
            {
                result = MessageBox.Show("Folder monitoring will be stoped.", "Exit My Pet", MessageBoxButtons.OK);

                if (result == DialogResult.OK)
                {
                    UpdateServiceStatusFalse();
                    NumberOfServices = NumberOfServices - 1;
                    AddDataToXML("My pet Stopped monitoring the folder", Watcher1.Path, DateTime.Now.ToString(), "-", "-");


                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // Use this since we are a WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Use this since we are a console app
                        System.Environment.Exit(1);
                    }
                }
            }
            else
            {
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // Use this since we are a WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Use this since we are a console app
                    System.Environment.Exit(1);
                }

            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (ServiceStatus == true)
            {
                result = MessageBox.Show("Folder monitoring will be stoped.", "Exit My Pet", MessageBoxButtons.OK);

                if (result == DialogResult.OK)
                {
                    UpdateServiceStatusFalse();
                    NumberOfServices = NumberOfServices - 1;
                    AddDataToXML("My pet Stopped monitoring the folder", Watcher1.Path, DateTime.Now.ToString(), "-", "-");


                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // Use this since we are a WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Use this since we are a console app
                        System.Environment.Exit(1);
                    }
                }
            }
            else
            {
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // Use this since we are a WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Use this since we are a console app
                    System.Environment.Exit(1);
                }

            }

        }


        //


        
        private void Read_Current_Session()
        {
            string RemoteComName = null;
            string RemoteUserName = null;
            string LocalPCname = Dns.GetHostName();
            string LocalUserName = Environment.UserName.ToString();
            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_ServerConnection");

                // Read the object 


                foreach (ManagementObject queryObj in searcher.Get())
                {
                    RemoteComName = queryObj["ComputerName"].ToString();
                    RemoteUserName = queryObj["UserName"].ToString();
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

            if (RemoteComName == null)
            {
                PCName = LocalPCname;
                PCUserName = LocalUserName;
            }
            else
            {
                PCName = RemoteComName;
                PCUserName = RemoteUserName;
            }
        }

        private void autoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 a = new Form6();
            a.ShowDialog();
        }

        //
       private void StartServiceAfterStartup()
        {

                if (NumberOfServices == 1)
                {
                    MessageBox.Show("Can not start new service. My pet already is monitoring a folder");
                }
                else
                {
                    NumberOfServices = NumberOfServices + 1;
                    CheckIsCurrent();
                    FindServiceInformation();
                    
                    if (includesubfolder == "True")
                    {

                        Watcher1.IncludeSubdirectories.Equals(true);
                    }
                    Watcher1.Path = foldertomonitor;
                    Watcher1.EnableRaisingEvents = true;
                    UpdateServiceStatusTrue();
                    CreateFileName();
                    CreateNewXML();
                    AddDataToXML("My pet started monitoring the folder", Watcher1.Path, DateTime.Now.ToString(), "-", "-");
                    AddDataToXML("My pet created new file for logging", filename1.ToString(), DateTime.Now.ToString(), "-", "-");
                    ShowInDataGridView();
                    processor = new FileProcessor();
                    this.label6.Text = servicename;
                    this.label7.Text = foldertomonitor;
                    this.label8.Text = ServiceStatus.ToString();

                    //Code here for timer.

                    timer1.Enabled = true;
                    TimeDiff();
                    if (MinusSign1 == false)
                    {
                        timer1.Interval = ((24 * 60 * 60 * 1000) - (TotalTimeIntervalInSecTimer1 * 1000));
                    }
                    else
                    {
                        timer1.Interval = TotalTimeIntervalInSecTimer1 * 1000;
                    }
                }
        }
    //
       private void CheckIsCurrent()
       {
           try
           {
               XmlDocument xdoc = new XmlDocument();
               FileStream rfile = new FileStream(filename, FileMode.Open);
               xdoc.Load(rfile);
               XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
               for (int i = 0; i < list.Count; i++)
               {
                   XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("IsCurrent")[i];

                   if (cl.InnerText == "Yes")
                   {
                       XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];
                       servicename = cl1.InnerText;
                       break;
                   }

               }
               rfile.Close();
           }
           catch
           {
               servicename = null;
           }
       }

//
       public void WriteMyPetSettingsXML()
       {
           try
           {
               XmlDocument xd = new XmlDocument();
               FileStream lfile = new FileStream(filename3, FileMode.Open);
               xd.Load(lfile);
               XmlElement Se = xd.CreateElement("AutoStart");

               XmlElement SN = xd.CreateElement("AutoStartService");
               XmlText SNtext = xd.CreateTextNode("False");
               SN.AppendChild(SNtext);

               Se.AppendChild(SN);

               xd.DocumentElement.AppendChild(Se);

               lfile.Close();
               xd.Save(filename);
           }
           catch
           {
               MessageBox.Show("\"C:/MyPet/AppSettings/MyPetSettings.xml\" not available");
           }
       }
//
       public void CreateMyPetSettingsXML()
       {
           try
           {
               XmlDocument xmlDoc = new XmlDocument();

               try
               {
                   xmlDoc.Load(filename3);
               }
               catch (System.IO.FileNotFoundException)
               {
                   //if file is not found, create a new xml file
                   XmlTextWriter xtw;
                   xtw = new XmlTextWriter(filename3, Encoding.UTF8);
                   xtw.WriteStartDocument();
                   xtw.WriteStartElement("MyPetSettings");
                   xtw.WriteEndElement();
                   xtw.Close();
                   //Add elements
                   XmlDocument xd = new XmlDocument();
                   FileStream lfile = new FileStream(filename3, FileMode.Open);
                   xd.Load(lfile);
                   XmlElement Se = xd.CreateElement("AutoStart");

                   XmlElement SN = xd.CreateElement("AutoStartService");
                   XmlText SNtext = xd.CreateTextNode("False");
                   SN.AppendChild(SNtext);

                   Se.AppendChild(SN);

                   xd.DocumentElement.AppendChild(Se);

                   lfile.Close();
                   xd.Save(filename3);

               }
           }
           catch (Exception ex)
           {
               WriteError(ex.ToString());
           }
       }
        //
       private void CheckAutoServiceStartup()
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
                       AutoServiceStartupStatus = true;
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
 
       private void SendMail(String sub, String body)
       {
               try
               {
                   string FromEmailid;
                   string Password;
                   string Toid;
                   string Body;
                   SmtpClient smtp = new SmtpClient();
                   MailMessage mail = new MailMessage();
                   smtp.Host = smtpserver;
                   FromEmailid = fromemailaddress;
                   Password = hashpw;
                   smtp.Credentials = new System.Net.NetworkCredential(FromEmailid, Password);
                        if (emailaddress2 == "Nothing")
                        {
                        Toid = emailaddress1;
                        }
                        else
                        {
                            if (emailaddress3 == "Nothing")
                            {
                            Toid = (emailaddress1 + ";" + emailaddress2);
                            }
                            else
                            {
                                if (emailaddress4 =="Nothing")
                                {
                                Toid = (emailaddress1 + ";" + emailaddress2 + ";" + emailaddress3);
                                }
                                else
                                {
                                Toid = (emailaddress1 + ";" + emailaddress2 + ";" + emailaddress3+ ";" + emailaddress4);
                                }
                            }
                        }

                   mail.To.Add(Toid);
                   mail.Subject = sub;
                   mail.From = new MailAddress(FromEmailid);
                   Body = body;
                   mail.Body = Body;
                   smtp.Port = int.Parse(smtpserverport);
                   smtp.EnableSsl = true;
                   smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                   //MessageBox.Show("The message being sending. Please wait for result.");
                   smtp.Send(mail);
                   //MessageBox.Show("Email Send successfully.");
               }
               catch (SmtpException ex)
               {
                   //MessageBox.Show("Email Send unsuccessful.");
                   AddDataToXML("SendEMail: " + ex.Message, "Check the email credentials or network connection to email server", "-", "-", "-");
                   ShowInDataGridView();
               }

           }



       //


       private void timer2_Tick(object sender, EventArgs e)
       {
           string LocalSub = "";
           string LocalBody = "";
           if(MailStatus = true &(CreatedNumber>0) &(DeletedNumber>0) &(RenamedNumber>0))
           {
           LocalSub = " File Created/Deleted/Renamed-Mypet Monitoring Report";
           LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files created: " + CreatedNumber + "Number of files deleted: " + DeletedNumber +  "Number of files renamed: " + RenamedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";
           SendMail(LocalSub, LocalBody);
           LocalBody = "";
           LocalSub = "";
           CreatedNumber = 0;
           DeletedNumber = 0;
           RenamedNumber = 0;
           MailStatus = false;
           timer2.Enabled = false;
           }

           if (MailStatus = true & (CreatedNumber > 0) & (DeletedNumber > 0))
           {
           LocalSub = " File Created/Deleted-Mypet Monitoring Report";
           LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files created: " + CreatedNumber + "Number of files deleted: " + DeletedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";
           SendMail(LocalSub, LocalBody);
           LocalBody = "";
           LocalSub = "";
           CreatedNumber = 0;
           DeletedNumber = 0;
           RenamedNumber = 0;
           MailStatus = false;
           timer2.Enabled = false;
           }
           if (MailStatus = true & (CreatedNumber > 0) & (RenamedNumber > 0))
           {
           LocalSub = " File Created/Renamed-Mypet Monitoring Report";
           LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files created: " + CreatedNumber +  "Number of files renamed: " + RenamedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";
           SendMail(LocalSub, LocalBody);
           LocalBody = "";
           LocalSub = "";
           CreatedNumber = 0;
           DeletedNumber = 0;
           RenamedNumber = 0;
           MailStatus = false;
           timer2.Enabled = false;
           }
           if (MailStatus = true & (DeletedNumber > 0) & (RenamedNumber > 0))
           {
           LocalSub = " File Deleted/Renamed-Mypet Monitoring Report";
           LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files deleted: " + DeletedNumber + "Number of files renamed: " + RenamedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";
           SendMail(LocalSub, LocalBody);
           LocalBody = "";
           LocalSub = "";
           CreatedNumber = 0;
           DeletedNumber = 0;
           RenamedNumber = 0;
           MailStatus = false;
           timer2.Enabled = false;
           }
           if (MailStatus = true & CreatedNumber > 0)
           {
               LocalSub = " File Created-Mypet Monitoring Report";
               LocalBody = "Hi\n\n" +"The following activities were happened in the folder.....\n" + "Number of files created: " +CreatedNumber +  "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";

               SendMail(LocalSub, LocalBody);
               LocalBody = "";
               LocalSub = "";
               CreatedNumber = 0;
               DeletedNumber = 0;
               RenamedNumber = 0;
               MailStatus = false;
               timer2.Enabled = false;
           }
           if (MailStatus = true & DeletedNumber > 0)
           {
               LocalSub = " File Deleted-Mypet Monitoring Report";
               LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files deleted: " + DeletedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nRegards \n\nMyPet Email Notifier.";
               SendMail(LocalSub, LocalBody);
               LocalBody = "";
               LocalSub = "";
               CreatedNumber = 0;
               DeletedNumber = 0;
               RenamedNumber = 0;
               MailStatus = false;
               timer2.Enabled = false;
           }
           if (MailStatus = true & RenamedNumber > 0)
           {
           LocalSub = " File Renamed-Mypet Monitoring Report";
           LocalBody = "Hi\n\n" + "The following activities were happened in the folder.....\n" + "Number of files renamed: " + RenamedNumber + "\nRefer particular log file for more information. This is an auto generated email notification. Please do not respond. \n\nMyPet Email Notifier.";
           SendMail(LocalSub, LocalBody);
           LocalBody = "";
           LocalSub = "";
           CreatedNumber = 0;
           DeletedNumber = 0;
           RenamedNumber = 0;
           MailStatus = false;
           timer2.Enabled = false;
           }

       }
        // 
        private  void timer234Interval()
        {
            //Code here to calculate the time difference between the application start and file creation.
            string CurTime = DateTime.Now.ToString("HH:mm:ss");
            DateTime dt1 = DateTime.ParseExact(CurTime, "HH:mm:ss", new DateTimeFormatInfo());
            string ConfigTime = "00:00:00";
            if (emailtimesetting == "Immediate")
            {
                ConfigTime = "00:02:00";
            }
            if (emailtimesetting == "12PM")
            {
                ConfigTime = "12:00:00";
            }
            DateTime dt2 = DateTime.ParseExact(ConfigTime, "HH:mm:ss", new DateTimeFormatInfo());
            TimeSpan ts1 = dt1.Subtract(dt2);

            string IntervalTime = ts1.ToString();
            string[] intervaltime = IntervalTime.Split(':');
            string intervaltime3;

            string MinusSign = intervaltime[0].Substring(0, 1);
            if (MinusSign == "-")
            {
                //MessageBox.Show("string contains minus sign");
                intervaltime3 = intervaltime[0].Substring(1, 2);
                Timer234MinusSign = true;
            }
            else
            {
                //MessageBox.Show("string does not contain minus sign");
                intervaltime3 = intervaltime[0];
                Timer234MinusSign = false;
            }
            TotalTimeIntervalInSecTimer234 = (int.Parse(intervaltime3) * 60 * 60) + (int.Parse(intervaltime[1]) * 60) + (int.Parse(intervaltime[2]));
            timer2.Interval = TotalTimeIntervalInSecTimer234;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (ServiceStatus == true)
            {
                ShowInDataGridView();
            }
            else
            {
                MessageBox.Show("My pet is not monitoring any folder at this moment");
            }


            
            
        }







    }

}
