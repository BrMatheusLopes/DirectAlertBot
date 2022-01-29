using DirectAlertBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DirectAlertBot
{
    public static class SchedulerJob
    {
        private const uint _maxTimerInterval = uint.MaxValue;
        private static readonly List<IJob> _jobs = new List<IJob>();
        private static readonly List<Action> _actions = new List<Action>();
        private static Timer _timer = new Timer(state => Run(), null, Timeout.Infinite, Timeout.Infinite);

        public static void Start()
        {
            Run();
        }

        public static void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public static IEnumerable<IJob> GetAllJobs()
        {
            return _jobs;
        }

        private static void Run()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            try
            {
                if (!_jobs.Any())
                    return;

                var firstJob = _jobs.First();
                var timeUntilTrigger = (firstJob.TriggerTime - DateTime.UtcNow);
                if (timeUntilTrigger <= TimeSpan.Zero)
                {
                    Task.Factory.StartNew(() =>
                    {
                        firstJob.Execute();
                    });
                    RemoveFinishedScheduledJob(firstJob);
                }

                var interval = timeUntilTrigger;
                if (interval <= TimeSpan.Zero)
                {
                    Run();
                    return;
                }
                else
                {
                    if (interval.TotalMilliseconds > _maxTimerInterval)
                        interval = TimeSpan.FromMilliseconds(_maxTimerInterval);

                    _timer.Change(interval, interval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SchedulerJob Unexpected Error: {ex}");
            }
        }

        private static void RemoveFinishedScheduledJob(IJob job)
        {
            _jobs.Remove(job);
        }

        public static void AddJob(IJob job)
        {
            if (job is null)
                throw new ArgumentNullException(nameof(job));

            _jobs.Add(job);
            _jobs.Sort(CompareTriggerTime);
            Run();
        }

        public static void RemoveJob(IJob job)
        {
            _jobs.Remove(job);
            _jobs.Sort(CompareTriggerTime);
            Run();
        }

        private static int CompareTriggerTime(IJob x, IJob y)
        {
            return DateTime.Compare(x.TriggerTime, y.TriggerTime);
        }
    }
}
