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
    public class ServiceProviderTest
    {
        [Test]
        public void ShouldReturnCorrectServiceProvider()
        {
            var applicationController = Substitute.For<IApplicationController>();
            var production = ServiceProvider.Create(applicationController, ServiceType.Production);
            Assert.AreEqual(typeof(ProductionServiceProvider), production.GetType());
            var test = ServiceProvider.Create(applicationController, ServiceType.Test);
            Assert.AreEqual(typeof(TestServiceProvider), test.GetType());
        }

        [Test]
        public void TestServiceProviderShouldReturnServiceToBeSet()
        {
            var applicationController = Substitute.For<IApplicationController>();
            TestServiceProvider testServiceProvider = (TestServiceProvider)ServiceProvider.Create(applicationController, ServiceType.Test);
            IMainWindowViewModel mainWindowViewModelMock = Substitute.For<IMainWindowViewModel>();
            testServiceProvider.SetMainWindowViewModel(mainWindowViewModelMock);
            Assert.AreEqual(mainWindowViewModelMock, testServiceProvider.GetMainWindowViewModel());

            IConfigWindowViewModel configWindowViewModelMock = Substitute.For<IConfigWindowViewModel>();
            testServiceProvider.SetConfigWindowViewModel(configWindowViewModelMock);
            Assert.AreEqual(configWindowViewModelMock, testServiceProvider.GetConfigWindowViewModel());
        }
    }
}
