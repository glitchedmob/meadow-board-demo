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
        private readonly DatabaseService _databaseService;
        private readonly PushButton _button;

        public MailboxButtonService(Logger logger, SmsQueueingService smsQueueingService, DatabaseService databaseService)
        {
            _logger = logger;
            _smsQueueingService = smsQueueingService;
            _databaseService = databaseService;
            _button = new PushButton(MeadowApp.Device.Pins.D10);
        }

        public void StartListening()
        {
            _button.PressStarted += (sender, args) =>
            {
                _logger.Info("Mailbox switch open");

                _databaseService.Db.Insert(new MailboxLog
                {
                    Type = MailboxLogType.MailboxOpened,
                    TimeStamp = DateTime.Now,
                });

                _smsQueueingService.QueueMessage("Mailbox opened");
            };

            _button.PressEnded += (sender, args) =>
            {
                _logger.Info("Mailbox switch closed");

                _databaseService.Db.Insert(new MailboxLog
                {
                    Type = MailboxLogType.MailboxClosed,
                    TimeStamp = DateTime.Now,
                });

                _smsQueueingService.QueueMessage("Mailbox closed!");
            };

            _logger.Info("Started listening for button pressess");
        }
    }
}
