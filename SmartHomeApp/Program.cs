using System;
using SmartHomeApp.Devices;
using SmartHomeApp.Managers;

namespace SmartHomeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. INISIALISASI SISTEM & PERANGKAT
            DeviceManager<SmartAC> acHub = new DeviceManager<SmartAC>();
            SmartAC myAC = new SmartAC();
            acHub.AddDevice("AC-01", myAC);

            AutomationEngine myEngine = new AutomationEngine();
            // Mendaftarkan jadwal default untuk demonstrasi otomasi
            myEngine.AddSchedule("18:00", () => {
                Console.WriteLine(">>> [OTOMASI ENGINE] Waktunya menyalakan AC sore...");
                myAC.TurnOn();
                myAC.SetTemperature(22);
            });
            myEngine.AddSchedule("06:00", () => {
                Console.WriteLine(">>> [OTOMASI ENGINE] Waktunya mematikan AC pagi...");
                myAC.TurnOff();
            });

            SmartLight lampuPintar = new SmartLight();
            SecurityAlarm alarmRumah = new SecurityAlarm();

            bool isRunning = true;

            // 2. MAIN LOOP
            while (isRunning)
            {
                Console.WriteLine("\n=====================================");
                Console.WriteLine("Aplikasi Manajemen Rumah Pintar");
                Console.WriteLine("=====================================");
                Console.WriteLine("Pilih menu pengujian perangkat:");
                Console.WriteLine("1. Tes Smart AC & Otomasi Waktu");
                Console.WriteLine("2. Tes Smart Light (Warna via Cuaca)");
                Console.WriteLine("3. Tes Security Alarm");
                Console.WriteLine("4. Keluar");
                Console.Write("Masukkan pilihan Anda (1-4): ");
                
                string? pilihan = Console.ReadLine();

                Console.WriteLine("\n-------------------------------------");

                switch (pilihan)
                {
                    case "1":
                        // --- SKENARIO 1: SMART AC & OTOMASI ---
                        Console.WriteLine("=== MENU SMART AC ===");
                        Console.WriteLine("Jadwal Otomasi Terdaftar: 18:00 (AC Nyala), 06:00 (AC Mati)");
                        Console.Write("Masukkan jam saat ini (format HH:mm) untuk memicu otomasi: ");
                        string? inputWaktu = Console.ReadLine();

                        try
                        {
                            // Jika user input "18:00", AC akan otomatis nyala. 
                            // Jika user input "99:99", Regex DbC di AutomationEngine akan menolak.
                            if (inputWaktu != null)
                            {
                                myEngine.ExecuteSchedule(inputWaktu);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n[DBC DETECTED] Error Otomasi: {ex.Message}");
                        }
                        break;

                    case "2":
                        // --- SKENARIO 2: SMART LIGHT ---
                        Console.WriteLine("=== MENU SMART LIGHT ===");
                        // Lampu harus dinyalakan dulu agar tidak terkena Pre-Condition DbC
                        lampuPintar.TurnOn(); 
                        Console.Write("Masukkan kondisi cuaca saat ini (Clear / Rain / Clouds) atau ketik ngawur untuk tes DbC: ");
                        string? inputCuaca = Console.ReadLine();

                        try
                        {
                            if (inputCuaca != null)
                            {
                                // Jika input selain yang ada di Dictionary, akan melempar exception
                                lampuPintar.AdjustColorByWeather(inputCuaca);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n[DBC DETECTED] Error Lampu: {ex.Message}");
                        }
                        break;

                    case "3":
                        // --- SKENARIO 3: SECURITY ALARM ---
                        Console.WriteLine("=== MENU SECURITY ALARM ===");
                        Console.WriteLine($"Status Alarm Saat Ini: {alarmRumah.CurrentState}");
                        Console.Write("Masukkan PIN (minimal 4 digit) untuk mengaktifkan mode 'ArmedAway': ");
                        string? inputPin = Console.ReadLine();

                        try
                        {
                            if (inputPin != null)
                            {
                                // Jika PIN < 4 digit, DbC akan memblokir
                                alarmRumah.ChangeState(AlarmState.ArmedAway, inputPin);
                                Console.WriteLine($"Berhasil! Status Alarm sekarang: {alarmRumah.CurrentState}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n[DBC DETECTED] Error Alarm: {ex.Message}");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Menutup aplikasi... Terima kasih!");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan masukkan angka 1-4.");
                        break;
                }
            }
        }
    }
}