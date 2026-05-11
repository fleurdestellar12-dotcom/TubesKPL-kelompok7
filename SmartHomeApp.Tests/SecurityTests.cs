using System.Diagnostics;
using Xunit;
using SmartHomeApp.Devices;

public class SecurityTests
{
    [Fact]
    public void TestSecurityModule()
    {
        var alarm = new SecurityAlarm();

        [cite_start]// Performance Testing menggunakan Stopwatch [cite: 11]
        Stopwatch sw = Stopwatch.StartNew();

        alarm.ChangeState(AlarmState.ArmedAway, "1234");

        sw.Stop();

        [cite_start]// Unit Testing Assert [cite: 10]
        Assert.Equal(AlarmState.ArmedAway, alarm.CurrentState);
        Debug.WriteLine($"Kecepatan eksekusi: {sw.ElapsedMilliseconds} ms");
    }
}