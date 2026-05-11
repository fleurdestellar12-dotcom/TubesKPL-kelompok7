using System;
using SmartHomeApp.Devices;

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
                SmartAC ac = new SmartAC();
                ac.TurnOn();
                
                // Mengetes perubahan suhu yang valid
                ac.SetTemperature(20);
                
                // Mengetes pelanggaran DbC (Akan melempar exception)
                Console.WriteLine("\nMencoba mengatur suhu ke 10 derajat...");
                ac.SetTemperature(10); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR DbC Terdeteksi]: {ex.Message}");
            }
        }
    }
}