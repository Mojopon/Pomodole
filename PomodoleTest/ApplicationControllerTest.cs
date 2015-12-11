using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Pomodole;
using System;

namespace PomodoleTest
{
    [TestFixture]
    public class ApplicationControllerTest
    {
        private TestServiceProvider serviceProvider;
        private IMainWindowViewModel mainWindowViewModelMock;
        private IConfigWindowViewModel configWindowViewModelMock;
        private IApplicationController controller;
        [SetUp]
        public void SetUpApplicationController()
        {
            ApplicationController.ResetInstance();
            Initialize();
        }

        void Initialize()
        {

            serviceProvider = (TestServiceProvider)ServiceProvider.GetInstance(ServiceProviderType.Test);
            mainWindowViewModelMock = Substitute.For<IMainWindowViewModel>();
            configWindowViewModelMock = Substitute.For<IConfigWindowViewModel>();
            serviceProvider.SetMainWindowViewModel(mainWindowViewModelMock);
            serviceProvider.SetConfigWindowViewModel(configWindowViewModelMock);

            controller = ApplicationController.GetInstance(serviceProvider);
        }

        [Test]
        public void ShouldReturnViewModel()
        {
            var mainWindowViewModel = controller.GetViewModel(ViewModelFor.MainWindow);
            Assert.AreEqual(mainWindowViewModelMock, mainWindowViewModel);
            var configWindowViewModel = controller.GetViewModel(ViewModelFor.ConfigWindow);
            Assert.AreEqual(configWindowViewModelMock, configWindowViewModel);

        }

        [Test]
        public void ShouldDelegateSendMessageEvent()
        {
            mainWindowViewModelMock.Received().SendMessage += Arg.Any<Action<IApplicationMessage>>();
            configWindowViewModelMock.Received().SendMessage += Arg.Any<Action<IApplicationMessage>>();
        }

        [Test]
        public void ShouldSendApplicationMessageToAllViewModels()
        {
            var sendApplicationMessageTest = new SendApplicationMessageTest();
            
            controller.SendMessage(sendApplicationMessageTest);
            Assert.IsTrue(sendApplicationMessageTest.HasExecutedTo(mainWindowViewModelMock));
        }

        internal class SendApplicationMessageTest : IApplicationMessage
        {
            private List<IViewModel> messageTargets = new List<IViewModel>();

            public void Execute(IViewModel target)
            {
                if(!messageTargets.Contains(target))
                    messageTargets.Add(target);
            }

            public bool HasExecutedTo(IViewModel target)
            {
                return messageTargets.Contains(target);
            }
        }

        [Test]
        public void ShouldChangeConfiguration()
        {
            IConfigManager configManagerMock = Substitute.For<IConfigManager>();
            var changeConfigurationMessage = new ChangeConfigurationMessage(configManagerMock);

            mainWindowViewModelMock.DidNotReceive().Configure(configManagerMock);
            controller.SendMessage(changeConfigurationMessage);
            mainWindowViewModelMock.Received().Configure(configManagerMock);
        }

        [Test]
        public void ShouldExecuteOpenConfigWindow()
        {
            var openConfigWindowApplicationMessage = new OpenConfigWindowApplicationMessage();

            configWindowViewModelMock.DidNotReceive().Open();
            controller.SendMessage(openConfigWindowApplicationMessage);
            configWindowViewModelMock.Received().Open();
        }
    }
}
