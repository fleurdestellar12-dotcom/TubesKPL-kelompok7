using System;
using SmartHomeApp.Devices;
using SmartHomeApp.Managers;

namespace SmartHomeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // --- Header Program (Dari main/Rodzy & Radit) ---
            Console.WriteLine("=====================================");
            Console.WriteLine("Aplikasi Manajemen Rumah Pintar");
            Console.WriteLine("Developer: I Made Radithya Kusuma Wardana");
            Console.WriteLine("NIM: 103022400005 | Kelas: SE-48-04");
            Console.WriteLine("=====================================\n");

            try
            {
                // --- 1. Inisialisasi Hub (Generics - Dari main/Rodzy) ---
                // Menggunakan class DeviceManager generik untuk menampung SmartAC
                DeviceManager<SmartAC> acHub = new DeviceManager<SmartAC>();
                Console.WriteLine($"[Sistem] Kapasitas Hub: {acHub.MaxDevicesAllowed} perangkat.\n");

                // Inisialisasi Perangkat AC
                SmartAC acRuangTamu = new SmartAC();
                SmartAC acKamar = new SmartAC();

                // Mendaftarkan perangkat ke Hub (Modul Rodzy)
                acHub.AddDevice("AC-01", acRuangTamu);
                acHub.AddDevice("AC-02", acKamar);

                // --- 2. Inisialisasi Automation Engine (Tugas Danu) ---
                AutomationEngine myEngine = new AutomationEngine();
                Console.WriteLine("=== KONFIGURASI OTOMASI (DANU) ===");

                // Menambah Jadwal Otomasi menggunakan Table-Driven Construction
                // Integrasi: Mengambil perangkat dari Hub milik Rodzy untuk dikontrol
                myEngine.AddSchedule("18:00", () => {
                    Console.WriteLine(">>> [OTOMASI] Waktunya menyalakan AC sore...");
                    var target = acHub.GetDevice("AC-01"); 
                    target.TurnOn();
                    target.SetTemperature(22);
                });

                myEngine.AddSchedule("06:00", () => {
                    Console.WriteLine(">>> [OTOMASI] Waktunya mematikan AC pagi...");
                    acHub.GetDevice("AC-01").TurnOff();
                });

                // --- 3. Simulasi Eksekusi Jadwal ---
                myEngine.ExecuteSchedule("12:00"); // Jam tanpa jadwal
                myEngine.ExecuteSchedule("18:00"); // Jam dengan jadwal otomasi

                // --- 4. Tes Validasi Design by Contract (DbC) ---
                Console.WriteLine("\n--- Tes Validasi Error ---");
                
                // Tes DbC Danu (Waktu salah format)
                Console.WriteLine("[Tes] Input waktu salah:");
                myEngine.AddSchedule("25:99", () => { });
            }
            catch (Exception ex)
            {
                // Menangkap exception dari DbC (Modul Radit/Danu/Rodzy)
                Console.WriteLine($"\n[DBC DETECTED]: {ex.Message}");
            }

            Console.WriteLine("\nProgram Selesai. Tekan Enter untuk keluar.");
            Console.ReadLine();
        }
    }
}