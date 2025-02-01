using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public static class EventAttributeExtensions
    {
        /// <summary>
        /// Gets the app event types from an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The app event types</returns>
        public static IEnumerable<Type> GetAppEventTypes(this Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAppEventType())
                {
                    yield return type;
                }
            }
        }
        /// <summary>
        /// Whether a type is an app event type or not, denoted by a class with the Event attribute
        /// set on it
        /// </summary>
        /// <param name="checkType">The type to check</param>
        /// <returns>True if the check typ eis an app event, false otherwise</returns>
        public static bool IsAppEventType(this Type checkType)
        {
            return checkType != null && checkType.GetCustomAttributes(typeof(EventAttribute), true).Length > 0;
        }
    }
}
