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
    public static class SaveToCosmos
    {
        [FunctionName("SaveToCosmos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"CosmosDB",
            collectionName:"Devices",
            ConnectionStringSetting = "CosmosDbConnection",
            CreateIfNotExists = true
            )] out Device cosmos,
            ILogger log)
        {
            try
            {
                var json = new StreamReader(req.Body).ReadToEnd();
                var data = JsonConvert.DeserializeObject<Device>(json);

                cosmos = data;

                return new OkResult();
            }
            catch { }

            cosmos = null;
            return new BadRequestResult();
        }
    }
}
