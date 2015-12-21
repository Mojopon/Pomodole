using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Pomodole;
using System.Windows.Input;

namespace PomodoleTest
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        private IApplicationController applicationController;
        private IPomodoro pomodoro;
        private IMainWindowViewModel mainWindowViewModel;
        [SetUp]
        public void SetUpMainWindowViewModel()
        {
            pomodoro = Substitute.For<IPomodoro>();
            applicationController = Substitute.For<IApplicationController>();
            mainWindowViewModel = new MainWindowViewModel(applicationController, pomodoro);
        }

        [Test]
        public void ShouldConfigurePomodoro()
        {
            var configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);

            mainWindowViewModel.Configure(configManager);
            pomodoro.Received().Configure(configManager);
        }

        [Test]
        public void ShouldStopTickingOnConfiguration()
        {
            var configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);
            ICommand startCommand = mainWindowViewModel.MainButtonCommand;
            startCommand.Execute(null);
            Assert.IsTrue(mainWindowViewModel.TimerRunning);

            mainWindowViewModel.Configure(configManager);
            pomodoro.Received().Configure(configManager);
            Assert.IsFalse(mainWindowViewModel.TimerRunning);
        }

        [Test]
        public void ShouldSwitchStartCommandToStopCommand()
        {
            ICommand startCommand = mainWindowViewModel.MainButtonCommand;
            startCommand.Execute(null);
            Assert.IsTrue(mainWindowViewModel.TimerRunning);

            ICommand stopCommand = mainWindowViewModel.MainButtonCommand;
            stopCommand.Execute(null);
            Assert.IsFalse(mainWindowViewModel.TimerRunning);
        }

        [Test]
        public void ShouldSendOpenConfigApplicationMessage()
        {
            ICommand command = mainWindowViewModel.ConfigButtonCommand;
            command.Execute(null);
            applicationController.Received().Trigger(Arg.Any<ToggleConfigWindowApplicationMessage>());
        }
    }
}
