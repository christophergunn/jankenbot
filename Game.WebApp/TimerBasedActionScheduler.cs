using System;
using System.Timers;
using Game.WebApp.Controller;

namespace Game.WebApp
{
    public class TimerBasedActionScheduler : IActionScheduler
    {
        private readonly Timer _timer;
        private Action _action;
        private DateTime? _timerDueAt;

        public TimeSpan? TimeTillNextEvent 
        { 
            get
            {
                return _timerDueAt == null 
                    ? (new TimeSpan?()) 
                    : _timerDueAt.Value.Subtract(DateTime.Now);
            }
        }

        public TimerBasedActionScheduler()
        {
            _timer = new Timer { AutoReset = false, Enabled = false };
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _timerDueAt = null;
            _action();
            _action = null;
        }

        public void ScheduleEvent(Action action, TimeSpan relativePeriod)
        {
            if (action == null) throw new ArgumentNullException("action");

            _action = action;
            _timer.Interval = relativePeriod.TotalMilliseconds;
            _timer.Start();
            _timerDueAt = DateTime.Now.Add(relativePeriod);
        }
    }
}