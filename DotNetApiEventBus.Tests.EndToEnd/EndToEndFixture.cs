using System.Diagnostics;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    
    public class EndToEndFixture : IDisposable
    {
        public const string EventBusDockerContainerName = "EventBusEndToEndTests";

        private List<Process> _apiProcesses = new List<Process>();
        private List<Process> _dockerProcesses = new List<Process>();

        /// <summary>
        /// Setting that dictates whether the testing framework will start the APIs
        /// set this to false if you want to manually start the APIs for debugging
        /// purposes
        /// </summary>
        public static bool StartApis = true;
        /// <summary>
        /// Settings that dictates whether to start the related docker infrastructure
        /// i.e. the event bus, set this to false if you want to manually start the
        /// event bus via the lrun.sh command
        /// </summary>
        public static bool StartDocker = true;

        public static int WaitForStartMs = 10000;

        public EndToEndFixture()
        {
            if (StartDocker)
            {
                DockerUtilities.StopInstance(EventBusDockerContainerName);
                DockerUtilities.RemoveInstance(EventBusDockerContainerName);
                StartProcess("docker", $"run --name {EventBusDockerContainerName} -p 5672:5672 -p 8080:15672 rabbitmq:3-management", _dockerProcesses);
                System.Threading.Thread.Sleep(WaitForStartMs);
            }
            if (StartApis)
            {
                StartProcess("dotnet", $"run --project ..\\DotNetApiEventBus.Tests.EndToEnd.Api\\ --configuration Release --no-build", _apiProcesses);
                StartProcess("dotnet", $"run --project ..\\DotNetApiEventBus.Tests.EndToEnd.Api2\\ --configuration Release --no-build", _apiProcesses);
                System.Threading.Thread.Sleep(WaitForStartMs);
            }
        }

        public void Dispose()
        {
            if (StartDocker)
            {
                DockerUtilities.StopInstance(EventBusDockerContainerName);
                DockerUtilities.RemoveInstance(EventBusDockerContainerName);
                StopProcesses(_dockerProcesses);
            }
            if (StartApis)
            {
                StopProcesses(_apiProcesses);
            }
        }

        private void StartProcess(string fileName, string arguments, List<System.Diagnostics.Process> processes)
        {
            try
            {
                var process = new Process();
                string projectDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", ".."));
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = projectDirectory;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                if (process.HasExited)
                {
                    throw new InvalidOperationException();
                }
                processes.Add(process);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        private void StopProcesses(List<System.Diagnostics.Process> processes)
        {
            processes.ForEach(process =>
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch { }
            });
            processes.Clear();
        }
    }
}
