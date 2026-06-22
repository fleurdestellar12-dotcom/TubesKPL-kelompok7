using System;
using System.Drawing;
using System.Windows.Forms;
using SmartHomeApp.Devices;

namespace SmartHomeApp
{
    public class SmartHomeDashboard : Form
    {
        private SmartLight myLight = new SmartLight();
        
        private Panel homePanel = new Panel();
        private Panel mainPanel = new Panel();
        private Panel? subPanel;
        private RichTextBox? txtLog;
        
        protected string statusLight = "MATI";

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

        private void SetupHomeLayout() { /* Sama seperti Tahap 3 */ 
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

            Button btnLight = CreateStyledButton("SMART LIGHT", 508, 280);
            btnLight.Click += (s, e) => ShowSubMenu("LIGHT");
            mainPanel.Controls.Add(btnLight);
        }

        protected void ShowSubMenu(string deviceType)
        {
            mainPanel.Visible = false;
            subPanel = new Panel { Bounds = new Rectangle(0, 0, 1366, 768) };
            this.Controls.Add(subPanel);

            txtLog = new RichTextBox { Bounds = new Rectangle(80, 50, 1200, 200), ReadOnly = true, BackColor = Color.AliceBlue, Font = new Font("Consolas", 12) };
            subPanel.Controls.Add(txtLog);

            Button btnBack = CreateStyledButton("KEMBALI", 508, 600);
            btnBack.BackColor = Color.Gray;
            btnBack.Click += (s, e) => { if (subPanel != null) { this.Controls.Remove(subPanel); } mainPanel.Visible = true; };
            subPanel.Controls.Add(btnBack);

            if (deviceType == "LIGHT") { SetupLightMenu(); }
        }

        private void SetupLightMenu()
        {
            AddSubButton("Nyalakan Lampu", 80, 300, (s, e) => { myLight.TurnOn(); statusLight = "NYALA"; LogStatus("Lampu Pintar Aktif."); });
            AddSubButton("Matikan Lampu", 490, 300, (s, e) => { myLight.TurnOff(); statusLight = "MATI"; LogStatus("Lampu Pintar Dimatikan."); });
            AddSubButton("Sesuaikan Cuaca", 900, 300, (s, e) => { 
                try {
                    myLight.AdjustColorByRandomWeather(out string skenario, out string warna);
                    LogStatus($"Sensor Aktif: Cuaca [{skenario}] -> Warna {warna}.");
                } catch(Exception ex) { LogStatus("DbC Error: " + ex.Message); }
            });
        }

        protected void AddSubButton(string text, int x, int y, EventHandler handler) {
            Button btn = CreateStyledButton(text, x, y); btn.Click += handler; subPanel?.Controls.Add(btn);
        }

        protected Button CreateStyledButton(string text, int x, int y) {
            return new Button { Text = text, Bounds = new Rectangle(x, y, 350, 70), BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 12, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
        }

        protected void LogStatus(string message) { 
            txtLog?.AppendText($"[{DateTime.Now:HH:mm:ss}] > {message}\n"); txtLog?.ScrollToCaret(); 
        }
    }
}
