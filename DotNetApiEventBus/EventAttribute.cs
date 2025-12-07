using System.Diagnostics.CodeAnalysis;

namespace DotNetApiEventBus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventAttribute : Attribute
    {
        public EventAttribute() : base() { }
    }
}
