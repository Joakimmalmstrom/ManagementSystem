using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Functions.Models;

namespace Functions
{
    public static class SaveDataToCosmosDb
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveDataToCosmosDb")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubEndpoint", ConsumerGroup = "cosmosdb")] EventData message,
            [CosmosDB(databaseName: "%CosmosDatabaseName%", collectionName: "%CosmosDataCollectionName%", ConnectionStringSetting = "CosmosDb", CreateIfNotExists = true)]
            out dynamic cosmos,
            ILogger log)
        {
            try
            {
                cosmos = Encoding.UTF8.GetString(message.Body.Array);
            }
            catch
            {
                cosmos = null;
            }
            
            
        }
    }
}