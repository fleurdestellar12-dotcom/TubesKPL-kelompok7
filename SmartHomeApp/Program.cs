using System;
using SmartHomeApp.Devices;
using SmartHomeApp.Managers;

namespace SmartHomeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("Aplikasi Manajemen Rumah Pintar");
            Console.WriteLine("Kelompok 7 - Modul Danu & Radit");
            Console.WriteLine("=====================================\n");

            try
            {
                // Inisialisasi Modul Radit (Smart AC)
                SmartAC acKamar = new SmartAC();

                // Inisialisasi Modul Danu (Automation Engine)
                AutomationEngine myEngine = new AutomationEngine();

                // Setup Jadwal (Table-Driven)
                myEngine.AddSchedule("18:00", () => {
                    Console.WriteLine(">>> [OTOMASI] Menyalakan AC sesuai jadwal...");
                    acKamar.TurnOn();
                    acKamar.SetTemperature(22);
                });

                // Simulasi
                myEngine.ExecuteSchedule("18:00");
            }
            catch (Exception ex)
            {
                // Defensive Programming
                Console.WriteLine($"\n[ERROR]: {ex.Message}");
            }

            Console.WriteLine("\nTekan Enter untuk keluar.");
            Console.ReadLine();
        }
    }
}