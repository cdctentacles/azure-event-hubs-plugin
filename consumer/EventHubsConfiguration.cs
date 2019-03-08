using System;
using System.Collections.Generic;
using System.Text;
using CDC.EventCollector;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace EventHubsConsumer
{
    public class EventHubsConfiguration
    {
        private string EventHubConnectionString = "{Event Hubs connection string}";
        private string EventHubName = "{Event Hub path/name}";
        private string StorageContainerName = "{Storage account container name}";
        private string StorageAccountName = "{Storage account name}";
        private string StorageAccountKey = "{Storage account key}";
        public EventHubsConfiguration(string EventHubConnectionString, string EventHubName, string StorageContainerName, string StorageAccountName, string StorageAccountKey)
        {
            this.EventHubConnectionString = EventHubConnectionString;
            this.EventHubName = EventHubName;
            this.StorageContainerName = StorageContainerName;
            this.StorageAccountName = StorageAccountName;
            this.StorageAccountKey = StorageAccountKey;
        }

        public string ConnectionString
        {
            get { return this.EventHubConnectionString; }
        }

        public string HubName
        {
            get { return this.EventHubName; }
        }

        public string ContainerName
        {
            get { return this.StorageContainerName; }
        }

        public string AccountName
        {
            get { return this.StorageAccountName; }
        }

        public string AccountKey
        {
            get { return this.AccountKey; }
        }
    }
}