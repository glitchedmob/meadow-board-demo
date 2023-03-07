using System;
using Meadow.Foundation;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Logging;
using MeadowBoardDemo.Entitties;

namespace MeadowBoardDemo.Services
{
    public class MailboxButtonService
    {
        private readonly Logger _logger;
        private readonly SmsQueueingService _smsQueueingService;
        private readonly StorageService _storageService;
        private readonly PushButton _button;

        public MailboxButtonService(Logger logger, SmsQueueingService smsQueueingService, StorageService storageService)
        {
            _logger = logger;
            _smsQueueingService = smsQueueingService;
            _storageService = storageService;
            _button = new PushButton(MeadowApp.Device.Pins.D10);
        }

        public void StartListening()
        {
            _button.PressStarted += async (sender, args) =>
            {
                _logger.Info("Mailbox switch open");

                var logs = _storageService.GetMailboxLogs();

                logs.Add(new MailboxLog
                {
                    Id = Guid.NewGuid(),
                    Type = MailboxLogType.MailboxOpened,
                    TimeStamp = DateTime.Now,
                });

                await _storageService.SaveMailboxLogs(logs);

                _smsQueueingService.QueueMessage("Mailbox opened");
            };

            _button.PressEnded += async (sender, args) =>
            {
                _logger.Info("Mailbox switch closed");

                var logs = _storageService.GetMailboxLogs();

                logs.Add(new MailboxLog
                {
                    Id = Guid.NewGuid(),
                    Type = MailboxLogType.MailboxClosed,
                    TimeStamp = DateTime.Now,
                });

                await _storageService.SaveMailboxLogs(logs);

                _smsQueueingService.QueueMessage("Mailbox closed!");
            };

            _logger.Info("Started listening for button pressess");
        }
    }
}
