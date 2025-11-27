using System.Diagnostics;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    public static class DockerUtilities
    {
        public static Process StartInstance(string arguments, string workingDirectory = "")
        {
            Process process = new Process();
            process.StartInfo.FileName = "docker";
            process.StartInfo.Arguments = arguments;
            if (!string.IsNullOrEmpty(workingDirectory))
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            return process;
        }

        public static bool StopInstance(string instanceName)
        {
            Process process = new Process();
            process.StartInfo.FileName = "docker";
            process.StartInfo.Arguments = $"stop {instanceName}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }

        public static bool RemoveInstance(string instanceName)
        {
            Process process = new Process();
            process.StartInfo.FileName = "docker";
            process.StartInfo.Arguments = $"rm {instanceName}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}
