using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public class FileRepository<EntityType> where EntityType : class, new()
    {
        public FileRepository(string outputPath)
        {
            OutputPath = outputPath;
        }

        public string OutputPath { get; set; }

        public List<EntityType> Get()
        {
            lock (this)
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
            lock (this)
            {
                string outputDirectory = Path.GetDirectoryName(OutputPath) ?? throw new InvalidOperationException(); ;
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                using (FileStream fs = new FileStream(OutputPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonSerializer.Serialize(events));
                    writer.Flush(); // Flush writer buffer to FileStream
                    fs.Flush(true); // Flush FileStream buffer to disk (bypasses OS cache)
                }
            }
        }
    }
}
