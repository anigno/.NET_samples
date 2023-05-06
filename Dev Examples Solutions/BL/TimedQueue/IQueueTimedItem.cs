using System;

namespace TimedQueue
{
    public interface IQueueTimedItem
    {
        DateTime DequeueTime { get; }
    }
}