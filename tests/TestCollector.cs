using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using eventcollector.tests;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 8)]

namespace CDC.AzureEventCollector
{
    public class CDCCollectorTest
    {
        [Fact]
        public void SetupOfSourceAndPersistentCollector()
        {
            var testSourceFactory = new TestEventSourceFactory();
            var persistentCollector = new TestPersistentCollector();
            var persistentCollectors = new List<IPersistentCollector>() { persistentCollector };
            var conf = new Configuration(testSourceFactory, persistentCollectors)
                .SetHealthStore(new TestHealthStore());

            // todo :complete the test.
        }
    }
}