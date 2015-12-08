using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pomodole;
using NSubstitute;

namespace PomodoleTest
{
    [TestFixture]
    public class ConfigManagerTest
    {
        private ConfigManager configManager;
        [SetUp]
        public void SetupConfigManager()
        {
            configManager = new ConfigManager();
        }

        [Test]
        public void ShouldConfigurePomodoro()
        {
            var pomodoroMock = Substitute.For<IPomodoro>();
            int taskTime = -1;
            int breakTime = -1;
            int repeatTime = -1;
            int longBreakTime = -1;
            pomodoroMock.ConfigurePomodoroRelatives(Arg.Do<IPomodoroConfig>(x => 
            {
                taskTime = x.TaskTime;
                breakTime = x.BreakTime;
                repeatTime = x.RepeatTime;
                longBreakTime = x.LongBreakTime;
            }));

            configManager.SetupPomodoroConfig(25, 5, 3, 15);
            configManager.ExecuteConfigurationFor(pomodoroMock);

            Assert.AreEqual(25, taskTime);
            Assert.AreEqual(5, breakTime);
            Assert.AreEqual(3, repeatTime);
            Assert.AreEqual(15, longBreakTime);
        }
    }
}
