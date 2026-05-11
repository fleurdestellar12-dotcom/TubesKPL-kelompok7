using System;
using SmartHomeApp.Devices;   
using SmartHomeApp.Managers;  

namespace SmartHomeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== APLIKASI RUMAH PINTAR (FINAL) ===");

            SmartAC myAC = new SmartAC();
            AutomationEngine myEngine = new AutomationEngine();

            try
            {
                
                myEngine.AddSchedule("18:00", () => {
                    Console.WriteLine(">>> Notifikasi: Waktunya menyalakan AC sore...");
                    myAC.TurnOn();
                    myAC.SetTemperature(22);
                });

                myEngine.AddSchedule("06:00", () => {
                    Console.WriteLine(">>> Notifikasi: Waktunya mematikan AC pagi...");
                    myAC.TurnOff();
                });

                
                myEngine.ExecuteSchedule("12:00"); 
                myEngine.ExecuteSchedule("18:00"); 

                
                Console.WriteLine("\n--- Tes Input Error ---");
                myEngine.AddSchedule("25:99", () => { });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DBC DETECTED]: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}