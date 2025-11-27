using Microsoft.Extensions.Logging;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public class EventOne : IEvent
    {
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
