using System.Diagnostics;
using Xunit;
using SmartHomeApp.Devices;

namespace SmartHomeApp.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void TestSecurityModule()
        {
            var alarm = new SecurityAlarm();

            // Performance Testing menggunakan Stopwatch 
            Stopwatch sw = Stopwatch.StartNew();

            alarm.ChangeState(AlarmState.ArmedAway, "1234");

            sw.Stop();

            // Unit Testing Assert 
            Assert.Equal(AlarmState.ArmedAway, alarm.CurrentState);
            Debug.WriteLine($"Kecepatan eksekusi: {sw.ElapsedMilliseconds} ms");
        }
    }
}