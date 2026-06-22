using System;
using System.Drawing;
using System.Windows.Forms;
using SmartHomeApp.Devices;

namespace SmartHomeApp
{
    public class SmartHomeDashboard : Form
    {
        private SmartAC myAC = new SmartAC();
        private SmartLight myLight = new SmartLight();
        private SecurityAlarm myAlarm = new SecurityAlarm();
        
        private Panel homePanel = new Panel();
        private Panel mainPanel = new Panel();
        private Panel? subPanel;
        private RichTextBox? txtLog;

        private string statusAC = "MATI";
        private string statusLight = "MATI";
        private string statusAlarm = "DISARMED";

        private string logHistoryAC = "";
        private string logHistoryLight = "";
        private string logHistoryAlarm = "";
        
        private string currentAlarmPin = "";
        private bool isPinSet = false;

        private string currentDevice = "";

        private Label? lblStatusAC;
        private Label? lblStatusLight;
        private Label? lblStatusAlarm;

        private Button? btnToggleAC;
        private Button? btnToggleLight;
        private Button? btnToggleAlarm;

        public SmartHomeDashboard()
        {
            this.Text = "SMART HOME CONTROL PANEL";
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            SetupHomeLayout();
            SetupMainLayout();
            
            mainPanel.Visible = false;
            homePanel.Visible = true;
        }

        // ==========================================
        // 1. LAYAR HOME (STATUS DASHBOARD)
        // ==========================================
        private void SetupHomeLayout()
        {
            homePanel.Bounds = new Rectangle(0, 0, 1366, 768);
            this.Controls.Add(homePanel);

            Label lblHeader = new Label();
            lblHeader.Text = "TUGAS BESAR KONSTRUKSI PERANGKAT LUNAK";
            lblHeader.Font = new Font("Arial", 14, FontStyle.Bold);
            lblHeader.ForeColor = Color.Gray;
            lblHeader.Bounds = new Rectangle(0, 30, 1366, 30);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            homePanel.Controls.Add(lblHeader);

            Label lblMainTitle = new Label();
            lblMainTitle.Text = "PROGRAM SMART HOME";
            lblMainTitle.Font = new Font("Arial", 42, FontStyle.Bold);
            lblMainTitle.ForeColor = Color.DarkBlue;
            lblMainTitle.Bounds = new Rectangle(0, 60, 1366, 70);
            lblMainTitle.TextAlign = ContentAlignment.MiddleCenter;
            homePanel.Controls.Add(lblMainTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "oleh Kelompok 7";
            lblSubtitle.Font = new Font("Arial", 12, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.DimGray;
            lblSubtitle.Bounds = new Rectangle(0, 135, 1366, 30);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            homePanel.Controls.Add(lblSubtitle);

            Label lblStatusHeader = new Label();
            lblStatusHeader.Text = "--- STATUS DEVICE SAAT INI ---";
            lblStatusHeader.Font = new Font("Arial", 16, FontStyle.Bold);
            lblStatusHeader.ForeColor = Color.DarkSlateBlue;
            lblStatusHeader.Bounds = new Rectangle(0, 210, 1366, 30);
            lblStatusHeader.TextAlign = ContentAlignment.MiddleCenter;
            homePanel.Controls.Add(lblStatusHeader);

            lblStatusAC = CreateStatusLabel("AC\n" + statusAC, 80, 260, Color.LightSkyBlue);
            lblStatusLight = CreateStatusLabel("LAMPU\n" + statusLight, 508, 260, Color.LightGoldenrodYellow);
            lblStatusAlarm = CreateStatusLabel("ALARM\n" + statusAlarm, 936, 260, Color.LightCoral);

            homePanel.Controls.Add(lblStatusAC);
            homePanel.Controls.Add(lblStatusLight);
            homePanel.Controls.Add(lblStatusAlarm);

            btnToggleAC = CreateStyledButton("HIDUPKAN", 80, 520);
            btnToggleAC.Height = 50;
            btnToggleAC.BackColor = Color.MediumSeaGreen;
            btnToggleAC.Click += (s, e) => {
                if (statusAC.Contains("NYALA")) {
                    myAC.TurnOff(); statusAC = "MATI";
                    logHistoryAC += $"[{DateTime.Now:HH:mm:ss}] > AC Berhasil Dimatikan (via Home).\n";
                } else {
                    myAC.TurnOn(); statusAC = "NYALA (Cooling)";
                    logHistoryAC += $"[{DateTime.Now:HH:mm:ss}] > AC Berhasil Dinyalakan (via Home).\n";
                }
                RefreshHomeStatus();
            };
            homePanel.Controls.Add(btnToggleAC);

            btnToggleLight = CreateStyledButton("HIDUPKAN", 508, 520);
            btnToggleLight.Height = 50;
            btnToggleLight.BackColor = Color.MediumSeaGreen;
            btnToggleLight.Click += (s, e) => {
                if (statusLight.Contains("NYALA")) {
                    myLight.TurnOff(); statusLight = "MATI";
                    logHistoryLight += $"[{DateTime.Now:HH:mm:ss}] > Lampu Pintar Dimatikan (via Home).\n";
                } else {
                    myLight.TurnOn(); statusLight = "NYALA";
                    logHistoryLight += $"[{DateTime.Now:HH:mm:ss}] > Lampu Pintar Aktif (via Home).\n";
                }
                RefreshHomeStatus();
            };
            homePanel.Controls.Add(btnToggleLight);

            btnToggleAlarm = CreateStyledButton("AKTIFKAN", 936, 520);
            btnToggleAlarm.Height = 50;
            btnToggleAlarm.BackColor = Color.MediumSeaGreen;
            btnToggleAlarm.Click += (s, e) => {
                if (!isPinSet) {
                    MessageBox.Show("Sistem Terkunci: Anda harus menyetel PIN Awal di menu pengaturan terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string inputPin = ShowPinDialog();
                if (inputPin == currentAlarmPin) {
                    if (!statusAlarm.Contains("DISARMED")) {
                        myAlarm.ChangeState(AlarmState.Disarmed, inputPin);
                        statusAlarm = "DISARMED (MATI)";
                        logHistoryAlarm += $"[{DateTime.Now:HH:mm:ss}] > Automata Transisi: Alarm Berhasil Dimatikan (via Home).\n";
                    } else {
                        myAlarm.ChangeState(AlarmState.ArmedAway, inputPin);
                        statusAlarm = "ARMED (AKTIF)";
                        logHistoryAlarm += $"[{DateTime.Now:HH:mm:ss}] > Automata Transisi: Alarm Berhasil Diaktifkan (via Home).\n";
                    }
                    RefreshHomeStatus();
                } else if (!string.IsNullOrEmpty(inputPin)) {
                    MessageBox.Show("Akses Ditolak: PIN Salah!", "Error Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            homePanel.Controls.Add(btnToggleAlarm);

            Button btnSettings = CreateStyledButton("PENGATURAN DEVICE", 508, 610);
            btnSettings.BackColor = Color.DarkSlateBlue;
            btnSettings.Click += (s, e) => { 
                homePanel.Visible = false; 
                mainPanel.Visible = true; 
            };
            homePanel.Controls.Add(btnSettings);
        }

        private Label CreateStatusLabel(string text, int x, int y, Color bgColor)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Arial", 20, FontStyle.Bold);
            lbl.BackColor = bgColor;
            lbl.ForeColor = Color.DarkBlue;
            lbl.Bounds = new Rectangle(x, y, 350, 250);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.BorderStyle = BorderStyle.FixedSingle;
            return lbl;
        }

        private void RefreshHomeStatus()
        {
            if (lblStatusAC != null) lblStatusAC.Text = "AC\n" + statusAC;
            if (lblStatusLight != null) lblStatusLight.Text = "LAMPU\n" + statusLight;
            if (lblStatusAlarm != null) lblStatusAlarm.Text = "ALARM\n" + statusAlarm;

            if (btnToggleAC != null) {
                btnToggleAC.Text = statusAC.Contains("NYALA") ? "MATIKAN" : "HIDUPKAN";
                btnToggleAC.BackColor = statusAC.Contains("NYALA") ? Color.IndianRed : Color.MediumSeaGreen;
            }

            if (btnToggleLight != null) {
                btnToggleLight.Text = statusLight.Contains("NYALA") ? "MATIKAN" : "HIDUPKAN";
                btnToggleLight.BackColor = statusLight.Contains("NYALA") ? Color.IndianRed : Color.MediumSeaGreen;
            }

            if (btnToggleAlarm != null) {
                bool isAlarmOn = !statusAlarm.Contains("DISARMED");
                btnToggleAlarm.Text = isAlarmOn ? "MATIKAN" : "AKTIFKAN";
                btnToggleAlarm.BackColor = isAlarmOn ? Color.IndianRed : Color.MediumSeaGreen;
            }
        }

        // ==========================================
        // 2. LAYAR MAIN (PILIH DEVICE)
        // ==========================================
        private void SetupMainLayout()
        {
            mainPanel.Bounds = new Rectangle(0, 0, 1366, 768);
            this.Controls.Add(mainPanel);

            Label title = new Label();
            title.Text = "PENGATURAN DEVICE";
            title.Font = new Font("Arial", 32, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.Bounds = new Rectangle(0, 50, 1366, 60);
            title.TextAlign = ContentAlignment.MiddleCenter;
            mainPanel.Controls.Add(title);

            int startX = 508; 
            int startY = 140;

            Button btnAC = CreateStyledButton("SMART AC", startX, startY);
            btnAC.Click += (s, e) => ShowSubMenu("AC");

            Button btnLight = CreateStyledButton("SMART LIGHT", startX, startY + 90);
            btnLight.Click += (s, e) => ShowSubMenu("LIGHT");

            Button btnAlarm = CreateStyledButton("SECURITY ALARM", startX, startY + 180);
            btnAlarm.Click += (s, e) => ShowSubMenu("ALARM");

            Button btnTambah = CreateStyledButton("TAMBAH DEVICE", startX, startY + 270);
            btnTambah.Width = 170;
            btnTambah.BackColor = Color.MediumSeaGreen;
            btnTambah.Click += (s, e) => ShowAddDeviceDialog();

            Button btnHapus = CreateStyledButton("HAPUS DEVICE", startX + 180, startY + 270);
            btnHapus.Width = 170;
            btnHapus.BackColor = Color.IndianRed;
            btnHapus.Click += (s, e) => ShowRemoveDeviceDialog();

            Button btnHome = CreateStyledButton("KEMBALI KE HOME", startX, startY + 360);
            btnHome.BackColor = Color.Gray;
            btnHome.Click += (s, e) => { 
                mainPanel.Visible = false; 
                RefreshHomeStatus(); 
                homePanel.Visible = true; 
            };

            mainPanel.Controls.Add(btnAC);
            mainPanel.Controls.Add(btnLight);
            mainPanel.Controls.Add(btnAlarm);
            mainPanel.Controls.Add(btnTambah);
            mainPanel.Controls.Add(btnHapus);
            mainPanel.Controls.Add(btnHome);
        }

        // ==========================================
        // 3. LAYAR SUB-MENU (KONTROL DETAIL)
        // ==========================================
        private void ShowSubMenu(string deviceType)
        {
            mainPanel.Visible = false;
            currentDevice = deviceType; 
            
            subPanel = new Panel();
            subPanel.Bounds = new Rectangle(0, 0, 1366, 768);
            this.Controls.Add(subPanel);

            txtLog = new RichTextBox();
            txtLog.Bounds = new Rectangle(80, 50, 1200, 200);
            txtLog.ReadOnly = true;
            txtLog.BackColor = Color.AliceBlue;
            txtLog.Font = new Font("Consolas", 12);
            
            if (deviceType == "AC") txtLog.Text = logHistoryAC;
            else if (deviceType == "LIGHT") txtLog.Text = logHistoryLight;
            else if (deviceType == "ALARM") txtLog.Text = logHistoryAlarm;

            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            
            subPanel.Controls.Add(txtLog);

            Button btnBack = CreateStyledButton("KEMBALI", 508, 600);
            btnBack.BackColor = Color.Gray;
            btnBack.Click += (s, e) => 
            { 
                if (subPanel != null) 
                { 
                    this.Controls.Remove(subPanel); 
                } 
                currentDevice = ""; 
                mainPanel.Visible = true; 
            };
            subPanel.Controls.Add(btnBack);

            if (deviceType == "AC") SetupACMenu();
            else if (deviceType == "LIGHT") SetupLightMenu();
            else if (deviceType == "ALARM") SetupAlarmMenu();
        }

        private void SetupACMenu()
        {
            Button btnOn = new Button();
            Button btnOff = new Button();

            btnOn = AddSubButton("Nyalakan AC", 80, 300, (s, e) => { 
                myAC.TurnOn(); 
                statusAC = "NYALA (Cooling)";
                LogStatus("AC Berhasil Dinyalakan."); 
                btnOn.BackColor = Color.MediumSeaGreen;
                btnOff.BackColor = Color.Gray;
            });
            
            btnOff = AddSubButton("Matikan AC", 490, 300, (s, e) => { 
                myAC.TurnOff(); 
                statusAC = "MATI";
                LogStatus("AC Berhasil Dimatikan."); 
                btnOn.BackColor = Color.Gray;
                btnOff.BackColor = Color.IndianRed;
            });

            if (statusAC.Contains("NYALA")) {
                btnOn.BackColor = Color.MediumSeaGreen;
                btnOff.BackColor = Color.Gray;
            } else {
                btnOn.BackColor = Color.Gray;
                btnOff.BackColor = Color.IndianRed;
            }
            
            Label lblTemp = new Label();
            lblTemp.Text = "Suhu (°C):";
            lblTemp.Bounds = new Rectangle(900, 260, 100, 30);
            lblTemp.Font = new Font("Arial", 12, FontStyle.Bold);
            
            ComboBox cmbTemp = new ComboBox();
            cmbTemp.Bounds = new Rectangle(1000, 260, 100, 30);
            cmbTemp.Font = new Font("Arial", 12);
            cmbTemp.DropDownStyle = ComboBoxStyle.DropDownList; 
            
            for (int i = 16; i <= 30; i++)
            {
                cmbTemp.Items.Add(i.ToString());
            }
            cmbTemp.SelectedItem = "24"; 
            
            if (subPanel != null)
            {
                subPanel.Controls.Add(lblTemp);
                subPanel.Controls.Add(cmbTemp);
            }

            AddSubButton("Ubah Suhu", 900, 300, (s, e) => { 
                if (cmbTemp.SelectedItem != null && int.TryParse(cmbTemp.SelectedItem.ToString(), out int suhu))
                {
                    try 
                    { 
                        myAC.SetTemperature(suhu); 
                        statusAC = $"NYALA ({suhu}°C)";
                        LogStatus($"Kontrak Valid: Suhu AC disesuaikan ke {suhu}°C."); 
                    } 
                    catch(Exception ex) 
                    { 
                        LogStatus("DbC Error: " + ex.Message); 
                    } 
                }
                else
                {
                    LogStatus("Validasi Gagal: Silakan pilih angka suhu!");
                }
            });
        }

        private void SetupLightMenu()
        {
            Button btnOn = new Button();
            Button btnOff = new Button();

            btnOn = AddSubButton("Nyalakan Lampu", 80, 300, (s, e) => { 
                myLight.TurnOn(); 
                statusLight = "NYALA";
                LogStatus("Lampu Pintar Aktif."); 
                btnOn.BackColor = Color.MediumSeaGreen;
                btnOff.BackColor = Color.Gray;
            });

            btnOff = AddSubButton("Matikan Lampu", 490, 300, (s, e) => { 
                myLight.TurnOff();
                statusLight = "MATI";
                LogStatus("Lampu Pintar Dimatikan."); 
                btnOn.BackColor = Color.Gray;
                btnOff.BackColor = Color.IndianRed;
            });

            if (statusLight.Contains("NYALA")) {
                btnOn.BackColor = Color.MediumSeaGreen;
                btnOff.BackColor = Color.Gray;
            } else {
                btnOn.BackColor = Color.Gray;
                btnOff.BackColor = Color.IndianRed;
            }
            
            AddSubButton("Sesuaikan Cuaca", 900, 300, async (s, e) => { 
                try 
                {
                    LogStatus("Menghubungkan ke API Cuaca Open-Meteo (Lokasi: Telkom University)...");
                    
                    string skenario = await myLight.FetchWeatherFromAPI();
                    
                    myLight.AdjustColorByWeatherAPI(skenario, out string warna);
                    statusLight = $"NYALA ({warna})";
                    LogStatus($"API Berhasil: Terdeteksi [{skenario}] -> Lampu menyesuaikan warna menjadi {warna}.");
                } 
                catch(Exception ex) 
                {
                    LogStatus("Error API/DbC: " + ex.Message);
                }
            });
        }

        private void SetupAlarmMenu()
        {
            Label lblPin = new Label();
            lblPin.Text = "PIN Saja:";
            lblPin.Bounds = new Rectangle(80, 260, 150, 30);
            lblPin.Font = new Font("Arial", 12, FontStyle.Bold);
            
            TextBox txtPin = new TextBox();
            txtPin.Bounds = new Rectangle(240, 260, 150, 30);
            txtPin.PasswordChar = '*';
            txtPin.MaxLength = 4;
            txtPin.Font = new Font("Arial", 12);
            
            Label lblNewPin = new Label();
            lblNewPin.Text = "PIN Baru:";
            lblNewPin.Bounds = new Rectangle(430, 260, 100, 30);
            lblNewPin.Font = new Font("Arial", 12, FontStyle.Bold);
            
            TextBox txtNewPin = new TextBox();
            txtNewPin.Bounds = new Rectangle(530, 260, 150, 30);
            txtNewPin.PasswordChar = '*';
            txtNewPin.MaxLength = 4;
            txtNewPin.Font = new Font("Arial", 12);
            
            if (subPanel != null)
            {
                subPanel.Controls.Add(lblPin);
                subPanel.Controls.Add(txtPin);
                subPanel.Controls.Add(lblNewPin);
                subPanel.Controls.Add(txtNewPin);
            }

            Button btnOn = new Button();
            Button btnOff = new Button();
            Button btnSetPin = new Button();

            btnOn = AddSubButton("Aktifkan Alarm", 80, 320, (s, e) => {
                if (!isPinSet) { LogStatus("Sistem Terkunci: Anda harus menyetel PIN Awal terlebih dahulu!"); return; }
                if (txtPin.Text != currentAlarmPin) { LogStatus("Akses Ditolak: PIN Salah!"); return; }
                
                try 
                { 
                    myAlarm.ChangeState(AlarmState.ArmedAway, txtPin.Text); 
                    statusAlarm = "ARMED (AKTIF)";
                    LogStatus("Automata Transisi: Alarm Berhasil Diaktifkan."); 
                    btnOn.BackColor = Color.MediumSeaGreen;
                    btnOff.BackColor = Color.Gray;
                    txtPin.Clear();
                } 
                catch(Exception ex) 
                { 
                    LogStatus("DbC Validasi Gagal: " + ex.Message); 
                }
            });

            btnOff = AddSubButton("Matikan Alarm", 490, 320, (s, e) => {
                if (!isPinSet) { LogStatus("Sistem Terkunci: Anda harus menyetel PIN Awal terlebih dahulu!"); return; }
                if (txtPin.Text != currentAlarmPin) { LogStatus("Akses Ditolak: PIN Salah!"); return; }
                
                try 
                { 
                    myAlarm.ChangeState(AlarmState.Disarmed, txtPin.Text); 
                    statusAlarm = "DISARMED (MATI)";
                    LogStatus("Automata Transisi: Alarm Berhasil Dimatikan."); 
                    btnOn.BackColor = Color.Gray;
                    btnOff.BackColor = Color.IndianRed;
                    txtPin.Clear();
                } 
                catch(Exception ex) 
                { 
                    LogStatus("DbC Validasi Gagal: " + ex.Message); 
                }
            });

            btnSetPin = AddSubButton(isPinSet ? "Ganti PIN" : "Setel PIN Awal", 900, 320, (s, e) => {
                if (!isPinSet) 
                {
                    if (txtNewPin.Text.Length == 4 && int.TryParse(txtNewPin.Text, out _)) 
                    {
                        currentAlarmPin = txtNewPin.Text;
                        isPinSet = true;
                        btnSetPin.Text = "Ganti PIN";
                        LogStatus("PIN awal berhasil disetel! Sistem Alarm siap digunakan.");
                        txtNewPin.Clear();
                    } 
                    else 
                    {
                        LogStatus("Validasi Gagal: PIN Baru harus berupa 4 digit angka!");
                    }
                } 
                else 
                {
                    if (txtPin.Text == currentAlarmPin) 
                    {
                        if (txtNewPin.Text.Length == 4 && int.TryParse(txtNewPin.Text, out _)) 
                        {
                            currentAlarmPin = txtNewPin.Text;
                            LogStatus("Autentikasi Sukses: PIN Alarm berhasil diganti ke yang baru!");
                            txtPin.Clear();
                            txtNewPin.Clear();
                        } 
                        else 
                        {
                            LogStatus("Validasi Gagal: PIN Baru harus berupa 4 digit angka!");
                        }
                    } 
                    else 
                    {
                        LogStatus("Akses Ditolak: Masukkan PIN yang benar untuk mengganti PIN!");
                    }
                }
            });
            btnSetPin.BackColor = Color.DarkOrange;

            if (!statusAlarm.Contains("DISARMED")) {
                btnOn.BackColor = Color.MediumSeaGreen;
                btnOff.BackColor = Color.Gray;
            } else {
                btnOn.BackColor = Color.Gray;
                btnOff.BackColor = Color.IndianRed;
            }
        }

        // ==========================================
        // FUNGSI PEMBANTU (HELPER)
        // ==========================================
        private Button AddSubButton(string text, int x, int y, EventHandler handler) 
        {
            Button btn = CreateStyledButton(text, x, y);
            btn.Click += handler;
            if (subPanel != null) {
                subPanel.Controls.Add(btn);
            }
            return btn;
        }

        private Button CreateStyledButton(string text, int x, int y) 
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Bounds = new Rectangle(x, y, 350, 70);
            btn.BackColor = Color.DodgerBlue;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void LogStatus(string message) 
        { 
            string logEntry = "[" + DateTime.Now.ToString("HH:mm:ss") + "] > " + message + "\n";
            
            if (currentDevice == "AC") logHistoryAC += logEntry;
            else if (currentDevice == "LIGHT") logHistoryLight += logEntry;
            else if (currentDevice == "ALARM") logHistoryAlarm += logEntry;

            if (txtLog != null && !txtLog.IsDisposed) {
                txtLog.AppendText(logEntry); 
                txtLog.ScrollToCaret(); 
            }
        }

        private string ShowPinDialog()
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Otorisasi Alarm",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Masukkan PIN Alarm:" };
            TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 240, PasswordChar = '*' };
            Button confirmation = new Button() { Text = "OK", Left = 160, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
        }

        private void ShowAddDeviceDialog()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Tambah Device Baru",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Width = 300, Text = "Pilih device yang ingin ditambahkan:" };
            ComboBox cmbDevices = new ComboBox() { Left = 20, Top = 50, Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
            
            cmbDevices.Items.AddRange(new string[] { "Smart TV", "Smart Refrigerator", "Smart Doorbell", "Smart Curtain", "CCTV Camera" });
            cmbDevices.SelectedIndex = 0;
            
            Button confirmation = new Button() { Text = "Tambah", Left = 210, Width = 100, Top = 100, DialogResult = DialogResult.OK, BackColor = Color.MediumSeaGreen, ForeColor = Color.White };
            
            confirmation.Click += (sender, e) => { 
                MessageBox.Show($"Simulasi: {cmbDevices.SelectedItem} berhasil ditambahkan ke jaringan Smart Home!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                prompt.Close(); 
            };
            
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(cmbDevices);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();
        }

        private void ShowRemoveDeviceDialog()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Hapus Device",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Width = 300, Text = "Pilih device yang ingin dihapus:" };
            ComboBox cmbDevices = new ComboBox() { Left = 20, Top = 50, Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
            
            cmbDevices.Items.AddRange(new string[] { "Smart AC", "Smart Light", "Security Alarm" });
            cmbDevices.SelectedIndex = 0;
            
            Button confirmation = new Button() { Text = "Hapus", Left = 210, Width = 100, Top = 100, DialogResult = DialogResult.OK, BackColor = Color.IndianRed, ForeColor = Color.White };
            
            confirmation.Click += (sender, e) => { 
                MessageBox.Show($"Simulasi: {cmbDevices.SelectedItem} berhasil dihapus dari sistem!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                prompt.Close(); 
            };
            
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(cmbDevices);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();
        }
    }
}