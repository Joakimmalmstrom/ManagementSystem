using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using Functions.Models;
using System.Net.Http;
using System.Text;

namespace Functions
{
    public static class CreateDevice
    {
        private static HttpClient client = new HttpClient();
        private static readonly RegistryManager registry = 
            RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("CreateDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CosmosDbModel>(json);

            try
            {
                var device = await registry.AddDeviceAsync(new Device(data.deviceId));
                var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                await client.PostAsync(Environment.GetEnvironmentVariable("SaveDeviceToCosmosDbUri"), payload);

                return new OkObjectResult(device);
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}
