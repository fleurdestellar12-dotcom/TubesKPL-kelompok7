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
            // Arrange
            var ac = new SmartAC();

            // Act
            ac.TurnOn();

            // Assert
            Assert.Equal(ACState.Cooling, ac.CurrentState);
        }

        [Fact]
        public void SetTemperature_WhenOff_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var ac = new SmartAC(); // Default state is Off

            // Act & Assert (DbC Testing)
            var exception = Assert.Throws<InvalidOperationException>(() => ac.SetTemperature(24));
            Assert.Contains("Tidak bisa mengatur suhu saat AC mati", exception.Message);
        }

        [Fact]
        public void SetTemperature_BelowMinimum_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var ac = new SmartAC();
            ac.TurnOn();

            // Act & Assert (DbC Testing)
            Assert.Throws<ArgumentOutOfRangeException>(() => ac.SetTemperature(10));
        }
    }
}