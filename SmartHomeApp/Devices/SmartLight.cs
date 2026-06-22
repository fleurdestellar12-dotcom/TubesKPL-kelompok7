using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHomeApp.Devices
{
    public class SmartLight
    {
        public bool IsOn { get; private set; } = false;
        public string CurrentColor { get; private set; } = "Mati";

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

        public async Task<string> FetchWeatherFromAPI()
        {
            using (HttpClient client = new HttpClient())
            {
                // PERBAIKAN: Menambahkan &timezone=auto di akhir URL
                string url = "https://api.open-meteo.com/v1/forecast?latitude=-6.9744&longitude=107.6303&current_weather=true&timezone=auto";
                
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement currentWeather = root.GetProperty("current_weather");
                    
                    int weatherCode = currentWeather.GetProperty("weathercode").GetInt32();
                    string timeStr = currentWeather.GetProperty("time").GetString(); 
                    
                    // Sekarang jam yang diambil sudah sesuai dengan Waktu Indonesia Barat (WIB)
                    int hour = DateTime.Parse(timeStr).Hour;
                    string waktu = (hour >= 6 && hour < 18) ? "Siang" : "Malam";

                    string cuaca = "Cerah";
                    if (weatherCode >= 51 && weatherCode <= 67) cuaca = "Hujan"; 
                    else if (weatherCode == 1 || weatherCode == 2 || weatherCode == 3) cuaca = "Mendung";

                    return $"{cuaca} {waktu}";
                }
            }
        }

        public void AdjustColorByWeatherAPI(string skenario, out string warna)
        {
            if (!IsOn)
            {
                throw new InvalidOperationException("Lampu belum dinyalakan! Nyalakan terlebih dahulu.");
            }

            if (weatherColorMap.TryGetValue(skenario, out string outputWarna))
            {
                warna = outputWarna;
                CurrentColor = outputWarna;
            }
            else
            {
                warna = "Putih Netral";
                CurrentColor = "Default";
            }
        }
    }
}