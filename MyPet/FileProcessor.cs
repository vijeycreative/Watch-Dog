using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace MyPet
{
    class FileProcessor
    {
       
        private Queue<string> workQueue1;
        private Queue<string> workQueue2;
        private Queue<string> workQueue3;
        private Thread workerThread;
        private EventWaitHandle waitHandle;
        private string FileName10; 



        public  FileProcessor()
        {
            workQueue1 = new Queue<string>();
            workQueue2 = new Queue<string>();
            workQueue3 = new Queue<string>();
            waitHandle = new AutoResetEvent(true);
            FileName10 = MyPet.Form1.filename1;
        }

        public void QueueInput(string Action, string Filepath, string DateAndTime)
        {


            workQueue1.Enqueue(Action);
            workQueue2.Enqueue(Filepath);
            workQueue3.Enqueue(DateAndTime);

            // Initialize and start thread when first file is added
            if (workerThread == null)
            {
                workerThread = new Thread(new ThreadStart(Work));
                workerThread.Start();
            }

            // If thread is waiting then start it
            else if (workerThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                waitHandle.Set();
            }
        }

        private void Work()
        {
            while (true)
            {
                string Action = RetrieveAction();
                string Filepath = RetrieveFilepath();
                string DateAndTime = RetrieveDateAndTime();

                if (Action != null)
                    AddDataToXML(Action, Filepath, DateAndTime);
                else
                    // If no files left to process then wait
                    waitHandle.WaitOne();
            }
        }

        private string RetrieveAction()
        {
            if (workQueue1.Count > 0)
                return workQueue1.Dequeue();
            else
                return null;
        }
        private string RetrieveFilepath()
        {
            if (workQueue2.Count > 0)
                return workQueue2.Dequeue();
            else
                return null;
        }

        private string RetrieveDateAndTime()
        {
            if (workQueue3.Count > 0)
                return workQueue3.Dequeue();
            else
                return null;
        }
        public void AddDataToXML(string Action, string FilePath, string DateAndTime)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                FileStream lfile = new FileStream(FileName10, FileMode.Open);
                xd.Load(lfile);
                XmlElement PR = xd.CreateElement("PartReport");

                XmlElement A = xd.CreateElement("Action");
                XmlText Atext = xd.CreateTextNode(Action);
                A.AppendChild(Atext);

                XmlElement FF = xd.CreateElement("FolderFileName");
                XmlText FFtext = xd.CreateTextNode(FilePath);
                FF.AppendChild(FFtext);

                XmlElement DT = xd.CreateElement("DateAndTime");
                XmlText DTtext = xd.CreateTextNode(DateAndTime);
                DT.AppendChild(DTtext);

                XmlElement CN = xd.CreateElement("ComputerName");
                XmlText CNtext = xd.CreateTextNode("");
                CN.AppendChild(CNtext);

                XmlElement US = xd.CreateElement("UserName");
                XmlText UStext = xd.CreateTextNode("");
                US.AppendChild(UStext);

                xd.DocumentElement.AppendChild(PR);
                PR.AppendChild(A);
                PR.AppendChild(FF);
                PR.AppendChild(DT);
                PR.AppendChild(CN);
                PR.AppendChild(US);

                lfile.Close();
                xd.Save(FileName10);
            }
            catch
            {
                MessageBox.Show("\".....FileProcessor-Mypet_File_Folder_Monitoring_Report.xml\" not available");
            }
        }


        //file processor ends here
    }
}
