using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaterCutter
{
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=management-iot.azure-devices.net;DeviceId=1;SharedAccessKey=OTh8Q4h56GhO0pF6gUWQrgy9O4G/h3DZc4HqsWq20AM=");
        private static bool sending = false;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Water Cutter");

            await deviceClient.SetMethodHandlerAsync("start", StartAsync, null);
            await deviceClient.SetMethodHandlerAsync("stop", StopAsync, null);

            await SendDataAsync();

            Console.ReadKey();
        }

        static async Task SendDataAsync()
        {
            while (true)
            {
                while (sending)
                {
                    await SendMessageAsync(new WaterCutterModel { deviceId = "1", x = 10, y = 15, z = 0.5 });

                    Console.WriteLine("Message has been sent");
                    await Task.Delay(5000);
                }

                await Task.Delay(1000);
            }
        }

        static Task<MethodResponse> StartAsync(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("Start Sending Messages");
            sending = true;
            return Task.FromResult(new MethodResponse(new byte[0], 200));
        }
        static Task<MethodResponse> StopAsync(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("Stop Sending Messages");
            sending = false;
            return Task.FromResult(new MethodResponse(new byte[0], 200));
        }

        private static async Task SendMessageAsync(WaterCutterModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var msg = new Message(Encoding.UTF8.GetBytes(json))
            {
                ContentEncoding = "utf-8",
                ContentType = "application/json"
            };

            await deviceClient.SendEventAsync(msg);
        }
    }
}
