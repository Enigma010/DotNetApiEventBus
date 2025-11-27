using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventAttribute : Attribute
    {
        public EventAttribute() : base() { }
    }
}
