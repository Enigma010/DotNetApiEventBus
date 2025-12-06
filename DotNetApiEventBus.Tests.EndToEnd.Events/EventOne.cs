using Microsoft.Extensions.Logging;
using DotNetApiEventBus;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public class EventOne : DoaminEventIdentifer, IEvent
    {
        public EventOne()
            : base("DotNetApiEventBus", "One", Guid.NewGuid().ToString())
        {
            Id = Guid.Parse(AggregateId);
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool ThrowDuringProcessing { get; set; } = false;
        public int AttemptNumber { get; set; } = 0;
        public int SucceedOnAttemptNumber { get; set; } = -1;
        public void PreHandleEvent(ILogger logger)
        {
            if(ThrowDuringProcessing && AttemptNumber != SucceedOnAttemptNumber)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
