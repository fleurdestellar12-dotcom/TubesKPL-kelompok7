using System;
using System.Collections.Generic;

namespace SmartHomeApp.Devices
{
    public class SmartLight
    {
        public bool IsOn { get; private set; } = false;
        public string CurrentColor { get; private set; } = "Mati";

        // Implementasi Table-Driven Construction
        private readonly Dictionary<string, string> weatherColorMap = new Dictionary<string, string>
        {
            { "Cerah Siang", "Putih Terang" },
            { "Cerah Malam", "Kuning Hangat" },
            { "Hujan Siang", "Putih Redup" },
            { "Hujan Malam", "Biru Redup" },
            { "Mendung Siang", "Putih Netral" },
            { "Mendung Malam", "Kuning Redup" }
        };

        public void TurnOn()
        {
            IsOn = true;
            CurrentColor = "Standby";
        }

        public void TurnOff()
        {
            IsOn = false;
            CurrentColor = "Mati";
        }

        // Pemrosesan logika ditarik ke dalam Backend (Clean Code)
        public void AdjustColorByRandomWeather(out string skenario, out string warna)
        {
            // Design by Contract (DbC): Tolak aksi jika lampu belum menyala
            if (!IsOn)
            {
                throw new InvalidOperationException("Lampu belum dinyalakan! Nyalakan terlebih dahulu.");
            }

            string[] cuaca = { "Cerah", "Hujan", "Mendung" };
            string[] waktu = { "Siang", "Malam" };

            Random rnd = new Random();
            string c = cuaca[rnd.Next(cuaca.Length)];
            string w = waktu[rnd.Next(waktu.Length)];
            
            // Format skenario yang terpilih
            skenario = $"{c} {w}";

            // Table-Driven Logic
            if (weatherColorMap.TryGetValue(skenario, out string outputWarna))
            {
                warna = outputWarna;
                CurrentColor = outputWarna;
            }
            else
            {
                warna = "Tidak Diketahui";
                CurrentColor = "Default";
            }
        }
    }
}