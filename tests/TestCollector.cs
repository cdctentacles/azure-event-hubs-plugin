using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using eventcollector.tests;
using Microsoft.Azure.EventHubs;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 8)]

namespace CDC.AzureEventCollector
{
    public class CDCCollectorTest
    {
        [Fact]
        // this test that events reach PersistentCollector API and
        // it throws back invalid credential exceptions.
        // we can't add actual key in code, hence only testing failure path.
        public void PersistentCollectorFailsInUpload()
        {
            var partitionId = new Guid();
            var testSourceFactory = new TestEventSourceFactory(partitionId);
            var persistentCollector = new Collector(partitionId,
                "Endpoint=sb://cdctentacles.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abcdefghijklmnopqrstuvwxyz",
                "test1",
                1,
                new TestHealthStore(),
                TimeSpan.FromSeconds(1));
            var persistentCollectors = new List<IPersistentCollector>() { persistentCollector };
            var conf = new Configuration(testSourceFactory, persistentCollectors)
                .SetHealthStore(new TestHealthStore());

            Assert.Null(testSourceFactory.EventSource);
            CDCCollector.NewCollectorConfiguration(conf);
            Assert.NotNull(testSourceFactory.EventSource);

            var eventSource = testSourceFactory.EventSource;
            var transactionBytes = Encoding.ASCII.GetBytes("{}");
            var transactionTasks = new List<Task>();
            var totalTaransactions = 10;
            Task t1 = null;
            for (var lsn = 1; lsn <= totalTaransactions; ++lsn)
            {
                Task t2 = eventSource.OnTransactionApplied(lsn - 1, lsn, transactionBytes);
                if (t2 != t1)
                {
                    transactionTasks.Add(t2);
                }
                t1 = t2;
            }

            try
            {
                Task.WaitAll(transactionTasks.ToArray());
            }
            catch (AggregateException aggEx)
            {
                var allExs = aggEx.Flatten();
                foreach (var ex in allExs.InnerExceptions)
                {
                    if (ex.GetType() != typeof(MessagingEntityNotFoundException))
                    {
                        throw;
                    }
                }
            }
        }
    }
}