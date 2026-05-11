using System;
using System.Threading.Tasks;

namespace SmartHomeApp.Services
{
    public class WeatherApi
    {
        public async Task<string> GetCurrentWeatherAsync()
        {
            Console.WriteLine("Memanggil API Cuaca...");
            await Task.Delay(1000); 
            return "Rain"; 
        }
    }
}