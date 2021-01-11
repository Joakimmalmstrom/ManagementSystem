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
    public static class SaveDeviceToCosmos
    {
        [FunctionName("SaveDeviceToCosmos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "%CosmosDatabaseName%", collectionName: "%CosmosCollectionName%", ConnectionStringSetting = "CosmosDb", CreateIfNotExists = true)]
                out CosmosDbModel cosmos,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                cosmos = JsonConvert.DeserializeObject<CosmosDbModel>(requestBody);

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
