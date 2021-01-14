using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Shared;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaterCutter
{
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=management-iot.azure-devices.net;DeviceId=1;SharedAccessKey=OTh8Q4h56GhO0pF6gUWQrgy9O4G/h3DZc4HqsWq20AM=", TransportType.Mqtt);
        private static Temperature temperature;
        private static Random random;
        private static int interval;
        private static bool sendingState = false;


        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Water Cutter");

            random = new Random();
            interval = 5000;

            //await deviceClient.SetMethodHandlerAsync("start", StartAsync, null);
            //await deviceClient.SetMethodHandlerAsync("stop", StopAsync, null);

            await SendDataAsync();
            await RecieveDataAsync();

            Console.ReadKey();
        }

        private static Task<MethodResponse> ChangeSendingState(MethodRequest methodRequest, object userContext)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<DataModel>(methodRequest.DataAsJson);

                sendingState = bool.Parse(payload.payload);
            }
            catch { }

            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { sendingState })), 200));
        }

        static async Task RecieveDataAsync()
        {
            while (true)
            {
                var data = await deviceClient.ReceiveAsync();
                var msg = Encoding.UTF8.GetString(data.GetBytes());

                Console.WriteLine(msg);
            }
        }

        static async Task SendDataAsync()
        {
            while (true)
            {
                while (true)
                {
                    try
                    {
                        temperature = new Temperature(random.Next(0, 20));
                        var payload = JsonConvert.SerializeObject(temperature);

                        await deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(payload)));
                        Console.WriteLine($"Sending Data: {payload}");
                    }
                    catch { }

                    await Task.Delay(interval);

                    //await SendMessageAsync(new WaterCutterModel { deviceId = "1", x = 10, y = 15, z = 0.5 });

                    //Console.WriteLine("Message has been sent");
                    //await Task.Delay(5000);
                }
            }
        }

        static Task<MethodResponse> StartAsync(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("Start Sending Messages");
            sendingState = true;
            return Task.FromResult(new MethodResponse(new byte[0], 200));
        }
        static Task<MethodResponse> StopAsync(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("Stop Sending Messages");
            sendingState = false;
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

        public class DataModel
        {
            public string payload { get; set; }
        }
    }
}
