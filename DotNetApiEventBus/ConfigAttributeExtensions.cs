using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DotNetApiEventBusCore
{
    public static class ConfigAttributeExtensions
    {
        /// <summary>
        /// Gets the key name associated with the enum value.
        /// </summary>
        /// <typeparam name="ValueType">The data type of the value for this key</typeparam>
        /// <param name="enumValue">The enumeration to do the lookup for</param>
        /// <returns>The key name</returns>
        /// <exception cref="InvalidOperationException">Thrown if the config attribute is not found</exception>
        public static string GetKey<ValueType>(this Enum enumValue)
        {
            var attr = enumValue.GetEnvVar<ValueType>();
            if (attr == null)
            {
                throw new InvalidOperationException($"The enum value {enumValue} does not have an associated EnvVarAttribute.");
            }
            return attr.Name;
        }

        /// <summary>
        /// Retrieves the value associated with the specified enumeration key from the configuration manager, throwing
        /// an exception if the value is not set.
        /// </summary>
        /// <typeparam name="ValueType">The type of the value to retrieve from the configuration.</typeparam>
        /// <param name="enumValue">The enumeration value that identifies the configuration key.</param>
        /// <param name="configurationManager">The configuration manager used to retrieve the value.</param>
        /// <returns>The value of type <typeparamref name="ValueType"/> associated with the specified enumeration key.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the configuration value for the specified key is not set.</exception>
        public static ValueType GetRequiredValue<ValueType>(this Enum enumValue, IConfiguration configurationManager)
        {
            string key = enumValue.GetKey<ValueType>();
            return GetValue<ValueType>(enumValue, configurationManager) ?? throw new InvalidOperationException($"The environment variable {key} is not set.");
        }

        /// <summary>
        /// Retrieves the value of the environment variable associated with the specified enumeration value from the
        /// configuration manager, or returns its default value if not set.
        /// </summary>
        /// <typeparam name="EnvVarType">The type of the environment variable value to retrieve.</typeparam>
        /// <param name="enumValue">The enumeration value representing the environment variable key.</param>
        /// <param name="configurationManager">The configuration manager used to retrieve the environment variable value.</param>
        /// <returns>The value of the environment variable if it is set in the configuration manager; otherwise, the default
        /// value specified by the associated attribute. Returns null if no value is set and no default is defined.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the environment variable associated with the specified enumeration value is not set and no default
        /// value is defined.</exception>
        public static EnvVarType? GetValue<EnvVarType>(this Enum enumValue, IConfiguration configurationManager)
        {
            string key = enumValue.GetKey<EnvVarType>();
            ConfigAttribute<EnvVarType> attribute = enumValue.GetEnvVar<EnvVarType>() ?? throw new InvalidOperationException($"The environment variable {key} is not set.");
            return configurationManager.GetValue<EnvVarType>(key) ?? attribute.DefaultValue;
        }

        /// <summary>
        /// Retrieves the configuration attribute of the specified type applied to an enumeration value, if present.
        /// </summary>
        /// <remarks>This method is an extension method for enumeration values. It returns the first
        /// ConfigAttribute of the specified type applied to the enum field, or null if no such attribute is
        /// found.</remarks>
        /// <typeparam name="EnvVarType">The type of the configuration attribute to retrieve from the enumeration value.</typeparam>
        /// <param name="enumValue">The enumeration value from which to retrieve the configuration attribute.</param>
        /// <returns>A ConfigAttribute of type EnvVarType if the attribute is applied to the specified enumeration value;
        /// otherwise, null.</returns>
        public static ConfigAttribute<EnvVarType>? GetEnvVar<EnvVarType>(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);

            if (name == null)
                return null;

            var field = type.GetField(name);
            return field?.GetCustomAttribute<ConfigAttribute<EnvVarType>>();
        }
    }
}
