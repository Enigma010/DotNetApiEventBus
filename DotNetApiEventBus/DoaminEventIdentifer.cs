using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    public class DoaminEventIdentifer : IEventIdentifier
    {
        public const string DefaultDelimiter = ".";
        public DoaminEventIdentifer(string domain, string subDomain, string aggregateId, string delimiter = DefaultDelimiter)
        {
            Domain = domain;
            SubDomain = subDomain;
            AggregateId = aggregateId;
            Delimiter = delimiter;
        }
        public string Domain { get; private set; } = string.Empty;
        public string SubDomain { get; private set; } = string.Empty;
        public string AggregateId { get; private set; } = string.Empty;
        public string Delimiter { get; private set; } = DefaultDelimiter;
        public string EventId { get => $"{Domain}{Delimiter}{SubDomain}{Delimiter}{AggregateId}"; }
    }
}
