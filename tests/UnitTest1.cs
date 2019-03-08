using System;
using Xunit;
using EventHubsConsumer;
using CDC.EventCollector;
using eventcollector.tests;

namespace CDC.AzureEventCollector
{
    public class UserScenario
    {
        [Fact]
        public void Test1()
        {
            var eventHubsConf = new EventHubsConfiguration("<hubskey>",	
            "cluster1", "azureeventhubscntr", "eventhubsstorageacnt", "<key>");
            var collectorEventsProducerFactory = new CollectorEventsProducerFactory(eventHubsConf);
            var collector = new TestEventCollector();
            var eventSource = collectorEventsProducerFactory.CreateSource(collector, new TestHealthStore());
            // assert that collector is getting all the events
        }
    }
}
