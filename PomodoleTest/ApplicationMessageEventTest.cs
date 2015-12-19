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
    public class ApplicationMessageEventTest
    {
        private IApplicationMessageEvent applicationMessageEvent;
        private TestApplicationMessagePublisher publisher;
        private TestApplicationMessageSubscriber subscriber;
        private ApplicationController applicationController;

        [SetUp]
        public void Setup()
        {
            ApplicationController.ResetInstance();
            applicationController = ApplicationController.Create();

            applicationMessageEvent = new ApplicationMessageEvent();
            publisher = new TestApplicationMessagePublisher();
            subscriber = new TestApplicationMessageSubscriber();
            publisher.Register(applicationMessageEvent);
        }

        [Test]
        public void SubscriberShouldReceiveMessageThroughEvent()
        {
            // make sure it doesnt occur error when empty
            applicationMessageEvent.Trigger(null);

            var applicationMessageMock = Substitute.For<IApplicationMessage>();
            bool messageReceived = false;

            subscriber.Subject += ((IApplicationMessage message) =>
            {
                if (message == applicationMessageMock)
                {
                    messageReceived = true;
                }
            });

            applicationMessageEvent.Register(subscriber);

            publisher.Trigger(applicationMessageMock);
            Assert.IsTrue(messageReceived);
        }

        [Test]
        public void ApplicationControllerShouldDelegateToApplicationMessageEvent()
        {
            var testServiceProvider = new TestServiceProvider(applicationController);
            publisher.Register(applicationController);

            var applicationMessageMock = Substitute.For<IApplicationMessage>();
            bool messageReceived = false;

            subscriber.Subject += ((IApplicationMessage message) =>
            {
                if (message == applicationMessageMock)
                {
                    messageReceived = true;
                }
            });

            applicationController.Register(subscriber);

            publisher.Trigger(applicationMessageMock);
            Assert.IsTrue(messageReceived);
        }

        [Test]
        public void ShouldSendActivateWindowMessageToMainWindow()
        {
            var mainWindow = Substitute.For<IMainWindow>();
            mainWindow.Subject.Returns ((IApplicationMessage message) => message.Execute(mainWindow));
            applicationMessageEvent.Register(mainWindow);
            mainWindow.DidNotReceive().ActivateWindow();
            applicationMessageEvent.Trigger(new ActivateMainWindowMessage());
            mainWindow.Received().ActivateWindow();
        }

        public class TestApplicationMessagePublisher : IApplicationMessagePublisher
        {
            public IApplicationMessageEvent Messenger { get; private set; }

            public void Register(IApplicationMessageEvent ev)
            {
                Messenger = ev;
            }

            public void Trigger(IApplicationMessage applicationMessage)
            {
                Messenger.Trigger(applicationMessage);
            }
        }

        public class TestApplicationMessageSubscriber : IApplicationMessageSubscriber
        {
            public Action<IApplicationMessage> Subject { get; set; }
        }
    }
}
