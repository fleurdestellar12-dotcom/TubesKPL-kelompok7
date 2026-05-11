using System;
using Xunit;
using SmartHomeApp.Managers;
using SmartHomeApp.Devices; // Meminjam SmartAC buatan Radit untuk tes

namespace SmartHomeApp.Tests
{
    public class DeviceManagerTests
    {
        [Fact]
        public void AddDevice_ValidInput_ShouldIncreaseCount()
        {
            // Arrange
            var hub = new DeviceManager<SmartAC>();
            var ac = new SmartAC();

            // Act
            hub.AddDevice("TEST-AC", ac);

            // Assert
            Assert.Equal(1, hub.GetDeviceCount());
        }

        [Fact]
        public void AddDevice_DuplicateId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var hub = new DeviceManager<SmartAC>();
            var ac1 = new SmartAC();
            var ac2 = new SmartAC();
            hub.AddDevice("ID-SAMA", ac1);

            // Act & Assert (Tes DbC)
            var ex = Assert.Throws<InvalidOperationException>(() => hub.AddDevice("ID-SAMA", ac2));
            Assert.Contains("sudah terdaftar", ex.Message);
        }

       [Fact]
        public void AddDevice_NullDevice_ShouldThrowArgumentNullException()
        {
            // Arrange
            var hub = new DeviceManager<SmartAC>();

            // Act & Assert (Tes DbC)
            // Tambahkan tanda '!' setelah 'null'
            Assert.Throws<ArgumentNullException>(() => hub.AddDevice("TEST-NULL", null!)); 
        }
    }
}