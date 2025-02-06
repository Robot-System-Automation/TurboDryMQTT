using System.Text;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using TurboDryMQTT.Hubs;

namespace TurboDryMQTT.Services
{
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly IHubContext<MqttHub> _hubContext;
        private readonly List<string> _subscribedTopics = new List<string>();
        private bool _isConnected = false;
        private Timer _disconnectTimer;
        private readonly Dictionary<string, string> _latestMessages = new Dictionary<string, string>();
        private Timer _updateCheckTimer;

        public MqttService(IHubContext<MqttHub> hubContext)
        {
            _hubContext = hubContext;

            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("MqttWebAppClient")
                //.WithTcpServer("test.mosquitto.org", 1883)
                .WithTcpServer("172.31.10.136", 1883)
                .WithCleanSession()
                .Build();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected to MQTT broker");

                await _mqttClient.SubscribeAsync("rsa/738/TD/status");

                await PublishMessageAsync("rsa/738/TD/status", "1");

                await _hubContext.Clients.All.SendAsync("BrokerStatus", "Connected");

                _disconnectTimer = new Timer(async _ =>
                {
                    _isConnected = false;
                    Console.WriteLine("No status update received - Broker Disconnected");
                    await _hubContext.Clients.All.SendAsync("BrokerStatus", "Disconnected");
                }, null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);

                await SubscribeToTopics();
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                System.Diagnostics.Debug.WriteLine("Disconnected from MQTT broker.");

                _isConnected = false;
                await _hubContext.Clients.All.SendAsync("BrokerStatus", "Disconnected");
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                if (e.ApplicationMessage == null || e.ApplicationMessage.Payload == null)
                {
                    Console.WriteLine("Recieved an empty message");
                    return;
                }

                var message = e.ApplicationMessage;

                if (string.IsNullOrEmpty(message.Topic))
                {
                    Console.WriteLine("Recieved a null or empty message");
                    return;
                }

                lock (_latestMessages)
                {
                    _latestMessages[topic] = payload; // Store the latest message
                }

                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

                //System.Diagnostics.Debug.WriteLine("Message received event triggered.");
                //var message = e.ApplicationMessage;
                //string topic = message.Topic;
                //var payload = Encoding.UTF8.GetString(message.Payload);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", topic, payload);

                //ProcessIncomingMessage(message.Topic, payload);

                //return Task.CompletedTask;
            };

            _mqttClient.ConnectAsync(options).Wait();

            _updateCheckTimer = new Timer(async _ =>
            {
                await CheckForUpdates();
            }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10));
        }

        private async Task SubscribeToTopics()
        {
            try
            {
                await _mqttClient.SubscribeAsync("rsa/738/TD/status");
                await _mqttClient.SubscribeAsync("rsa/738/TD/op_station_number");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st1_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st1_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st1_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st1_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st2_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st2_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st2_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st2_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st3_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st3_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st3_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st3_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st4_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st4_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st4_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st4_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st5_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st5_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st5_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st5_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st6_lev2_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st6_lev2_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st6_lev1_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/st6_lev1_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/sole_presence_lf");
                await _mqttClient.SubscribeAsync("rsa/738/TD/sole_presence_rg");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos1");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos2");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos3");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos4");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos5");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos6");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos7");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos8");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos9");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos10");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos11");
                await _mqttClient.SubscribeAsync("rsa/738/TD/list_pos12");

                System.Diagnostics.Debug.WriteLine("Subscribed to topics.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Subscription failed: {ex.Message}");
            }
        }

        public async Task PublishMessageAsync(string topic, string payload)
        {
            if (!_mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT Client is not connected.");
                await _hubContext.Clients.All.SendAsync("BrokerStatus", "Disconnected"); // Notify the page
                return;
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message);
            Console.WriteLine($"Published message: {payload} to topic: {topic}");
        }

        private async Task CheckForUpdates()
        {
            List<(string topic, string message)> updates;

            lock (_latestMessages)
            {
                updates = _latestMessages.Select(kvp => (kvp.Key, kvp.Value)).ToList();
            }

            foreach (var (topic, newMessage) in updates)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", topic, newMessage);
                Console.WriteLine($"Updated message for {topic}: {newMessage}");
            }
        }
    }
}

