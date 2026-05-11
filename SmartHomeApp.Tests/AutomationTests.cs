using System;
using Xunit;
using SmartHomeApp.Managers;

namespace SmartHomeApp.Tests
{
    public class AutomationTests
    {
        [Fact]
        public void AddSchedule_InvalidTimeFormat_ShouldThrowArgumentException()
        {
            
            var engine = new AutomationEngine();

            
            Assert.Throws<ArgumentException>(() => engine.AddSchedule("jam:salah", () => { }));
        }

        [Fact]
        public void ExecuteSchedule_ValidTime_ShouldInvokeAction()
        {
           
            var engine = new AutomationEngine();
            bool wasExecuted = false;

            engine.AddSchedule("07:00", () => wasExecuted = true);

           
            engine.ExecuteSchedule("07:00");

            
            Assert.True(wasExecuted);
        }
    }
}