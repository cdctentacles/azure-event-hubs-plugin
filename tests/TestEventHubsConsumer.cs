using System;
using Xunit;
using CDC.EventCollector;
using eventcollector.tests;
using System.Threading;
using System.Threading.Tasks;

namespace CDC.AzureEventHubsPlugin
{
    public class UserScenario
    {
        [Fact]
        public async Task TestEventHubsConsumer()
        {
            var eventHubsConf = new EventHubsConfiguration("<hubsendpoint>",	
                                                            "cluster1",
                                                            "azureeventhubscntr",
                                                            "eventhubsstorageacnt",
                                                            "<storagekey>");
            var collectorEventsProducerFactory = new CollectorEventsProducerFactory(eventHubsConf);
            var collector = new TestEventCollector();
            var eventSource = collectorEventsProducerFactory.CreateSource(collector, new TestHealthStore());
            var source = eventSource as CollectorEventsProducer;
            await source.Init();
            // wait for collector to get the events
            Thread.Sleep(30000);
            await source.StopEventProcessor();
        }
    }
}
