using System;
using Xunit;
using SmartHomeApp.Devices;

namespace SmartHomeApp.Tests
{
    public class SmartACTests
    {
        [Fact]
        public void TurnOn_ShouldChangeStateToCooling()
        {
            
            var ac = new SmartAC();

            
            ac.TurnOn();

       
            Assert.Equal(ACState.Cooling, ac.CurrentState);
        }

        [Fact]
        public void SetTemperature_WhenOff_ShouldThrowInvalidOperationException()
        {
            
            var ac = new SmartAC();

            
            var exception = Assert.Throws<InvalidOperationException>(() => ac.SetTemperature(24));
            Assert.Contains("Tidak bisa mengatur suhu saat AC mati", exception.Message);
        }

        [Fact]
        public void SetTemperature_BelowMinimum_ShouldThrowArgumentOutOfRangeException()
        {
            
            var ac = new SmartAC();
            ac.TurnOn();

            
            Assert.Throws<ArgumentOutOfRangeException>(() => ac.SetTemperature(10));
        }
    }
}