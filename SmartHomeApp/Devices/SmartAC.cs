using System;
using System.IO;
using System.Text.Json;

namespace SmartHomeApp.Devices
{
    public enum ACState
    {
        Off,
        Cooling,
        FanOnly
    }

    public class SmartAC
    {
        public ACState CurrentState { get; private set; }
        public int Temperature { get; private set; }

        private readonly int _minTemp;
        private readonly int _maxTemp;

        public SmartAC()
        {
           
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "appsettings.json");
            string jsonString = File.ReadAllText(configPath);
            using JsonDocument doc = JsonDocument.Parse(jsonString);

            var settings = doc.RootElement.GetProperty("ACSettings");
            Temperature = settings.GetProperty("DefaultTemperature").GetInt32();
            _minTemp = settings.GetProperty("MinTemperature").GetInt32();
            _maxTemp = settings.GetProperty("MaxTemperature").GetInt32();

            CurrentState = ACState.Off;
        }

        
        public void TurnOn()
        {
            CurrentState = ACState.Cooling;
            Console.WriteLine("AC Dinyalakan. Mode: Cooling.");
        }

        public void TurnOff()
        {
            CurrentState = ACState.Off;
            Console.WriteLine("AC Dimatikan.");
        }

        
        public void SetTemperature(int newTemp)
        {
            
            if (CurrentState == ACState.Off)
            {
                throw new InvalidOperationException("Contract Failed: Tidak bisa mengatur suhu saat AC mati.");
            }

           
            if (newTemp < _minTemp || newTemp > _maxTemp)
            {
                throw new ArgumentOutOfRangeException(nameof(newTemp), $"Contract Failed: Suhu harus antara {_minTemp} dan {_maxTemp} derajat.");
            }

            Temperature = newTemp;
            Console.WriteLine($"Suhu berhasil diubah menjadi {Temperature}°C.");
        }
    }
}