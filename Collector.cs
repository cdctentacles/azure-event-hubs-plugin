using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CDC.EventCollector;

namespace CDC.AzureEventCollector
{
    public class Collector : IPersistentCollector
    {
        public Task PersistTransactions(List<PartitionChange> changes)
        {
            return Task.CompletedTask;
        }
    }
}
