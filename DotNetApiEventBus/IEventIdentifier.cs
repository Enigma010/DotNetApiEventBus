using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    public interface IEventIdentifier
    {
        public string EventId { get; }
    }
}
