using System;
using System.Text;
using System.Threading.Tasks;
using CDC.EventCollector;

namespace CDC.AzureEventHubsPlugin
{
    public class TestEventCollector : IEventCollector
    {
        public Task TransactionApplied(Guid partitionId, long previousLsn, long lsn, byte [] transaction)
        {
            // write to console the transactions we are recieving
            Console.WriteLine(string.Format("Received message with partitionId: {0}, previousLsn: {1}, lsn: {2}, transaction: {3}",
                              partitionId, previousLsn, lsn, Encoding.UTF8.GetString(transaction)));
            return Task.FromResult(0);
        }
    }
}