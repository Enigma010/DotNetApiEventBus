namespace DotNetApiEventBusCore
{
    /// <summary>
    /// Specifies metadata for a configuration field that can be set via an environment variable, including its name and
    /// an optional default value.
    /// </summary>
    /// <remarks>Apply this attribute to a field to associate it with a specific environment variable and,
    /// optionally, a default value to use if the environment variable is not set. This attribute is not inherited and
    /// cannot be applied multiple times to the same field.</remarks>
    /// <typeparam name="ValueType">The type of the value expected from the environment variable. This type must be compatible with the field to
    /// which the attribute is applied.</typeparam>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ConfigAttribute<ValueType> : Attribute
    {
        /// <summary>
        /// Gets the name associated with this instance.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets or sets the default value to use when no explicit value is provided.
        /// </summary>
        public ValueType? DefaultValue { get; set; } = default(ValueType);
        /// <summary>
        /// Initializes a new instance of the ConfigAttribute class with the specified name and optional default value.
        /// </summary>
        /// <param name="name">The name of the configuration attribute. Cannot be null.</param>
        /// <param name="defaultValue">The default value to assign to the attribute if no value is provided. If null, the attribute has no default
        /// value.</param>
        public ConfigAttribute(string name, ValueType? defaultValue = default)
        {
            Name = name;
            DefaultValue = defaultValue;
        }
    }
}
