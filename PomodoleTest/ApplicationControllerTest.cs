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
            mainWindowViewModelMock.Subject
                                   .Returns(new Action<IApplicationMessage>((IApplicationMessage message) => message.Execute(mainWindowViewModelMock)));
            configWindowViewModelMock = Substitute.For<IConfigWindowViewModel>();
            configWindowViewModelMock.Subject
                                     .Returns(new Action<IApplicationMessage>((IApplicationMessage message) => message.Execute(configWindowViewModelMock)));
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
        public void ShouldSendApplicationMessageToAllViewModels()
        {
            var sendApplicationMessageTest = new SendApplicationMessageTest();
            
            controller.Trigger(sendApplicationMessageTest);
            Assert.IsTrue(sendApplicationMessageTest.HasExecutedTo(mainWindowViewModelMock));
        }

        internal class SendApplicationMessageTest : IApplicationMessage
        {
            private List<IApplicationMessageSubscriber> messageTargets = new List<IApplicationMessageSubscriber>();

            public void Execute(object target)
            {
                if(!messageTargets.Contains(target))
                    messageTargets.Add(target as IApplicationMessageSubscriber);
            }

            public bool HasExecutedTo(IApplicationMessageSubscriber target)
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
            controller.Trigger(changeConfigurationMessage);
            mainWindowViewModelMock.Received().Configure(configManagerMock);
        }

        [Test]
        public void ShouldExecuteOpenConfigWindow()
        {
            var openConfigWindowApplicationMessage = new OpenConfigWindowApplicationMessage();

            configWindowViewModelMock.DidNotReceive().Open();
            controller.Trigger(openConfigWindowApplicationMessage);
            configWindowViewModelMock.Received().Open();
        }
    }
}
