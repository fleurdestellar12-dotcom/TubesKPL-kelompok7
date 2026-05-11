using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLog; 

namespace SmartHomeApp.Managers
{
    public class AutomationEngine
    {
        
        private readonly Dictionary<string, Action> _scheduleTable = new Dictionary<string, Action>();

        
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        
        public void AddSchedule(string time, Action action)
        {
            
            if (!Regex.IsMatch(time, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                _logger.Error($"Gagal menambah jadwal: Format waktu salah ({time})");
                throw new ArgumentException("Contract Failed: Format waktu harus HH:mm (00:00 - 23:59).");
            }

            _scheduleTable[time] = action ?? throw new ArgumentNullException(nameof(action));
            _logger.Info($"Jadwal otomatis ditambahkan untuk jam: {time}");
        }

        public void ExecuteSchedule(string currentTime)
        {
            Console.WriteLine($"\n[Automation Engine] Mengecek jam: {currentTime}...");

            
            if (_scheduleTable.TryGetValue(currentTime, out Action actionToExecute))
            {
                _logger.Info($"Menjalankan aksi otomatis jam {currentTime}");
                actionToExecute.Invoke();
            }
            else
            {
                Console.WriteLine("-> Tidak ada jadwal terdaftar.");
            }
        }
    }
}