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
            var production = ServiceProvider.GetInstance(ServiceProviderType.Production);
            Assert.AreEqual(typeof(ProductionServiceProvider), production.GetType());
            var test = ServiceProvider.GetInstance(ServiceProviderType.Test);
            Assert.AreEqual(typeof(TestServiceProvider), test.GetType());
        }

        [Test]
        public void ShouldReturnProductionServiceProviderByDefault()
        {
            var provider = ServiceProvider.GetInstance();
            Assert.AreEqual(typeof(ProductionServiceProvider), provider.GetType());
        }

        [Test]
        public void TestServiceProviderShouldReturnServiceToBeSet()
        {
            TestServiceProvider testServiceProvider = (TestServiceProvider)ServiceProvider.GetInstance(ServiceProviderType.Test);
            IMainWindowViewModel mainWindowViewModelMock = Substitute.For<IMainWindowViewModel>();
            testServiceProvider.SetMainWindowViewModel(mainWindowViewModelMock);

            Assert.AreEqual(mainWindowViewModelMock, testServiceProvider.GetMainWindowViewModel());
        }
    }
}
