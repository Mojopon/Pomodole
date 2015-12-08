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
    public class ApplicationControllerTest
    {
        private TestServiceProvider serviceProvider;
        private IMainWindowViewModel mainWindowViewModelMock;
        private IApplicationController controller;
        [Test]
        public void ShouldInitializeWithTheServiceProviderWhenGetInstance()
        {
            Initialize();

            mainWindowViewModelMock.Received().Configure(Arg.Any<IConfigManager>());
        }

        void Initialize()
        {
            serviceProvider = (TestServiceProvider)ServiceProvider.GetInstance(ServiceProviderType.Test);
            mainWindowViewModelMock = Substitute.For<IMainWindowViewModel>();
            serviceProvider.SetMainWindowViewModel(mainWindowViewModelMock);
            
            controller = ApplicationController.GetInstance(serviceProvider);
        }
    }
}
