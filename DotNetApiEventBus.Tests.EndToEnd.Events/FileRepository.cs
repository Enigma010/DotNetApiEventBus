using System.Text.Json;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public class FileRepository<EntityType> where EntityType : class, new()
    {
        private static object _lockObject = new object();

        public FileRepository(string outputPath)
        {
            OutputPath = outputPath;
        }

        public string OutputPath { get; set; }

        public List<EntityType> Get()
        {
            lock (_lockObject)
            {
                if (!File.Exists(OutputPath))
                {
                    return new List<EntityType>();
                }
                return JsonSerializer.Deserialize<List<EntityType>>(
                    File.ReadAllText(OutputPath),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<EntityType>();
            }
        }

        public void Save(List<EntityType> events)
        {
            lock (_lockObject)
            {
                string outputDirectory = Path.GetDirectoryName(OutputPath) ?? throw new InvalidOperationException();
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                using (FileStream fs = new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonSerializer.Serialize(events));
                    // Truncate the file to the new length
                    fs.SetLength(fs.Position);
                    writer.Flush(); // Flush writer buffer to FileStream
                    fs.Flush(true); // Flush FileStream buffer to disk (bypasses OS cache)
                }
            }
        }
    }
}
