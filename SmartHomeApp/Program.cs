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
            Console.WriteLine("Developer: I Made Radithya Kusuma Wardana");
            Console.WriteLine("NIM: 103022400005 | Kelas: SE-48-04");
            Console.WriteLine("=====================================\n");

            try
            {
                // Inisialisasi Hub menggunakan Generics untuk menampung SmartAC
                DeviceManager<SmartAC> acHub = new DeviceManager<SmartAC>();
                Console.WriteLine($"[Sistem] Kapasitas Hub: {acHub.MaxDevicesAllowed} perangkat.\n");

                // Inisialisasi Perangkat (Buatan Radit)
                SmartAC acRuangTamu = new SmartAC();
                SmartAC acKamar = new SmartAC();

                // Mendaftarkan perangkat ke Hub (Buatan Rodzy)
                acHub.AddDevice("AC-01", acRuangTamu);
                acHub.AddDevice("AC-02", acKamar);

                // Menggunakan perangkat dari Hub
                var acTarget = acHub.GetDevice("AC-01");
                acTarget.TurnOn();
                acTarget.SetTemperature(22);

                // Tes Pelanggaran DbC Rodzy (Mendaftarkan ID yang sama)
                Console.WriteLine("\n[Tes Error] Mencoba mendaftarkan ID AC-01 lagi...");
                acHub.AddDevice("AC-01", new SmartAC());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR DbC Terdeteksi]: {ex.Message}");
            }
        }
    }
}