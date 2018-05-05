using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyPet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To show the splash screen
            Splash a = new Splash();
            for (int i = 30000; i > 0; i--)
            {
                a.Show();
            }
            a.Close();
            //To show splash screen

            Application.EnableVisualStyles();
            // Specify a "currently active folder"
            string activeDir = @"C:\";

            //Create a new subfolder under the current active folder
            string newPath = System.IO.Path.Combine(activeDir, "MyPet");

            // Create the subfolder
            System.IO.Directory.CreateDirectory(newPath);

            // Specify a "currently active folder"
            string activeDir1 = @"C:\Mypet";

            //Create a new subfolder under the current active folder
            string newPath1 = System.IO.Path.Combine(activeDir1, "LogFiles");

            //Create a new subfolder under the current active folder
            string newPath2 = System.IO.Path.Combine(activeDir1, "Config");

            //Create a new subfolder under the current active folder
            string newPath3 = System.IO.Path.Combine(activeDir1, "AppSettings");


            // Create the subfolder
            System.IO.Directory.CreateDirectory(newPath1);
            System.IO.Directory.CreateDirectory(newPath2);
            System.IO.Directory.CreateDirectory(newPath3);
     
            Application.Run(new Form1());


        }
    }
}
