using System;
using Xunit;
using SmartHomeApp.Devices;

namespace SmartHomeApp.Tests
{
    public class SmartLightTests
    {
        [Fact]
        public void AdjustColorByWeather_ValidWeather_ShouldChangeColor()
        {
            var light = new SmartLight();
            light.TurnOn();

            light.AdjustColorByWeather("Rain");

            Assert.Equal("Kuning Redup", light.CurrentColor);
        }

        [Fact]
        public void AdjustColor_WhenOff_ShouldThrowException()
        {
            var light = new SmartLight(); // Kondisi awal mati
            Assert.Throws<InvalidOperationException>(() => light.AdjustColorByWeather("Clear"));
        }
    }
}