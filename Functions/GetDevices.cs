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
    public static class GetDevices
    {
        private static readonly RegistryManager registry =
            RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var results = registry.CreateQuery("SELECT * FROM devices");
            var twins = await results.GetNextAsTwinAsync();

            if (twins.Any())
                return new OkObjectResult(twins.OrderBy(t => t.DeviceId));
            else
                return new NotFoundResult();
        }
    }
}
