using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GameServer.GUI
{
    public partial class Form1 : Form
    {
        private bool holdingWindow = false;
        private int windowGrabXPos;
        private int windowGrabYPos;

        public Form1()
        {
            InitializeComponent();
            this.TransparencyKey = Color.LimeGreen;

            GUIConsole.Init(OutputBox);

            InitFonts();
        }

        private void InitFonts()
        {
            PrivateFontCollection _pfc = new PrivateFontCollection();
            int _fontLength = Properties.Resources.Grand9K_Pixel.Length;
            byte[] _fontData = Properties.Resources.Grand9K_Pixel;
            IntPtr _data = Marshal.AllocCoTaskMem(_fontLength);
            Marshal.Copy(_fontData, 0, _data, _fontLength);
            _pfc.AddMemoryFont(_data, _fontLength);

            Font _pixelFont = new Font(_pfc.Families[0], 11);
            FormTitle.Font = new Font(_pfc.Families[0], FormTitle.Font.Size);
        }

        private void Submit()
        {
            string _command = InputBox.Text;
            InputBox.Text = "";

            GUIConsole.ProcessCommand(_command);
        }

        #region UIFunctions
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            holdingWindow = true;
            windowGrabXPos = MousePosition.X;
            windowGrabYPos = MousePosition.Y;
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (holdingWindow) {
                int _mouseMoveX = MousePosition.X - windowGrabXPos;
                int _mouseMoveY = MousePosition.Y - windowGrabYPos;
                windowGrabXPos = MousePosition.X;
                windowGrabYPos = MousePosition.Y;
                this.SetDesktopLocation(this.DesktopLocation.X + _mouseMoveX, this.DesktopLocation.Y + _mouseMoveY);
            }
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            holdingWindow = false;
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.BackgroundImage = Properties.Resources.CloseWindowCrossActive;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.BackgroundImage = Properties.Resources.CloseWindowCross;
        }

        private void CloseButton_MouseDown(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeBtn_MouseEnter(object sender, EventArgs e)
        {
            MinimizeBtn.BackgroundImage = Properties.Resources.MinimizeWindowActive;
        }

        private void MinimizeBtn_MouseLeave(object sender, EventArgs e)
        {
            MinimizeBtn.BackgroundImage = Properties.Resources.MinimizeWindow;
        }

        private void MinimizeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SubmitButton_MouseEnter(object sender, EventArgs e)
        {
            SubmitButton.BackgroundImage = Properties.Resources.SubmitButtonActive;
        }

        private void SubmitButton_MouseLeave(object sender, EventArgs e)
        {
            SubmitButton.BackgroundImage = Properties.Resources.SubmitButton;
        }

        private void SubmitButton_MouseDown(object sender, MouseEventArgs e)
        {
            Submit();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Submit();
            }
        }
        #endregion
    }
}
