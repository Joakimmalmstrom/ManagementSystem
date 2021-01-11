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

namespace Functions
{
    public static class GetDataById
    {
        [FunctionName("GetDataById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "CosmosDB",
                collectionName: "Devices",
                ConnectionStringSetting = "CosmosDb",
                Id = "{Query.id}"
                )] Robot device,
            ILogger log)
        {
            if (device is null)
                return new NotFoundResult();
            else
                return new OkObjectResult(device);
        }
    }
}
