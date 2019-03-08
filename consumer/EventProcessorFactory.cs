using System;
using System.Collections.Generic;
using System.Text;
using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventHubsConsumer
{
    class EventProcessorFactory : IEventProcessorFactory
    {
        private IEventCollector collector;
        private Guid id;

        public EventProcessorFactory(IEventCollector collector, Guid Id)
        {
            this.collector = collector;
            this.id = Id;
        }

        IEventProcessor IEventProcessorFactory.CreateEventProcessor(PartitionContext context)
        {
            return new SimpleEventProcessor(context, collector, id);
        }
    }
}