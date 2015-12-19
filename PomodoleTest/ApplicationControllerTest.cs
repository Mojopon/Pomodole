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
        private IConfigManager configManagerMock;
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
            var newController = ApplicationController.Create();

            serviceProvider = (TestServiceProvider)ServiceProvider.Create(controller, ServiceType.Test);

            configManagerMock = Substitute.For<IConfigManager>();
            configManagerMock.Subject.Returns(new Action<IApplicationMessage>((IApplicationMessage message) => message.Execute(configManagerMock)));
            serviceProvider.SetConfigManager(configManagerMock);

            mainWindowViewModelMock = Substitute.For<IMainWindowViewModel>();
            mainWindowViewModelMock.Subject
                                   .Returns(new Action<IApplicationMessage>((IApplicationMessage message) => message.Execute(mainWindowViewModelMock)));
            serviceProvider.SetMainWindowViewModel(mainWindowViewModelMock);
        
            configWindowViewModelMock = Substitute.For<IConfigWindowViewModel>();
            configWindowViewModelMock.Subject
                                     .Returns(new Action<IApplicationMessage>((IApplicationMessage message) => message.Execute(configWindowViewModelMock)));
            serviceProvider.SetConfigWindowViewModel(configWindowViewModelMock);

            newController.Initialize(serviceProvider);
            controller = newController;
        }

        [Test]
        public void ShouldSendApplicationMessageToAllViewModels()
        {
            var sendApplicationMessageTest = new SendApplicationMessageTest();
            
            controller.Trigger(sendApplicationMessageTest);
            Assert.IsTrue(sendApplicationMessageTest.HasExecutedTo(configManagerMock));
            Assert.IsTrue(sendApplicationMessageTest.HasExecutedTo(mainWindowViewModelMock));
            Assert.IsTrue(sendApplicationMessageTest.HasExecutedTo(configWindowViewModelMock));
        }

        [Test]
        public void ShouldLoadConfigurationFileAtInitialize()
        {
            ApplicationController.ResetInstance();
            configManagerMock.ClearReceivedCalls();
            mainWindowViewModelMock.ClearReceivedCalls();
            configManagerMock.DidNotReceive().LoadConfigurationFromFile();
            configManagerMock.TaskTime.Returns(20);
            configManagerMock.BreakTime.Returns(5);
            configManagerMock.RepeatTime.Returns(2);
            configManagerMock.LongBreakTime.Returns(15);
            int taskTime = -1;
            int breakTime = -1;
            int repeatTime = -1;
            int longBreakTime = -1;
            mainWindowViewModelMock.Configure(Arg.Do<IConfigManager>(c =>
            {
                taskTime = c.TaskTime;
                breakTime = c.BreakTime;
                repeatTime = c.RepeatTime;
                longBreakTime = c.LongBreakTime;
            }));
            var newController = ApplicationController.Create();
            mainWindowViewModelMock.DidNotReceive().Configure(configManagerMock);
            newController.Initialize(serviceProvider);
            configManagerMock.Received().LoadConfigurationFromFile();
            mainWindowViewModelMock.Received().Configure(Arg.Any<IConfigManager>());
            Assert.AreEqual(20, taskTime);
            Assert.AreEqual(5, breakTime);
            Assert.AreEqual(2, repeatTime);
            Assert.AreEqual(15, longBreakTime);
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
        public void ShouldShowConfigWindow()
        {
            var configWindowMock = Substitute.For<IConfigWindow>();
            serviceProvider.SetView(ViewFor.ConfigWindow, configWindowMock);

            configWindowMock.DidNotReceive().Show();
            controller.Show(ViewFor.ConfigWindow);
            configWindowMock.Received().Show();
        }
    }
}
