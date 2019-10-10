using System;
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace EventGridPublisher
{
    // This captures the "Data" portion of an EventGridEvent on a domain
    class ContosoItemReceivedEventData
    {
        [JsonProperty(PropertyName = "itemSku")]
        public string ItemSku { get; set; }

        // [JsonProperty(PropertyName = "color1\\.color2")]
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "model")]
        public int Model { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Enter values for <domain-name> and <region>. You can find this domain endpoint value
            // in the "Overview" section in the "Event Grid Domains" blade in Azure Portal.
            string domainEndpoint = "https://testeventgriddomain.westus-1.eventgrid.azure.net/api/events";

            // TODO: Enter value for <domain-key>. You can find this in the "Access Keys" section in the
            // "Event Grid Domains" blade in Azure Portal.
            string domainKey = "hexZjX1VZ99cMqV1VQx9dAo7s7go3uzkrbyLaBha7KQ=";

            string domainHostname = new Uri(domainEndpoint).Host;
            TopicCredentials domainKeyCredentials = new TopicCredentials(domainKey);
            EventGridClient client = new EventGridClient(domainKeyCredentials);

            client.PublishEventsAsync(domainHostname, GetEventsList()).GetAwaiter().GetResult();
            Console.Write("Published events to Event Grid domain.");
            Console.ReadLine();
        }

        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 1; i++)
            {
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Contoso.Items.ItemReceived",

                    // TODO: Specify the name of the topic (under the domain) to which this event is destined for.
                    // Currently using a topic name "domaintopic0"
                    Topic = $"testeventgriddomain",
                    Data = new ContosoItemReceivedEventData()
                    {
                        ItemSku = "Contoso Item SKU #1",
                        Color = "green",
                        Model = 11
                    },
                    EventTime = DateTime.Now,
                    Subject = "BLUE",
                    DataVersion = "2.0"
                });
            }

            return eventsList;
        }
    }
}

