﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pomodole;

namespace PomodoleTest
{
    [TestFixture]
    public class PomodoroTest
    {
        private Pomodoro pomodoro;

        [SetUp]
        public void CreatePomodoro()
        {
            pomodoro = new Pomodoro();

            int task = 2;
            int shortBreak = 1;
            int repeat = 2;
            int longBreak = 3;

            var pomodoroConfig = new PomodoroConfig(task, shortBreak, repeat, longBreak);

            pomodoro.ConfigurePomodoroRelatives(pomodoroConfig);
        }

        [Test]
        public void ShouldSwitchTaskCountdownAndBreakCountdown()
        {
            Assert.AreEqual(2, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.AreEqual(PomodoroPhase.NotRunning, pomodoro.CurrentPhase);
            pomodoro.Tick();
            Assert.AreEqual(1, pomodoro.GetMinute());
            Assert.AreEqual(59, pomodoro.GetSecond());
            Assert.AreEqual(PomodoroPhase.RunningTask, pomodoro.CurrentPhase);

            for (int i = 0; i < 119; i++)
            {
                pomodoro.Tick();
            }

            bool onSwitchToBreakEventCalled = false;
            pomodoro.OnSwitchToBreak += new Action(() => onSwitchToBreakEventCalled = true);
            pomodoro.Tick();
            Assert.IsTrue(onSwitchToBreakEventCalled);
            Assert.IsFalse(pomodoro.CountdownEnd);
            Assert.AreEqual(1, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.AreEqual(PomodoroPhase.WaitingSwitchToBreak, pomodoro.CurrentPhase);
            Assert.AreEqual(1, pomodoro.Progress);

            bool onSwitchToTaskEventCalled = false;
            pomodoro.OnSwitchToTask += new Action(() => onSwitchToTaskEventCalled = true);

            for (int i = 0; i < 60; i++)
            {
                pomodoro.Tick();
                Assert.AreEqual(PomodoroPhase.RunningBreak, pomodoro.CurrentPhase);
            }
            pomodoro.Tick();
            Assert.IsTrue(onSwitchToTaskEventCalled);
            Assert.IsFalse(pomodoro.CountdownEnd);
            Assert.AreEqual(2, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.AreEqual(PomodoroPhase.WaitingSwitchToTask, pomodoro.CurrentPhase);
            Assert.AreEqual(1, pomodoro.Progress);
        }
        [Test]
        public void ShouldSwitchTaskToLongBreakWhenRepeatEnd()
        {
            bool onSwitchToLongBreakEventCalled = false;
            pomodoro.OnSwitchToLongBreak += new Action(() => onSwitchToLongBreakEventCalled = true);
            bool onCompletePomodoroEventCalled = false;
            pomodoro.OnCompletePomodoro += new Action(() => onCompletePomodoroEventCalled = true);

            Assert.AreEqual(2, pomodoro.GetRepeatTimeLeft());
            Assert.IsFalse(onSwitchToLongBreakEventCalled);
            Assert.IsFalse(onCompletePomodoroEventCalled);
            for (int j = 0; j < 2; j++)
            {
                // Timer should tick for one second when started so we need one more second for every spans
                for (int i = 0; i < 121; i++)
                {
                    pomodoro.Tick();
                }
                Assert.AreEqual(1, pomodoro.GetMinute());
                Assert.AreEqual(0, pomodoro.GetSecond());
                Assert.IsFalse(onSwitchToLongBreakEventCalled);
                Assert.IsFalse(onCompletePomodoroEventCalled);
                for (int i = 0; i < 61; i++)
                {
                    pomodoro.Tick();
                }
                Assert.AreEqual(2, pomodoro.GetMinute());
                Assert.AreEqual(0, pomodoro.GetSecond());
                Assert.IsFalse(onSwitchToLongBreakEventCalled);
                Assert.IsFalse(onCompletePomodoroEventCalled);
            }

            Assert.AreEqual(0, pomodoro.GetRepeatTimeLeft());
            Assert.AreEqual(2, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.IsFalse(onSwitchToLongBreakEventCalled);
            Assert.IsFalse(onCompletePomodoroEventCalled);
            for (int i = 0; i < 121; i++)
            {
                pomodoro.Tick();
            }
            Assert.AreEqual(3, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.AreEqual(PomodoroPhase.WaitingSwitchToLongBreak, pomodoro.CurrentPhase);
            Assert.AreEqual(1, pomodoro.Progress);
            Assert.IsTrue(onSwitchToLongBreakEventCalled);
            Assert.IsFalse(onCompletePomodoroEventCalled);
            for (int i = 0; i < 180; i++)
            {
                pomodoro.Tick();
                Assert.AreEqual(PomodoroPhase.RunningLongBreak, pomodoro.CurrentPhase);
            }
            pomodoro.Tick();
            Assert.AreEqual(2, pomodoro.GetMinute());
            Assert.AreEqual(0, pomodoro.GetSecond());
            Assert.IsTrue(onCompletePomodoroEventCalled);
            Assert.AreEqual(PomodoroPhase.Completed, pomodoro.CurrentPhase);
            Assert.AreEqual(1, pomodoro.Progress);
        }
    }
}
