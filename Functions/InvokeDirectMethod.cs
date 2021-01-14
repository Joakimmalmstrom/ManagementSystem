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

namespace Functions
{
    public static class InvokeDirectMethod
    {
        private static readonly ServiceClient service = ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("InvokeDirectMethod")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<DataModel>(requestBody);
                
                var payload = new CloudToDeviceMethod(data.methodName);
                payload.SetPayloadJson(JsonConvert.SerializeObject(new { data.payload }));

                var response = await service.InvokeDeviceMethodAsync(data.deviceId, payload);
                return new OkObjectResult(response);

            }
            catch
            {
                return new BadRequestResult();
            }
        }

        private static async Task<CloudToDeviceMethodResult> SendDirectMethodAsync(dynamic data)
        {
            var payload = new CloudToDeviceMethod(data.methodName);
            payload.SetPayloadJson(JsonConvert.SerializeObject(new { data.payload }));

            return await service.InvokeDeviceMethodAsync(data.deviceId, payload);
        }

        public class DataModel
        {
            public string deviceId { get; set; }
            public string methodName { get; set; }
            public string payload { get; set; }

        }

    }
}
