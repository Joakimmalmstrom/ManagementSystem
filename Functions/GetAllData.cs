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
using System.Collections.Generic;

namespace Functions
{
    public static class GetAllData
    {
        [FunctionName("GetAllData")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, [CosmosDB(
                databaseName: "CosmosDB",
                collectionName: "Devices",
                ConnectionStringSetting = "CosmosDb",
                SqlQuery = "SELECT * FROM c"
                )] IEnumerable<Robot> devices,
            ILogger log)
        {
            if (devices is null)
                return new NotFoundResult();
            else
                return new OkObjectResult(devices);
        }
    }
}
