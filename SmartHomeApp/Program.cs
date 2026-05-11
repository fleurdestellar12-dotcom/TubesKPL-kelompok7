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
            Console.WriteLine("Kelompok 7 - Modul AC Pintar (Radit)");
            Console.WriteLine("=====================================\n");

            try
            {
                SmartAC ac = new SmartAC();

                
                ac.TurnOn();

                
                ac.SetTemperature(20);

                
                Console.WriteLine("\nMencoba mengatur suhu ke 10 derajat...");
                ac.SetTemperature(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR DbC Terdeteksi]: {ex.Message}");
            }

            Console.ReadLine(); 
        }
    }
}