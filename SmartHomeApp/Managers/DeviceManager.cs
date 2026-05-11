using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SmartHomeApp.Managers
{
	// Generics: <T> memungkinkan manajer ini menyimpan perangkat tipe apapun (AC, Lampu, Alarm)
	public class DeviceManager<T> where T : class
	{
		private readonly Dictionary<string, T> _devices = new Dictionary<string, T>();
		public int MaxDevicesAllowed { get; private set; }

		public DeviceManager()
		{
			// Runtime Configuration: Membaca batas kapasitas perangkat dari JSON
			string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "appsettings.json");
			string jsonString = File.ReadAllText(configPath);
			using JsonDocument doc = JsonDocument.Parse(jsonString);

			var settings = doc.RootElement.GetProperty("HubSettings");
			MaxDevicesAllowed = settings.GetProperty("MaxDevicesAllowed").GetInt32();
		}

		public void AddDevice(string deviceId, T device)
		{
			// DbC (Pre-condition): Validasi input secara ketat
			if (string.IsNullOrWhiteSpace(deviceId))
			{
				throw new ArgumentException("Contract Failed: ID Perangkat tidak boleh kosong.", nameof(deviceId));
			}

			if (device == null)
			{
				throw new ArgumentNullException(nameof(device), "Contract Failed: Objek perangkat tidak valid (null).");
			}

			if (_devices.ContainsKey(deviceId))
			{
				throw new InvalidOperationException($"Contract Failed: Perangkat dengan ID '{deviceId}' sudah terdaftar.");
			}

			if (_devices.Count >= MaxDevicesAllowed)
			{
				throw new InvalidOperationException($"Contract Failed: Kapasitas Hub penuh! Maksimal hanya {MaxDevicesAllowed} perangkat.");
			}

			// Jika semua DbC lolos, tambahkan ke list
			_devices.Add(deviceId, device);
			Console.WriteLine($"[Device Hub] Perangkat '{deviceId}' ({typeof(T).Name}) berhasil didaftarkan.");
		}

		public T GetDevice(string deviceId)
		{
			if (!_devices.ContainsKey(deviceId))
			{
				throw new KeyNotFoundException($"Perangkat dengan ID '{deviceId}' tidak ditemukan.");
			}
			return _devices[deviceId];
		}

		public int GetDeviceCount()
		{
			return _devices.Count;
		}
	}
}