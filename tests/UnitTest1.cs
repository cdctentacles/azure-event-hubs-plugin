using System;
using Xunit;
using EventHubsConsumer;
using CDC.EventCollector;

namespace EventHubsConsumer.tests
{
    public class UserScenario
    {
        [Fact]
        public void Test1()
        {
            var collectorEventsProducerFactory = new CollectorEventsProducerFactory(eventHubsConf);
            var collector = new TestCollector();
            var eventSource = collectorEventsProducerFactory.CreateSource(collector, new TestHealthStore());
            // assert that collector is getting all the events
        }
    }
}
