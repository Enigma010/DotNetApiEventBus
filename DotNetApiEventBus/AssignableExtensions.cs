using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public static class AssignableExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="givenType"/> taking into account generic definitions
        /// </summary>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }

            return givenType == genericType
              || givenType.MapsToGenericTypeDefinition(genericType)
              || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }
        /// <summary>
        /// Returns if a giveb type has an interface of a specific generic type
        /// </summary>
        /// <param name="givenType">The type to check</param>
        /// <param name="genericType">The generic type</param>
        /// <returns>True if the itnerface maps to a generic type</returns>
        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }
        /// <summary>
        /// Returns if a given type map sto a generic type
        /// </summary>
        /// <param name="givenType">The given type</param>
        /// <param name="genericType">The generic type</param>
        /// <returns>True if the given type is of the generic type</returns>
        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && givenType.IsGenericType
              && givenType.GetGenericTypeDefinition() == genericType;
        }
    }
}
