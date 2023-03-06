using Meadow.Foundation;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class MailboxButtonService
    {
        private readonly Logger _logger;
        private readonly SmsService _smsService;
        private readonly WifiService _wifiService;
        private readonly StatusLedService _statusLedService;
        private readonly PushButton _button;

        public MailboxButtonService(Logger logger, SmsService smsService, WifiService wifiService,
            StatusLedService statusLedService)
        {
            _logger = logger;
            _smsService = smsService;
            _wifiService = wifiService;
            _statusLedService = statusLedService;
            _button = new PushButton(MeadowApp.Device.Pins.D10);
        }

        public void StartListening()
        {
            _button.PressStarted += async (sender, args) =>
            {
                _logger.Info("Mailbox opened");
                _statusLedService.StartBlinking(Color.Blue);
                await _wifiService.Connect();
                _statusLedService.StartBlinking(Color.Purple);
                await _smsService.SendSms("Mailbox opened");
                _statusLedService.StopBlinking();
            };

            _button.PressEnded += async (sender, args) =>
            {
                _logger.Info("Mailbox closed");
                _statusLedService.StartBlinking(Color.Green);
                await _smsService.SendSms("Mailbox closed");
                _statusLedService.StartBlinking(Color.Red);
                await _wifiService.Disconnect();
                _statusLedService.StopBlinking();
            };

            _logger.Info("Started listening for button pressess");
        }
    }
}
