using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerEventWebJob
{
    public class Producer : IProducer
    {
        string connectionString = "Endpoint=sb://iaminazurenamespace.servicebus.windows.net/;SharedAccessKeyName=Demo1eventhub;SharedAccessKey=05Qpha3BwB73tjgAz8H3gKKK2HDbfawN5qRGNq9iFqI=;EntityPath=demo1";
        string eventHubName = "demo1";
        EventDataBatch generateData;
        List<string> device = new List<string>();
        EventHubProducerClient producerClient;

        public string ProduceChargeEvent()
        {
            string sessionID = string.Empty;
            sessionID = Guid.NewGuid().ToString();
            Init();
            GenerateEvent().Wait();
            return sessionID;
        }

        public void Init()
        {
            producerClient = new EventHubProducerClient(connectionString, eventHubName);
            device.Add("Mobile");
            device.Add("Laptop");
            device.Add("Desktop");
            device.Add("Tablet");
        }
        public async Task GenerateEvent()
        {
            try
            {
                // send in batch
                int partitionId = 0;
                int id = 1;
                foreach (var eachDevice in device)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    var batchOptions = new CreateBatchOptions() { PartitionId = partitionId.ToString() };
                    generateData = producerClient.CreateBatchAsync(batchOptions).Result;
                    strBuilder.AppendFormat("Search triggered for iPhone 21 from decive {0} ", eachDevice);

                    var eveData = new EventData(Encoding.UTF8.GetBytes(strBuilder.ToString()));
                    // All value should be dynamic
                    eveData.Properties.Add("ID", id);
                    eveData.Properties.Add("UserId", "UserId");
                    eveData.Properties.Add("Location", "North India");
                    eveData.Properties.Add("DeviceType", eachDevice);
                    generateData.TryAdd(eveData);
                    producerClient.SendAsync(generateData).Wait();

                    //Reset partitionId as it can be 0 or 1 as we have define in azure event hub  
                    partitionId++;
                    id++;
                    if (partitionId > 1)
                        partitionId = 0;
                }
                await Task.CompletedTask;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error occruied {0}. Try again later", exp.Message);
            }
        }
    }
}
