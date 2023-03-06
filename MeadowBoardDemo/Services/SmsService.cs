using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Meadow.Foundation;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class SmsService
    {
        private readonly Logger _logger;
        private readonly AppSecrets _appSecrets;
        private readonly StatusLedService _statusLedService;

        private Uri MessagesUrl => new Uri($"https://api.twilio.com/2010-04-01/Accounts/{_appSecrets.TwilioSid}/Messages.json");

        private string AuthHeader
        {
            get
            {
                var authContent = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(_appSecrets.TwilioSid + ":" + _appSecrets.TwilioAuthToken));

                return $"Basic {authContent}";
            }
        }

        public SmsService(Logger logger, AppSecrets appSecrets, StatusLedService statusLedService)
        {
            _logger = logger;
            _appSecrets = appSecrets;
            _statusLedService = statusLedService;
        }

        public async Task<MessageResponse?> SendSms(string message)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = MessagesUrl,
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), AuthHeader },
                    { HttpRequestHeader.Accept.ToString(), "application/json" },
                },
                Content = BuildContent(message),
            };

            using var client = new HttpClient();

            _statusLedService.StartBlinking(Color.Green);
            _logger.Info("Sending SMS...");

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            var messageResponse = JsonSerializer.Deserialize<MessageResponse>(json);

            _logger.Info("SMS Sent");
            _statusLedService.StopBlinking();

            return messageResponse;
        }

        private FormUrlEncodedContent BuildContent(string message)
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Body", message),
                new KeyValuePair<string, string>("From", _appSecrets.TwilioNumber),
                new KeyValuePair<string, string>("To", _appSecrets.NotificationNumber),
            });
        }
    }

    public class MessageResponse
    {
        [JsonPropertyName("sid")]
        public string? Sid { get; set; }
    }
}
