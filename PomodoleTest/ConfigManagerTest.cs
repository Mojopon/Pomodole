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
        private IConfigurationFileManagementSystem configurationFileManager;

        private int taskTime;
        private int breakTime;
        private int repeatTime;
        private int longBreakTime;
        [SetUp]
        public void SetupTest()
        {
            taskTime = -1;
            breakTime = -1;
            repeatTime = -1;
            longBreakTime = -1;

            App.ServiceType = ServiceType.Test;
            configManager = new ConfigManager();
            ConfigurationFileManagementSystem.ResetInstance();
            configurationFileManager = Substitute.For<IConfigurationFileManagementSystem>();
            ConfigurationFileManagementSystem.SetInstance(configurationFileManager);
        }

        [Test]
        public void ShouldConfigurePomodoro()
        {
            var pomodoroMock = Substitute.For<IPomodoro>();
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

        [Test]
        public void ShouldSaveConfigurations()
        {
            configManager.SetupPomodoroConfig(20, 3, 2, 8);

            configurationFileManager.Save(Arg.Do((ConfigManager.Configurations c) => {
                taskTime = c.PomodoroConfig.TaskTime;
                breakTime = c.PomodoroConfig.BreakTime;
                repeatTime = c.PomodoroConfig.RepeatTime;
                longBreakTime = c.PomodoroConfig.LongBreakTime;
            }), App.ConfigurationFileName);
            configManager.SaveConfigurationToFile();
            configurationFileManager.Received().Save(Arg.Any<ConfigManager.Configurations>(), App.ConfigurationFileName);

            Assert.AreEqual(20, taskTime);
            Assert.AreEqual(3, breakTime);
            Assert.AreEqual(2, repeatTime);
            Assert.AreEqual(8, longBreakTime);
        }

        [Test]
        public void ShouldReceiveApplicationMessage()
        {
            configManager.SetupPomodoroConfig(15, 2, 1, 10);

            configurationFileManager.Save(Arg.Do((ConfigManager.Configurations c) => {
                taskTime = c.PomodoroConfig.TaskTime;
                breakTime = c.PomodoroConfig.BreakTime;
                repeatTime = c.PomodoroConfig.RepeatTime;
                longBreakTime = c.PomodoroConfig.LongBreakTime;
            }), App.ConfigurationFileName);

            var messenger = new ApplicationMessageEvent();
            messenger.Register(configManager);

            configurationFileManager.DidNotReceive().Save(Arg.Any<ConfigManager.Configurations>(), App.ConfigurationFileName);
            messenger.Trigger(new ConfigurationDataManagingMessage(messenger, ConfigurationDataManagingMessage.ActionType.Save));
            configurationFileManager.Received().Save(Arg.Any<ConfigManager.Configurations>(), App.ConfigurationFileName);
            Assert.AreEqual(15, taskTime);
            Assert.AreEqual(2, breakTime);
            Assert.AreEqual(1, repeatTime);
            Assert.AreEqual(10, longBreakTime);

            var loadedPomodoroConfig = new PomodoroConfig();
            loadedPomodoroConfig.TaskTime = 30;
            loadedPomodoroConfig.BreakTime = 10;
            loadedPomodoroConfig.RepeatTime = 5;
            loadedPomodoroConfig.LongBreakTime = 15;
            var configurations = new ConfigManager.Configurations();
            configurations.PomodoroConfig = loadedPomodoroConfig;
            configurationFileManager.Load<ConfigManager.Configurations>(App.ConfigurationFileName).Returns(configurations);

            configurationFileManager.DidNotReceive().Load<ConfigManager.Configurations>(App.ConfigurationFileName);
            messenger.Trigger(new ConfigurationDataManagingMessage(messenger, ConfigurationDataManagingMessage.ActionType.Load));
            configurationFileManager.Received().Load<ConfigManager.Configurations>(App.ConfigurationFileName);
            Assert.AreEqual(loadedPomodoroConfig.TaskTime, configManager.TaskTime);
            Assert.AreEqual(loadedPomodoroConfig.BreakTime, configManager.BreakTime);
            Assert.AreEqual(loadedPomodoroConfig.RepeatTime, configManager.RepeatTime);
            Assert.AreEqual(loadedPomodoroConfig.LongBreakTime, configManager.LongBreakTime);
        }
    }
}
