using System;

namespace DirectAlertBot.Interfaces
{
    public interface IJob
    {
        bool IsFinished { get; }
        DateTime TriggerTime { get; }
        void Execute();
    }
}