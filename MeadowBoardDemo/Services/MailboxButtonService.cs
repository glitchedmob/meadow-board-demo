using Meadow.Foundation;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class MailboxButtonService
    {
        private readonly Logger _logger;
        private readonly NotificationService _notificationService;
        private readonly PushButton _button;

        public MailboxButtonService(Logger logger, NotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _button = new PushButton(MeadowApp.Device.Pins.D10);
        }

        public void StartListening()
        {
            _button.PressStarted += (sender, args) =>
            {
                _logger.Info("Mailbox switch open");

                _notificationService.QueueNotification(new Notification
                {
                    Message = "Mailbox opened"
                });
            };

            _button.PressEnded += (sender, args) =>
            {
                _logger.Info("Mailbox switch closed");

                _notificationService.QueueNotification(new Notification
                {
                    Message = "Mailbox closed!"
                });
            };

            _logger.Info("Started listening for button pressess");
        }
    }
}
