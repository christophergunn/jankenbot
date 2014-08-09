using System;

namespace Game.WebApp.Controller
{
    public interface IActionScheduler
    {
        void ScheduleEvent(Action action, TimeSpan relativePeriod);
    }
}