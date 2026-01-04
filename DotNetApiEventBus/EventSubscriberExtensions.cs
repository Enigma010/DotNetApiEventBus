using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DotNetApiEventBus
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public static class EventSubscriberExtensions
    {
        /// <summary>
        /// Gets a list of the event consumer types from an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>A list of classes extending from EventConsumer</returns>
        public static IEnumerable<(Type ConsumerType, Type EventType)> GetSubscriberAndEventTypes(this Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsEventSubscriberType())
                {
                    yield return new(type, type!.BaseType!.GetGenericArguments().First());
                }
            }
        }

        /// <summary>
        /// Returns if the type is and event consumer or sub-class of an event consumer
        /// </summary>
        /// <param name="checkType">The type to check</param>
        /// <returns>True if the type is an event consumer or sub-class, false otherwise</returns>
        public static bool IsEventSubscriberType(this Type checkType)
        {
            return checkType != null && checkType.IsAssignableToGenericType(typeof(EventSubscriber<>));
        }
    }
}
