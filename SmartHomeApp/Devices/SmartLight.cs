using System;
using System.Collections.Generic;

namespace SmartHomeApp.Devices
{
    public class SmartLight
    {
        public bool IsOn { get; private set; }
        public string CurrentColor { get; private set; }
        
        private readonly Dictionary<string, string> _weatherColorMatrix;

        public SmartLight()
        {
            IsOn = false;
            CurrentColor = "Putih Netral";
            _weatherColorMatrix = new Dictionary<string, string>
            {
                { "Clear", "Putih Terang" },
                { "Rain", "Kuning Redup" },
                { "Clouds", "Biru Sejuk" }
            };
        }

        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine("Lampu Pintar dinyalakan.");
        }

        public void AdjustColorByWeather(string weatherCondition)
        {
            if (!IsOn) 
                throw new InvalidOperationException("Contract Failed: Lampu masih mati!");
            
            if (!_weatherColorMatrix.ContainsKey(weatherCondition))
                throw new ArgumentException("Contract Failed: Cuaca tidak dikenal");

            CurrentColor = _weatherColorMatrix[weatherCondition];
            Console.WriteLine($"[Otomatisasi Cuaca] {weatherCondition} -> Warna menjadi: {CurrentColor}");
        }
    }
}