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
using System.Linq;

namespace Functions
{
    public static class GetDevice
    {
        private static readonly RegistryManager registry =
            RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("GetDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string deviceId = req.Query["deviceId"];
            var results = registry.CreateQuery($"SELECT * FROM devices WHERE deviceId = '{deviceId}'");
            var twins = await results.GetNextAsTwinAsync();

            if (twins.FirstOrDefault() != null)
                return new OkObjectResult(twins.FirstOrDefault());
            else
                return new NotFoundResult();
        }
    }
}
