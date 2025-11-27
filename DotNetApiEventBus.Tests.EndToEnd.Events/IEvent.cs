using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public interface IEvent
    {
        public Guid Id { get; set; }
        public bool ThrowDuringProcessing { get; set; }
        public int AttemptNumber { get; set; }
        public int SucceedOnAttemptNumber { get; set; }
    }
}
