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
    public class ConfigWindowViewModelTest
    {
        IApplicationController applicationController;
        IConfigManager configManager;
        IConfigWindowViewModel viewModel;
        [SetUp]
        public void SetupConfigWindowViewModel()
        {
            applicationController = Substitute.For<IApplicationController>();
            configManager = Substitute.For<IConfigManager>();
            viewModel = new ConfigWindowViewModel(applicationController, configManager);
        }

        [Test]
        public void ShouldSendChangeConfigurationEvent()
        {
            var target = Substitute.For<IMainWindowViewModel>();
            applicationController.Trigger(Arg.Do<ChangeConfigurationMessage>(x => x.Execute(target)));

            ICommand command = viewModel.OkButtonCommand;
            command.Execute(null);
            applicationController.Received().Trigger(Arg.Any<ChangeConfigurationMessage>());
            target.Received().Configure(configManager);
        }
    }
}
