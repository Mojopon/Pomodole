using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Pomodole;

namespace PomodoleTest
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        private IPomodoro pomodoro;
        private IMainWindowViewModel mainWindowViewModel;
        [SetUp]
        public void SetUpMainWindowViewModel()
        {
            pomodoro = Substitute.For<IPomodoro>();
            mainWindowViewModel = new MainWindowViewModel(pomodoro);
        }

        [Test]
        public void ShouldConfigurePomodoro()
        {
            var configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);

            mainWindowViewModel.Configure(configManager);
            pomodoro.Received().Configure(configManager);
        }
    }
}
