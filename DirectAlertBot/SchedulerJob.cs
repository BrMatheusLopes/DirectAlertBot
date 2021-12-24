using DirectAlertBot.Jobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DirectAlertBot
{
    public class SchedulerJob
    {
        private readonly List<IJob> _jobs = new List<IJob>();
        private readonly ILogger<SchedulerJob> _logger;
        private bool _isRunning;

        public SchedulerJob(ILogger<SchedulerJob> logger)
        {
            _logger = logger;
        }

        public async void Start(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("SchedulerJob started...");
            await Run(cancellationToken);
        }

        public void Stop()
        {
            _logger.LogInformation("SchedulerJob stopped...");
            _isRunning = false;
        }

        private async Task Run(CancellationToken cancellationToken)
        {
            if (_isRunning)
                return;

            _isRunning = true;
            while (_isRunning)
            {
                try
                {
                    foreach (IJob job in _jobs)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        TimeSpan timeUntilTrigger = job.TriggerTime - DateTime.Now;
                        if (timeUntilTrigger <= TimeSpan.Zero)
                        {
                            await job.Execute();
                            job.Finished = true;
                        }

                        await Task.Delay(100, cancellationToken);
                    }

                    await RemoveFinishedScheduledJobs();
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError($"SchedulerJob: {ex.Message}");
                    Stop();
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SchedulerJob Unexpected Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        private async Task RemoveFinishedScheduledJobs()
        {
            _jobs.RemoveAll(x => x.Finished);
            await Task.CompletedTask;
        }

        public void AddJob(IJob job)
        {
            _jobs.Add(job);
        }

        public void RemoveJob(IJob job)
        {
            _jobs.Remove(job);
        }
    }
}
