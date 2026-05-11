using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // WAJIB ADA UNTUK REGEX
using Newtonsoft.Json;

namespace SmartHomeApp.Managers
{
    public class AutomationEngine
    {
        private readonly Dictionary<string, Action> _scheduleTable = new Dictionary<string, Action>();

        public void AddSchedule(string time, Action action)
        {
            // VALIDASI FORMAT HH:mm (00:00 - 23:59)
            // Ini yang bikin Unit Test kamu PASS nantinya
            if (!Regex.IsMatch(time, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                throw new ArgumentException("Contract Failed: Format waktu harus HH:mm (00:00 - 23:59).");
            }

            if (action == null) throw new ArgumentNullException(nameof(action));

            _scheduleTable[time] = action;
        }

        public void ExecuteSchedule(string currentTime)
        {
            Console.WriteLine($"\n[Automation Engine] Mengecek jadwal jam: {currentTime}");
            if (_scheduleTable.TryGetValue(currentTime, out var actionToExecute))
            {
                actionToExecute.Invoke();
            }
            else
            {
                Console.WriteLine("-> Tidak ada jadwal.");
            }
        }
    }
}