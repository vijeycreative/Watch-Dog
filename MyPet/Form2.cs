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
using System.Text.RegularExpressions;
using System.Net.Mail;



namespace MyPet
{
    public partial class Form2 : Form
    {

        public int NumberOfEAddress = new int();
        public int NumberOfUpdatEAddress = new int();
        public bool ServiceExist;
        bool IsEditStatus;
        bool IsConfigStatus;
        private string filename = "C:/MyPet/Config/MyPetConfig.xml";
        bool form2validation1 = true;
        bool form2validation2 = true;
        public Form2()
        {
            InitializeComponent();
            textBox2.Text = "C:\\MyPet\\LogFiles";
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
        }


        private void button7_Click(object sender, EventArgs e)
        {

            Form2validation1();

            if (form2validation1 == true & form2validation2 == true)
            {
                ProcessForm2();
            }
            else
            {
                 form2validation1 = true;
                 form2validation2 = true;
                
            }
        }

        private void ProcessForm2()
        {

                if (IsEditStatus == true)
                {

                    UpdateXML();
                }
                if (IsConfigStatus == true)
                {
                    CreateNewXML();
                    CheckServiceName();
                    if (ServiceExist == true)
                    {
                        MessageBox.Show("Service Name " + textBox3.Text + " already exist. Please change the Service Name");
                    }
                    else
                    {
                        WriteXML();

                    }
                }
       
        }

        public void CreateNewXML()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(filename);
                }
                catch (System.IO.FileNotFoundException)
                {
                    //if file is not found, create a new xml file
                    XmlTextWriter xtw;
                    xtw = new XmlTextWriter(filename, Encoding.UTF8);
                    xtw.WriteStartDocument();
                    xtw.WriteStartElement("Configuration");
                    xtw.WriteEndElement();
                    xtw.Close();
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

        public void WriteXML()
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                FileStream lfile = new FileStream(filename, FileMode.Open);
                xd.Load(lfile);
                XmlElement Se = xd.CreateElement("Service");

                XmlElement SN = xd.CreateElement("ServiceName");
                XmlText SNtext = xd.CreateTextNode(this.textBox3.Text);
                SN.AppendChild(SNtext);

                XmlElement FTM = xd.CreateElement("FolderToMonitor");
                XmlText FTMtext = xd.CreateTextNode(this.textBox1.Text);
                FTM.AppendChild(FTMtext);

                XmlElement ISF = xd.CreateElement("IncludeSubFolder");
                XmlText ISFtext = xd.CreateTextNode(this.checkBox1.Checked.ToString());
                ISF.AppendChild(ISFtext);

                XmlElement NH = xd.CreateElement("NumberOfHours");
                if (this.comboBox1.SelectedIndex == 0)
                {
                    XmlText NHtext = xd.CreateTextNode("9");
                    NH.AppendChild(NHtext);
                }
                if (this.comboBox1.SelectedIndex == 1)
                {
                    XmlText NHtext = xd.CreateTextNode("12");
                    NH.AppendChild(NHtext);
                }
                if (this.comboBox1.SelectedIndex == 2)
                {
                    XmlText NHtext = xd.CreateTextNode("3");
                    NH.AppendChild(NHtext);
                }
                if (this.comboBox1.SelectedIndex == 3)
                {
                    XmlText NHtext = xd.CreateTextNode("6");
                    NH.AppendChild(NHtext);
                }

                XmlElement FL = xd.CreateElement("FolderLocation");
                XmlText FLtext = xd.CreateTextNode(this.textBox2.Text);
                FL.AppendChild(FLtext);

                XmlElement ESC = xd.CreateElement("ESCreate");
                XmlText ESCtext = xd.CreateTextNode(this.checkBox2.Checked.ToString());
                ESC.AppendChild(ESCtext);

                XmlElement ESD = xd.CreateElement("ESDelete");
                XmlText ESDtext = xd.CreateTextNode(this.checkBox4.Checked.ToString());
                ESD.AppendChild(ESDtext);

                XmlElement ESR = xd.CreateElement("ESRename");
                XmlText ESRtext = xd.CreateTextNode(this.checkBox3.Checked.ToString());
                ESR.AppendChild(ESRtext);

                XmlElement ESM = xd.CreateElement("ESMove");
                XmlText ESMtext = xd.CreateTextNode(this.checkBox5.Checked.ToString());
                ESM.AppendChild(ESMtext);

                NumberOfEAddress = listBox1.Items.Count;

                switch (NumberOfEAddress)
                {
                    case 0:

                        XmlElement EA01 = xd.CreateElement("EMailAddress1");
                        XmlText EA01text = xd.CreateTextNode("Nothing");
                        EA01.AppendChild(EA01text);
                        Se.AppendChild(EA01);

                        XmlElement EA02 = xd.CreateElement("EMailAddress2");
                        XmlText EA02text = xd.CreateTextNode("Nothing");
                        EA02.AppendChild(EA02text);
                        Se.AppendChild(EA02);

                        XmlElement EA03 = xd.CreateElement("EMailAddress3");
                        XmlText EA03text = xd.CreateTextNode("Nothing");
                        EA03.AppendChild(EA03text);
                        Se.AppendChild(EA03);

                        XmlElement EA04 = xd.CreateElement("EMailAddress4");
                        XmlText EA04text = xd.CreateTextNode("Nothing");
                        EA04.AppendChild(EA04text);
                        Se.AppendChild(EA04);
                        break;

                    case 1:

                        listBox1.SetSelected(0, true);
                        XmlElement EA11 = xd.CreateElement("EMailAddress1");
                        XmlText EA11text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA11.AppendChild(EA11text);
                        Se.AppendChild(EA11);

                        XmlElement EA12 = xd.CreateElement("EMailAddress2");
                        XmlText EA12text = xd.CreateTextNode("Nothing");
                        EA12.AppendChild(EA12text);
                        Se.AppendChild(EA12);

                        XmlElement EA13 = xd.CreateElement("EMailAddress3");
                        XmlText EA13text = xd.CreateTextNode("Nothing");
                        EA13.AppendChild(EA13text);
                        Se.AppendChild(EA13);

                        XmlElement EA14 = xd.CreateElement("EMailAddress4");
                        XmlText EA14text = xd.CreateTextNode("Nothing");
                        EA14.AppendChild(EA14text);
                        Se.AppendChild(EA14);
                        break;

                    case 2:

                        listBox1.SetSelected(0, true);
                        XmlElement EA21 = xd.CreateElement("EMailAddress1");
                        XmlText EA21text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA21.AppendChild(EA21text);
                        Se.AppendChild(EA21);

                        listBox1.SetSelected(1, true);
                        XmlElement EA22 = xd.CreateElement("EMailAddress2");
                        XmlText EA22text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA22.AppendChild(EA22text);
                        Se.AppendChild(EA22);

                        XmlElement EA23 = xd.CreateElement("EMailAddress3");
                        XmlText EA23text = xd.CreateTextNode("Nothing");
                        EA23.AppendChild(EA23text);
                        Se.AppendChild(EA23);

                        XmlElement EA24 = xd.CreateElement("EMailAddress4");
                        XmlText EA24text = xd.CreateTextNode("Nothing");
                        EA24.AppendChild(EA24text);
                        Se.AppendChild(EA24);
                        break;

                    case 3:

                        listBox1.SetSelected(0, true);
                        XmlElement EA31 = xd.CreateElement("EMailAddress1");
                        XmlText EA31text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA31.AppendChild(EA31text);
                        Se.AppendChild(EA31);

                        listBox1.SetSelected(1, true);
                        XmlElement EA32 = xd.CreateElement("EMailAddress2");
                        XmlText EA32text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA32.AppendChild(EA32text);
                        Se.AppendChild(EA32);

                        listBox1.SetSelected(2, true);
                        XmlElement EA33 = xd.CreateElement("EMailAddress3");
                        XmlText EA33text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA33.AppendChild(EA33text);
                        Se.AppendChild(EA33);

                        XmlElement EA34 = xd.CreateElement("EMailAddress4");
                        XmlText EA34text = xd.CreateTextNode("Nothing");
                        EA34.AppendChild(EA34text);
                        Se.AppendChild(EA34);
                        break;

                    case 4:

                        listBox1.SetSelected(0, true);
                        XmlElement EA41 = xd.CreateElement("EMailAddress1");
                        XmlText EA41text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA41.AppendChild(EA41text);
                        Se.AppendChild(EA41);

                        listBox1.SetSelected(1, true);
                        XmlElement EA42 = xd.CreateElement("EMailAddress2");
                        XmlText EA42text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA42.AppendChild(EA42text);
                        Se.AppendChild(EA42);

                        listBox1.SetSelected(2, true);
                        XmlElement EA43 = xd.CreateElement("EMailAddress3");
                        XmlText EA43text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA43.AppendChild(EA43text);
                        Se.AppendChild(EA43);

                        listBox1.SetSelected(3, true);
                        XmlElement EA44 = xd.CreateElement("EMailAddress4");
                        XmlText EA44text = xd.CreateTextNode(this.listBox1.SelectedItem.ToString());
                        EA44.AppendChild(EA44text);
                        Se.AppendChild(EA44);
                        break;

                }

                XmlElement FromEmailAddress = xd.CreateElement("FromEmailAddress");
                XmlText FromEmailAddressText = xd.CreateTextNode(this.textBox4.Text);
                FromEmailAddress.AppendChild(FromEmailAddressText);

                XmlElement HashPw = xd.CreateElement("HashPw");
                XmlText HashPwText = xd.CreateTextNode(this.textBox5.Text);
                HashPw.AppendChild(HashPwText);

                XmlElement SMTPser = xd.CreateElement("SMTPserver");
                XmlText SMTPsertext = xd.CreateTextNode(this.textBox7.Text);
                SMTPser.AppendChild(SMTPsertext);

                XmlElement SMTPserport = xd.CreateElement("SMTPserverport");
                XmlText SMTPserporttext = xd.CreateTextNode(this.textBox8.Text);
                SMTPserport.AppendChild(SMTPserporttext);

                XmlElement EmailTimeSetting = xd.CreateElement("EmailTimeSetting");
                if (radioButton1.Checked == true)
                {
                    XmlText EmailTimeSettingtext1 = xd.CreateTextNode("Immediate");
                    EmailTimeSetting.AppendChild(EmailTimeSettingtext1);
                }
                else
                {
                    if (radioButton2.Checked == true)
                    {
                        XmlText EmailTimeSettingtext2 = xd.CreateTextNode("12PM");
                        EmailTimeSetting.AppendChild(EmailTimeSettingtext2);
                    }
                    else
                    {
                            XmlText EmailTimeSettingtext4 = xd.CreateTextNode("Nothing");
                            EmailTimeSetting.AppendChild(EmailTimeSettingtext4);
                    }
                }
              
                XmlElement ServiceStatus = xd.CreateElement("ServiceStatus");
                XmlText ServiceStatustext = xd.CreateTextNode("False");
                ServiceStatus.AppendChild(ServiceStatustext);

                XmlElement IsCurrentService = xd.CreateElement("IsCurrent");
                XmlText IsCurrentServicetext = xd.CreateTextNode("False");
                IsCurrentService.AppendChild(IsCurrentServicetext);

                Se.AppendChild(SN);
                Se.AppendChild(FTM);
                Se.AppendChild(ISF);
                Se.AppendChild(NH);
                Se.AppendChild(FL);
                Se.AppendChild(ESC);
                Se.AppendChild(ESD);
                Se.AppendChild(ESR);
                Se.AppendChild(ESM);
                Se.AppendChild(FromEmailAddress);
                Se.AppendChild(HashPw);
                Se.AppendChild(SMTPser);
                Se.AppendChild(SMTPserport);
                Se.AppendChild(EmailTimeSetting);
                Se.AppendChild(ServiceStatus);
                Se.AppendChild(IsCurrentService);
                xd.DocumentElement.AppendChild(Se);

                lfile.Close();
                xd.Save(filename);
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }
        }

        public void CheckServiceName()
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

                    if ((cl.InnerText) == this.textBox3.Text)
                    {
                        ServiceExist = true;
                        break;
                    }

                }
                rfile.Close();
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }

        }

        public void IsItEdit(string myservicename, bool IsEdit)
        {
            this.textBox3.Text = myservicename;
            this.textBox3.Enabled = false;
            this.Text = "Edit Service Configuration";
            IsEditStatus = IsEdit;
            AssignValueInForm2();
        }

        public void IsItConfig(bool isconfig)
        {
            IsConfigStatus = isconfig;

        }

        public void AssignValueInForm2()
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

                    if ((cl.InnerText) == this.textBox3.Text)
                    {
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("FolderToMonitor")[i];
                        this.textBox1.Text = cl1.InnerText;

                        XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("IncludeSubFolder")[i];
                        string IncludeFolderCurrent = "True";
                        if (cl2.InnerText == IncludeFolderCurrent)
                        {
                            this.checkBox1.Checked = true;
                        }

                        XmlElement cl3 = (XmlElement)xdoc.GetElementsByTagName("NumberOfHours")[i];
                        string NumberOfHoursCurrent;
                        NumberOfHoursCurrent = cl3.InnerText;
                        if (NumberOfHoursCurrent == "9")
                        {
                            comboBox1.SelectedIndex = 0;
                        }
                        if (NumberOfHoursCurrent == "12")
                        {
                            comboBox1.SelectedIndex = 1;
                        }
                        if (NumberOfHoursCurrent == "3")
                        {
                            comboBox1.SelectedIndex = 2;
                        }
                        if (NumberOfHoursCurrent == "6")
                        {
                            comboBox1.SelectedIndex = 3;
                        }
                        XmlElement cl4 = (XmlElement)xdoc.GetElementsByTagName("FolderLocation")[i];
                        this.textBox2.Text = cl4.InnerText;

                        XmlElement cl5 = (XmlElement)xdoc.GetElementsByTagName("ESCreate")[i];
                        string ESCreateCurrent = "True";
                        if (cl5.InnerText == ESCreateCurrent)
                        {
                            this.checkBox2.Checked = true;
                        }
                        else
                        {
                            this.checkBox2.Checked = false;
                        }

                        XmlElement cl6 = (XmlElement)xdoc.GetElementsByTagName("ESDelete")[i];
                        string ESDeleteCurrent = "True";
                        if (cl6.InnerText == ESDeleteCurrent)
                        {
                            this.checkBox4.Checked = true;
                        }
                        else
                        {
                            this.checkBox4.Checked = false;
                        }

                        XmlElement cl7 = (XmlElement)xdoc.GetElementsByTagName("ESRename")[i];
                        string ESRenameCurrent = "True";
                        if (cl7.InnerText == ESRenameCurrent)
                        {
                            this.checkBox3.Checked = true;
                        }
                        else
                        {
                            this.checkBox3.Checked = false;
                        }

                        XmlElement cl8 = (XmlElement)xdoc.GetElementsByTagName("ESMove")[i];
                        string ESMoveCurrent = "True";
                        if (cl8.InnerText == ESMoveCurrent)
                        {
                            this.checkBox5.Checked = true;
                        }
                        else
                        {
                            this.checkBox5.Checked = false;
                        }

                        NumberOfUpdatEAddress = listBox1.Items.Count;

                        XmlElement cl9 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];

                        if (cl9.InnerText == "Nothing")
                        {
                        }
                        else
                        {
                            this.listBox1.Items.Add(cl9.InnerText);
                        }

                        XmlElement cl10 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                        if (cl10.InnerText == "Nothing")
                        {
                        }
                        else
                        {
                            this.listBox1.Items.Add(cl10.InnerText);
                        }

                        XmlElement cl11 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                        if (cl11.InnerText == "Nothing")
                        {
                        }
                        else
                        {
                            this.listBox1.Items.Add(cl11.InnerText);
                        }
                        XmlElement cl12 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                        if (cl12.InnerText == "Nothing")
                        {
                        }
                        else
                        {
                            this.listBox1.Items.Add(cl12.InnerText);
                        }

                        XmlElement cl13 = (XmlElement)xdoc.GetElementsByTagName("FromEmailAddress")[i];
                        this.textBox4.Text = cl13.InnerText;

                        XmlElement cl14 = (XmlElement)xdoc.GetElementsByTagName("HashPw")[i];
                        this.textBox5.Text = cl14.InnerText;
                        this.textBox6.Text = cl14.InnerText;

                        XmlElement cl15 = (XmlElement)xdoc.GetElementsByTagName("SMTPserver")[i];
                        this.textBox7.Text = cl15.InnerText;

                        XmlElement cl16 = (XmlElement)xdoc.GetElementsByTagName("SMTPserverport")[i];
                        this.textBox8.Text = cl16.InnerText;

                        XmlElement cl20 = (XmlElement)xdoc.GetElementsByTagName("EmailTimeSetting")[i];
                        if (cl20.InnerText == "Immediate")
                        {
                            radioButton1.Checked = true;
                        }
                        if (cl20.InnerText == "12PM")
                        {
                            radioButton2.Checked = true;
                        }
                        break;
                    }


                }
                rfile.Close();
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");

            }

        }

        public void UpdateXML()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                FileStream up = new FileStream(filename, FileMode.Open);
                xdoc.Load(up);
                XmlNodeList list = xdoc.GetElementsByTagName("ServiceName");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)xdoc.GetElementsByTagName("ServiceName")[i];
                    if ((cl.InnerText) == this.textBox3.Text)
                    {
                        XmlElement cl1 = (XmlElement)xdoc.GetElementsByTagName("FolderToMonitor")[i];
                        cl1.InnerText = "";
                        cl1.InnerText = this.textBox1.Text;

                        XmlElement cl2 = (XmlElement)xdoc.GetElementsByTagName("IncludeSubFolder")[i];
                        cl2.InnerText = "";
                        cl2.InnerText = this.checkBox1.Checked.ToString();

                        XmlElement cl3 = (XmlElement)xdoc.GetElementsByTagName("NumberOfHours")[i];
                        cl3.InnerText = "";
                        if (this.comboBox1.SelectedIndex == 0)
                        {
                            cl3.InnerText = "9";

                        }
                        if (this.comboBox1.SelectedIndex == 1)
                        {
                            cl3.InnerText = "12";
                        }
                        if (this.comboBox1.SelectedIndex == 2)
                        {
                            cl3.InnerText = "3";
                        }
                        if (this.comboBox1.SelectedIndex == 3)
                        {
                            cl3.InnerText = "6";
                        }

                        XmlElement cl4 = (XmlElement)xdoc.GetElementsByTagName("FolderLocation")[i];
                        cl4.InnerText = "";
                        cl4.InnerText = this.textBox2.Text;

                        XmlElement cl5 = (XmlElement)xdoc.GetElementsByTagName("ESCreate")[i];
                        cl5.InnerText = "";
                        cl5.InnerText = this.checkBox2.Checked.ToString();

                        XmlElement cl6 = (XmlElement)xdoc.GetElementsByTagName("ESDelete")[i];
                        cl6.InnerText = "";
                        cl6.InnerText = this.checkBox4.Checked.ToString();

                        XmlElement cl7 = (XmlElement)xdoc.GetElementsByTagName("ESRename")[i];
                        cl7.InnerText = "";
                        cl7.InnerText = this.checkBox3.Checked.ToString();

                        XmlElement cl8 = (XmlElement)xdoc.GetElementsByTagName("ESMove")[i];
                        cl8.InnerText = "";
                        cl8.InnerText = this.checkBox5.Checked.ToString();


                        NumberOfEAddress = listBox1.Items.Count;

                        switch (NumberOfEAddress)
                        {
                            case 0:
                                XmlElement cl01 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                                cl01.InnerText = "";
                                cl01.InnerText = "Nothing";
                                XmlElement cl02 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                                cl02.InnerText = "";
                                cl02.InnerText = "Nothing";
                                XmlElement cl03 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                                cl03.InnerText = "";
                                cl03.InnerText = "Nothing";
                                XmlElement cl04 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                                cl04.InnerText = "";
                                cl04.InnerText = "Nothing";
                                break;

                            case 1:
                                this.listBox1.SetSelected(0, true);
                                XmlElement cl11 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                                cl11.InnerText = "";
                                cl11.InnerText = this.listBox1.SelectedItem.ToString();

                                XmlElement cl12 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                                cl12.InnerText = "";
                                cl12.InnerText = "Nothing";
                                XmlElement cl13 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                                cl13.InnerText = "";
                                cl13.InnerText = "Nothing";
                                XmlElement cl14 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                                cl14.InnerText = "";
                                cl14.InnerText = "Nothing";
                                break;

                            case 2:
                                this.listBox1.SetSelected(0, true);
                                XmlElement cl21 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                                cl21.InnerText = "";
                                cl21.InnerText = this.listBox1.SelectedItem.ToString();

                                this.listBox1.SetSelected(1, true);
                                XmlElement cl22 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                                cl22.InnerText = "";
                                cl22.InnerText = this.listBox1.SelectedItem.ToString();

                                XmlElement cl23 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                                cl23.InnerText = "";
                                cl23.InnerText = "Nothing";
                                XmlElement cl24 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                                cl24.InnerText = "";
                                cl24.InnerText = "Nothing";
                                break;

                            case 3:
                                this.listBox1.SetSelected(0, true);
                                XmlElement cl31 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                                cl31.InnerText = "";
                                cl31.InnerText = this.listBox1.SelectedItem.ToString();

                                this.listBox1.SetSelected(1, true);
                                XmlElement cl32 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                                cl32.InnerText = "";
                                cl32.InnerText = this.listBox1.SelectedItem.ToString();

                                this.listBox1.SetSelected(2, true);
                                XmlElement cl33 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                                cl33.InnerText = "";
                                cl33.InnerText = this.listBox1.SelectedItem.ToString();

                                XmlElement cl34 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                                cl34.InnerText = "";
                                cl34.InnerText = "Nothing";
                                break;

                            case 4:
                                this.listBox1.SetSelected(0, true);
                                XmlElement cl41 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress1")[i];
                                cl41.InnerText = "";
                                cl41.InnerText = this.listBox1.SelectedItem.ToString();

                                this.listBox1.SetSelected(1, true);
                                XmlElement cl42 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress2")[i];
                                cl42.InnerText = "";
                                cl42.InnerText = this.listBox1.SelectedItem.ToString();

                                this.listBox1.SetSelected(2, true);
                                XmlElement cl43 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress3")[i];
                                cl43.InnerText = "";
                                cl43.InnerText = this.listBox1.SelectedItem.ToString();

                                XmlElement cl44 = (XmlElement)xdoc.GetElementsByTagName("EMailAddress4")[i];
                                cl44.InnerText = "";
                                cl44.InnerText = this.listBox1.SelectedItem.ToString();
                                break;

                        }
                        XmlElement cl103 = (XmlElement)xdoc.GetElementsByTagName("FromEmailAddress")[i];
                        cl103.InnerText = "";
                        cl103.InnerText = this.textBox4.Text;

                        XmlElement cl104 = (XmlElement)xdoc.GetElementsByTagName("HashPw")[i];
                        cl104.InnerText = "";
                        cl104.InnerText = this.textBox5.Text;

                        XmlElement cl105 = (XmlElement)xdoc.GetElementsByTagName("SMTPserver")[i];
                        cl105.InnerText = "";
                        cl105.InnerText = this.textBox7.Text;

                        XmlElement cl106 = (XmlElement)xdoc.GetElementsByTagName("SMTPserverport")[i];
                        cl106.InnerText = "";
                        cl106.InnerText = this.textBox8.Text;

                        XmlElement cl107 = (XmlElement)xdoc.GetElementsByTagName("EmailTimeSetting")[i];
                        cl107.InnerText = "";
                        if (radioButton1.Checked == true)
                        {
                            cl107.InnerText = "Immediate";
                        }
                        else
                        {
                            if (radioButton2.Checked == true)
                            {
                                cl107.InnerText = "12PM";
                            }
                            else
                            {
                                cl107.InnerText = "Nothing";
                            }
                        }
                       break;
                    }
                }
                up.Close();
                xdoc.Save(filename);
            }
            catch
            {
                MessageBox.Show("\"C:/MyPet/Config/MyPetConfig.xml\" not available");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string value = "";
            bool Stat = false;
            MyPet.CustomInputBox Tmp = new MyPet.CustomInputBox();

            if (Tmp.InputBox("Add Email Address", "Enter TO Email Address:", ref value, ref Stat) == DialogResult.OK)
            {

                if (Stat == false)
                {
                }
                else
                {
                    listBox1.Items.Add(value);
                }
            }

            if (listBox1.Items.Count >= 4)
            {
                this.button4.Enabled = false;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
            if (listBox1.Items.Count <= 3)
            {
                this.button4.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Form2validation1()
        {
            {
                form2validation1 = true;

                if (textBox3.Text == null || textBox3.Text.Length == 0)
                {
                    MessageBox.Show("Please enter sevice name");
                    form2validation1 = false;
                }
                if (textBox1.Text == null || textBox1.Text.Length == 0)
                {
                    MessageBox.Show("Please select the folder to monitor");
                    form2validation1 = false;
                }
                if (textBox2.Text == null || textBox2.Text.Length == 0)
                {
                    MessageBox.Show("Please select the folder to save the report file");
                    form2validation1 = false;
                }
                if ((checkBox2.Checked == true) || (checkBox3.Checked == true) || (checkBox4.Checked == true) || (checkBox5.Checked == true))
                {
                    Form2Validatation2();
                }

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            Form2Validatation2();
            if (form2validation2 == true)
            {
                try
                {
                    string FromEmailid;
                    string Password;
                    string Toid;
                    string Sub;
                    string Body;
                    SmtpClient smtp = new SmtpClient();
                    MailMessage mail = new MailMessage();
                    smtp.Host = textBox7.Text;
                    FromEmailid = textBox4.Text;
                    Password = textBox5.Text;
                    smtp.Credentials = new System.Net.NetworkCredential(FromEmailid, Password);
                    listBox1.SetSelected(0, true);
                    Toid = this.listBox1.SelectedItem.ToString();
                    mail.To.Add(Toid);
                    Sub = "Test mail from MyPet";
                    mail.Subject = Sub;
                    mail.From = new MailAddress(FromEmailid);
                    Body = "This mail has been sent by Mypet as a test mail. please do not repond.\nRegards  \nMyPet Email Notifier.";
                    mail.Body = Body;
                    smtp.Port = int.Parse(textBox8.Text);
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //MessageBox.Show("The message being sending. Please wait for result.");
                    smtp.Send(mail);
                    MessageBox.Show("Email testing successfull.");
                }
                catch (SmtpException ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Email testing unsuccessful. The provided credentials (one or more) for email notification are not valid. Please check.");
                }

            }
        }

        private void Form2Validatation2() 
        {
                    form2validation2 = true;
                    if (listBox1.Items.Count == 0)
                    {
                        MessageBox.Show("Please add TO email address for notification");
                        form2validation2 = false;
                    }


                    if (textBox4.Text == null || textBox4.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter FROM email address");
                        form2validation2 = false;
                    }
                    else
                    {
                        bool status;
                        status = Regex.IsMatch(textBox4.Text, @"
                        ^
                        [-a-zA-Z0-9][-.a-zA-Z0-9]*
                        @
                        [-.a-zA-Z0-9]+
                        (\.[-.a-zA-Z0-9]+)*
                        \.
                        (
                        com|edu|info|gov|int|mail|net|org|biz|
                        name|museum|coop|aero|pro|sg
                        |
                        [a-zA-Z]{2}
                        )
                        $",
                            RegexOptions.IgnorePatternWhitespace);

                        if (status == false)
                        {
                            MessageBox.Show("Enter valid FROM email address");
                            form2validation2 = false;
                            textBox4.Clear();
                        }
                    }

                    if (textBox5.Text == null || textBox5.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter password for FROM email address");
                        form2validation2 = false;
                    }

                    if (textBox6.Text == null || textBox6.Text.Length == 0)
                    {
                        MessageBox.Show("Please Retype the password for FROM email address");
                        form2validation2 = false;
                    }

                    if (textBox5.Text == textBox6.Text)
                    { }
                    else
                    {
                        MessageBox.Show("Password does not match each other. Please retype.");
                        form2validation2 = false;
                        textBox5.Clear();
                        textBox6.Clear();
                    }

                    if (textBox7.Text == null || textBox7.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter SMTP server name.");
                        form2validation2 = false;
                    }
                    if (textBox8.Text == null || textBox8.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter port number for SMTP server.");
                        form2validation2 = false;
                    }
                        Regex objNotWholePattern = new Regex("[^0-9]");
                        if (objNotWholePattern.IsMatch(textBox8.Text)== true)
                            {
                            MessageBox.Show("Please enter port number for SMTP server.");
                            form2validation2 = false;
                            textBox8.Clear();
                            }    
                    
           }

        private void textBox3_Enter(object sender, EventArgs e)
        {

        }

       
        //
    }
}