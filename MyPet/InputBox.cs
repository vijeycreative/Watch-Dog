using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace MyPet
{

    public partial class InputBox : Form
    {

        // the InputBox

        private static InputBox newInputBox;

        // the string that will be returned to the calling form

        private static string returnString;


        public InputBox()
        {

            InitializeComponent();

        }

        public static string Show(string inputBoxText)
        {

            newInputBox = new InputBox();

            newInputBox.Text = inputBoxText;

            newInputBox.ShowDialog();

            return returnString;

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

            returnString = string.Empty;

            newInputBox.Dispose();

        }

        // only used to add a little color to the background

        private void InputBox_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            Rectangle rec = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            LinearGradientBrush l = new LinearGradientBrush(rec, Color.LightSteelBlue, Color.Snow, LinearGradientMode.Vertical);

            g.FillRectangle(l, rec);

        }

        public static bool IsValidEmailAddress(string sEmail)
        {
            if (sEmail == null)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(sEmail, @"
                        ^
                        [-a-zA-Z0-9][-.a-zA-Z0-9]*
                        @
                        [-.a-zA-Z0-9]+
                        (\.[-.a-zA-Z0-9]+)*
                        \.
                        (
                        com|edu|info|gov|int|mil|net|org|biz|
                        name|museum|coop|aero|pro
                        |
                        [a-zA-Z]{2}
                        )
                        $",
                RegexOptions.IgnorePatternWhitespace);
            }
        }


        private void buttonOK_Click_1(object sender, EventArgs e)
        {

            bool status;
            status = IsValidEmailAddress(textBox1.Text);
            if (status == false)
            {
                MessageBox.Show("Enter valid email address");
                textBox1.Text = "";
                
            }
            else
            {
                returnString = textBox1.Text;
                newInputBox.Dispose();
            }
        }
    }
}

