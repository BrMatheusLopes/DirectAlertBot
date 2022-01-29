using DirectAlertBot.Interfaces;
using System;

namespace DirectAlertBot.Jobs
{
    public abstract class Job : IJob
    {
        protected Job(DateTime triggerTime)
        {
            TriggerTime = triggerTime;
        }

        public DateTime TriggerTime { get; }
        public bool IsFinished =>
            TriggerTime.Subtract(DateTime.UtcNow) <= TimeSpan.Zero;

        public abstract void Execute();
    }
}
