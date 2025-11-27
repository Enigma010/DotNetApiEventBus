using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus.Tests.EndToEnd.Events
{
    public class App
    {
        public const string DefaultName = "DotNetApiEventBus.Tests.EndToEnd.Events";
        public string Name { get; set; } = DefaultName;
    }
}
