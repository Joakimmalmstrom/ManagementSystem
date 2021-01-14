using Microsoft.Azure.Devices;
using Shared;
using System;
using System.Threading.Tasks;

namespace ServiceApp
{
    class Program
    {
        private static readonly ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("IotHub");

        static async Task Main(string[] args)
        {
            while (true)
            {
                await serviceClient.InvokeDeviceMethodAsync("1", new CloudToDeviceMethod("stop"));
                await Task.Delay(10000);

                await serviceClient.InvokeDeviceMethodAsync("1", new CloudToDeviceMethod("start"));
                await Task.Delay(10000);
            }
        }
    }
}
