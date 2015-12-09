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
            serviceProvider.SetMainWindowViewModel(mainWindowViewModelMock);

            controller = ApplicationController.GetInstance(serviceProvider);
        }

        [Test]
        public void ShouldInitializeWithGivenServiceProviderWhenGetInstance()
        {
            mainWindowViewModelMock.Received().Configure(Arg.Any<IConfigManager>());
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

            controller.SendMessage(changeConfigurationMessage);
            mainWindowViewModelMock.Received().Configure(configManagerMock);
        }
    }
}
