using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Functions.Models;
using System.Net.Http;

namespace Functions
{
    public static class SaveDeviceToCosmos
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveDeviceToCosmos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "%CosmosDatabaseName%", collectionName: "%CosmosDeviceCollectionName%", ConnectionStringSetting = "CosmosDb", CreateIfNotExists = true)]
                out CosmosDbModel cosmos,
            ILogger log)
        {
            try
            {
                //log.LogInformation("1-1");
                string json = new StreamReader(req.Body).ReadToEnd();
                var data = JsonConvert.DeserializeObject<CosmosDbModel>(json);

                var response = Task.FromResult(client.GetAsync($"{Environment.GetEnvironmentVariable("GetDeviceUri")}?deviceid={data.deviceId}")).Result;
                var result = response.Result.Content.ReadAsStringAsync().Result;

                var device = JsonConvert.DeserializeObject<CosmosDbModel>(result);

                cosmos = device;

                return new OkResult();
            }
            catch
            {
                cosmos = null;

                return new BadRequestResult();
            }
        }
    }
}
