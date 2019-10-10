using System;
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace EventGridPublisher
{
    // This captures the "Data" portion of an EventGridEvent on a custom topic
    class ContosoItemReceivedEventData
    {
        [JsonProperty(PropertyName = "itemSku")]
        public string ItemSku { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Enter values for <topic-name> and <region>. You can find this topic endpoint value
            // in the "Overview" section in the "Event Grid Topics" blade in Azure Portal.
            string topicEndpoint = "https://eventgridtopic.westus-1.eventgrid.azure.net/api/events";

            // TODO: Enter value for <topic-key>. You can find this in the "Access Keys" section in the
            // "Event Grid Topics" blade in Azure Portal.
            string topicKey = "4m8G639DEl685zi6ZYT9UV/JNj61sdUMI1cWnR+cJRQ=";

            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            client.PublishEventsAsync(topicHostname, GetEventsList()).GetAwaiter().GetResult();
            Console.Write("Published events to Event Grid topic.");
            Console.ReadLine();
        }

        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 2; i++)
            {
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Contoso.Items.ItemReceived",
                    Data = new ContosoItemReceivedEventData()
                    {
                        ItemSku = "Contoso Item SKU #1"
                    },
                    EventTime = DateTime.Now,
                    Subject = "Door1",
                    DataVersion = "2.0"
                });
            }

            return eventsList;
        }
    }
}

