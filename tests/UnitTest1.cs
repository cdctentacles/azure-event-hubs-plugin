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
            var eventHubsConf = new EventHubsConfiguration("Endpoint=sb://cdctentacles.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Cbnmd50ea7iozxa5NAcqlF4727NGENguVNWyPNl7CsI=",	
            "cluster1", "azureeventhubscntr", "eventhubsstorageacnt", "8gSa/MZK709NUuYcoO9Uv4y/odE8egTIbEmXsO6+54OHP/Sp3ZtCfsIsBjuMqZ6DjBLYXsK1YDVmlXCKB94Kjg==");
            var collectorEventsProducerFactory = new CollectorEventsProducerFactory(eventHubsConf);
            var collector = new TestEventCollector();
            var eventSource = collectorEventsProducerFactory.CreateSource(collector, new TestHealthStore());
            // assert that collector is getting all the events
        }
    }
}
