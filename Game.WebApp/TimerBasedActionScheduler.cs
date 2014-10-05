using System;
using Game.WebApp.Controller;

namespace Game.WebApp
{
    public class TimerBasedActionScheduler : IActionScheduler
    {
        public TimeSpan? TimeTillNextEvent { get; private set; }

        public void ScheduleEvent(Action action, TimeSpan relativePeriod)
        {
            throw new NotImplementedException();
        }
    }
}