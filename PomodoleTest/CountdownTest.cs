using NUnit.Framework;
using Pomodole;

namespace PomodoleTest
{
    [TestFixture]
    public class CountdownTest
    {
        Countdown countdown;
        [SetUp]
        public void CreateCountdown()
        {
            // initialize with start minute
            countdown = new Countdown(10);
        }

        [Test]
        public void ShouldProgressTimeUntilTimerEnd()
        {
            countdown.Tick();
            Assert.AreEqual(59, countdown.GetSecond());
            Assert.AreEqual(9, countdown.GetMinute());

            countdown = new Countdown(1);
            for (int timeLeft = 59; timeLeft > 0; timeLeft--)
            {
                countdown.Tick();
                Assert.AreEqual(timeLeft, countdown.GetSecond());
                Assert.IsFalse(countdown.CountdownEnd);
            }
            countdown.Tick();
            Assert.AreEqual(0, countdown.GetSecond());
            Assert.AreEqual(0, countdown.GetMinute());
            Assert.IsTrue(countdown.CountdownEnd);
        }

        [Test]
        public void ShouldResetTime()
        {
            countdown.Tick();
            Assert.AreEqual(59, countdown.GetSecond());
            Assert.AreEqual(9, countdown.GetMinute());
            countdown.Reset();
            Assert.AreEqual(0, countdown.GetSecond());
            Assert.AreEqual(10, countdown.GetMinute());
        }
    }
}
