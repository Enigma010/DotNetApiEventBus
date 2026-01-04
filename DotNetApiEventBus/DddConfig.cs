using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetApiEventBus
{

    /// <summary>
    /// Provides configuration settings for the application's domain and subdomain, including access to configuration
    /// keys and environment variable management.
    /// </summary>
    /// <remarks>Use this class to retrieve and manage domain-related configuration values from a
    /// configuration source or to set environment variables that control application domain behavior. The DddConfig
    /// class centralizes domain and subdomain configuration, supporting both strongly typed access and integration with
    /// environment variables.</remarks>
    public class DddConfig : IDddConfig
    {
        /// <summary>
        /// Represents the key used to identify the application domain in configuration or context collections.
        /// </summary>
        public const string AppDomainKey = "APP_DOMAIN";
        /// <summary>
        /// Represents the configuration key used to retrieve the application's subdomain value.
        /// </summary>
        public const string AppSubDomainKey = "APP_SUBDOMAIN";
        /// <summary>
        /// Specifies the available configuration keys for application domain settings.
        /// </summary>
        /// <remarks>Use the members of this enumeration to reference specific configuration values
        /// related to the application's domain and subdomain. These keys are typically used to retrieve or set
        /// configuration values in a strongly typed manner.</remarks>
        public enum Configs
        {
            [Config<string>(name: AppDomainKey)]
            Domain,
            [Config<string>(name: AppSubDomainKey)]
            Subdomain,
        }
        /// <summary>
        /// Gets or sets the domain associated with the current context.
        /// </summary>
        public string Domain { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the subdomain portion of the host name.
        /// </summary>
        public string SubDomain { get; set; } = string.Empty;
        /// <summary>
        /// Initializes a new instance of the DddConfig class using the specified configuration source.
        /// </summary>
        /// <param name="configuration">The configuration source from which to retrieve required domain and subdomain values. Cannot be null.</param>
        public DddConfig(IConfiguration configuration)
        {
            Domain = Configs.Domain.GetRequiredValue<string>(configuration);
            SubDomain = Configs.Subdomain.GetRequiredValue<string>(configuration);
        }
        /// <summary>
        /// Configures the environment variables for the application domain and subdomain.
        /// </summary>
        /// <remarks>This method sets environment variables that may affect application configuration and
        /// behavior. Changes apply to the current process and any child processes started after the variables are
        /// set.</remarks>
        /// <param name="domain">The value to set for the application's domain environment variable. Cannot be null.</param>
        /// <param name="subDomain">The value to set for the application's subdomain environment variable. Cannot be null.</param>
        public static void ConfigureEnvironmentVariables(string domain, string subDomain)
        {
            Environment.SetEnvironmentVariable(Configs.Domain.GetKey<string>(), domain);
            Environment.SetEnvironmentVariable(Configs.Subdomain.GetKey<string>(), subDomain);
        }
    }
}
