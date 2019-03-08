using System;
using System.Collections.Generic;
using System.Text;
using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventHubsConsumer
{
    public class CollectorEventsProducerFactory : ISourceFactory
    {
        private CollectorEventsProducer collectorEventsProducer;
        private readonly EventHubsConfiguration eventHubsConf;

        public CollectorEventsProducerFactory(EventHubsConfiguration conf) => this.eventHubsConf = conf;

        public ISource CreateSource(IEventCollector collector, IHealthStore healthStore)
        {
            if (this.collectorEventsProducer == null)
            {
                this.collectorEventsProducer = new CollectorEventsProducer(collector, healthStore, eventHubsConf);
            }

            return this.collectorEventsProducer;
        }

        public CollectorEventsProducer EventSource
        {
            get { return this.collectorEventsProducer; }
        }
    }
}