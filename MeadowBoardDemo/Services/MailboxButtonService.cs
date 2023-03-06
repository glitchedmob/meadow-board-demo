using Meadow.Foundation;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class MailboxButtonService
    {
        private readonly Logger _logger;
        private readonly SmsQueueingService _smsQueueingService;
        private readonly PushButton _button;

        public MailboxButtonService(Logger logger, SmsQueueingService smsQueueingService)
        {
            _logger = logger;
            _smsQueueingService = smsQueueingService;
            _button = new PushButton(MeadowApp.Device.Pins.D10);
        }

        public void StartListening()
        {
            _button.PressStarted += (sender, args) =>
            {
                _logger.Info("Mailbox switch open");

                _smsQueueingService.QueueMessage("Mailbox opened");
            };

            _button.PressEnded += (sender, args) =>
            {
                _logger.Info("Mailbox switch closed");

                _smsQueueingService.QueueMessage("Mailbox closed!");
            };

            _logger.Info("Started listening for button pressess");
        }
    }
}
