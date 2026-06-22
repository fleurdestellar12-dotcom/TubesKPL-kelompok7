using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartHomeApp
{
    public class SmartHomeDashboard : Form
    {
        private Panel homePanel = new Panel();
        private Panel mainPanel = new Panel();

        public SmartHomeDashboard()
        {
            this.Text = "SMART HOME CONTROL PANEL";
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            SetupHomeLayout();
            SetupMainLayout();

            mainPanel.Visible = false;
            homePanel.Visible = true;
        }

        private void SetupHomeLayout()
        {
            homePanel.Bounds = new Rectangle(0, 0, 1366, 768);
            this.Controls.Add(homePanel);

            Label lblMainTitle = new Label { Text = "PROGRAM SMART HOME", Font = new Font("Arial", 42, FontStyle.Bold), ForeColor = Color.DarkBlue, Bounds = new Rectangle(0, 60, 1366, 70), TextAlign = ContentAlignment.MiddleCenter };
            homePanel.Controls.Add(lblMainTitle);

            Button btnSettings = CreateStyledButton("PENGATURAN DEVICE", 508, 560);
            btnSettings.BackColor = Color.DarkSlateBlue;
            btnSettings.Click += (s, e) => { homePanel.Visible = false; mainPanel.Visible = true; };
            homePanel.Controls.Add(btnSettings);
        }

        private void SetupMainLayout()
        {
            mainPanel.Bounds = new Rectangle(0, 0, 1366, 768);
            this.Controls.Add(mainPanel);

            Label title = new Label { Text = "PENGATURAN DEVICE", Font = new Font("Arial", 32, FontStyle.Bold), ForeColor = Color.DarkBlue, Bounds = new Rectangle(0, 50, 1366, 60), TextAlign = ContentAlignment.MiddleCenter };
            mainPanel.Controls.Add(title);
        }

        protected Button CreateStyledButton(string text, int x, int y)
        {
            return new Button { Text = text, Bounds = new Rectangle(x, y, 350, 70), BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 12, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
        }
    }
}