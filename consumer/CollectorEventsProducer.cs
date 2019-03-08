using System;
using System.Collections.Generic;
using System.Text;
using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventHubsConsumer
{
    public class CollectorEventsProducer : Source
    {
        public CollectorEventsProducer(IEventCollector collector, IHealthStore healthStore, EventHubsConfiguration conf) :
        base(collector, healthStore)
        {
            this.Id = new Guid();
            this.collector = collector;
            this.conf = conf;
            this.StartEventProcessor();
        }

        public override Guid GetSourceId()
        {
            return this.Id;
        }

        public override ITransactionalLog GetTransactionalLog()
        {
            return null;
        }

        public IEventCollector Collector
        {
            get { return this.collector; }
        }

        public async void StartEventProcessor()
        {
            var eventProcessorHost = new EventProcessorHost(
                conf.HubName,
                PartitionReceiver.DefaultConsumerGroupName,
                conf.ConnectionString,
                string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", conf.AccountName, conf.AccountKey),
                conf.ContainerName
            );

            this.eventProcessorHost = eventProcessorHost;

            // Registers the Event Processor Host and starts receiving messages
            // https://stackoverflow.com/questions/33371803/how-to-pass-parameters-to-an-implementation-of-ieventprocessor
            await eventProcessorHost.RegisterEventProcessorFactoryAsync(new EventProcessorFactory(collector, GetSourceId()));
        }

        public async void StopEventProcessor()
        {
            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }

        Guid Id;
        private IEventCollector collector;
        private readonly EventHubsConfiguration conf;
        private EventProcessorHost eventProcessorHost;
    }
}
