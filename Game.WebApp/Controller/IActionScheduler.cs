using System;

namespace Game.WebApp.Controller
{
    public interface IActionScheduler
    {
        TimeSpan? TimeTillNextEvent { get; }
        void ScheduleEvent(Action action, TimeSpan relativePeriod);
    }
}