using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartHomeApp
{
    public class SmartHomeDashboard : Form
    {
        public SmartHomeDashboard()
        {
            this.Text = "SMART HOME CONTROL PANEL";
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}