using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace CDC.AzureEventCollector
{
    public class Collector : IPersistentCollector
    {
        public Collector(Guid id, string eventHubConnectionString, string eventHubName, uint maxRetryCount, IHealthStore healthStore)
        {
            this.id = id;
            this.eventHubConnectionString = eventHubConnectionString;
            this.eventHubName = eventHubName;
            this.maxRetryCount = maxRetryCount;
            this.healthStore = healthStore;

            if (eventHubClients.ContainsKey(GetEventHubKey()))
            {
                var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnectionString)
                {
                    EntityPath = eventHubName
                };

                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
                eventHubClients.Add(GetEventHubKey(), eventHubClient);
            }
        }

        public async Task PersistTransactions(List<PartitionChange> changes)
        {
            var eventHubClient = eventHubClients[GetEventHubKey()];
            var changesInJson = JsonConvert.SerializeObject(changes);
            var retryCount = 0;

            while (retryCount < this.maxRetryCount)
            {
                try
                {
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(changesInJson)));
                    break;
                }
                catch (Exception ex)
                {
                    var msg = string.Format("AzureEventBusCollector : Retry {0}, Exception while persisting changes {1}", retryCount, ex.ToString());
                    healthStore.WriteWarning(msg);
                }

                retryCount += 1;
            }
        }

        private string GetEventHubKey()
        {
            return this.eventHubConnectionString + "/" + this.eventHubName;
        }

        public Guid GetId()
        {
            return this.id;
        }

        Guid id;
        private static Dictionary<string, EventHubClient> eventHubClients = new Dictionary<string, EventHubClient>();
        private readonly string eventHubConnectionString;
        private readonly string eventHubName;
        private readonly uint maxRetryCount;
        private IHealthStore healthStore;
    }
}
