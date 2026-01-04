namespace DotNetApiEventBus
{
    public interface IEventBusConfig
    {
        string ConnectionString { get; }
        IDddConfig DddConfig { get; }
        string Host { get; }
        string Password { get; }
        int Port { get; }
        string QueueName { get; }
        string Username { get; }

        static abstract void ConfigureEnvironmentVariables(string eventBusHostName, string eventBusUsername, string eventBusPassword, int eventBusPort);
    }
}