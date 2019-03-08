using System;
using System.Threading.Tasks;
using CDC.EventCollector;

namespace EventHubsConsumer.tests
{
    public class TestEventCollector : IEventCollector
    {
        public Task TransactionApplied(Guid partitionId, long previousLsn, long lsn, byte [] transaction)
        {
            return Task.FromResult(0);
            // write to console the transactions we are recieving
        }
    }
}