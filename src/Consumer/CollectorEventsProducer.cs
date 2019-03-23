using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace CDC.AzureEventHubsPlugin
{
    public class CollectorEventsProducer : Source
    {
        public CollectorEventsProducer(IEventCollector collector, IHealthStore healthStore, EventHubsConfiguration conf) :
        base(collector, healthStore)
        {
            this.Id = new Guid();
            this.collector = collector;
            this.conf = conf;
            this.healthStore = healthStore;
        }

        public async Task Init()
        {
            await StartEventProcessor();
        }

        public override Guid GetSourceId()
        {
            return this.Id;
        }

        public IEventCollector Collector
        {
            get { return this.collector; }
        }

        public async Task StartEventProcessor()
        {
            try
            {
                this.eventProcessorHost = new EventProcessorHost(
                    conf.HubName,
                    PartitionReceiver.DefaultConsumerGroupName,
                    conf.ConnectionString,
                    string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", conf.AccountName, conf.AccountKey),
                    conf.ContainerName
                );
            }
            catch (Exception e)
            {
                var msg = string.Format("AzureEventHubsPlugin: StartEventProcessor() failed with exception {0}", e);
                healthStore.WriteError(msg);
            }

            try
            {
                // Registers the Event Processor Host and starts receiving messages
                await eventProcessorHost.RegisterEventProcessorFactoryAsync(new EventProcessorFactory(collector, GetSourceId()));
            }
            catch (Exception e)
            {
                var msg = string.Format("AzureEventHubsPlugin: StartEventProcessor() failed with exception {0}", e);
                healthStore.WriteError(msg);
            }
        }

        public async Task StopEventProcessor()
        {
            try
            {
                // Disposes of the Event Processor Host
                await eventProcessorHost.UnregisterEventProcessorAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("StopEventProcessor() failed with: ", e);
            }
        }

        Guid Id;
        private IEventCollector collector;
        private readonly EventHubsConfiguration conf;
        private EventProcessorHost eventProcessorHost;
        private IHealthStore healthStore;
    }
}
