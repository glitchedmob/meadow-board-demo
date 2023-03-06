using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowBoardDemo.Services
{
    public class NotificationService
    {
        private readonly WifiService _wifiService;
        private readonly SmsService _smsService;
        private readonly Queue<Notification> _queue = new Queue<Notification>();
        private bool _processingQueue = false;

        public NotificationService(WifiService wifiService, SmsService smsService)
        {
            _wifiService = wifiService;
            _smsService = smsService;
        }

        public void QueueNotification(Notification notification)
        {
            _queue.Enqueue(notification);

            _ = ProcessQueue();
        }

        private async Task ProcessQueue()
        {
            if (_processingQueue || !_queue.Any())
            {
                return;
            }

            _processingQueue = true;

            await _wifiService.Connect();

            while (_queue.TryDequeue(out var notification))
            {
                await ProcessNotification(notification);
            }

            await _wifiService.Disconnect();

            _processingQueue = false;

            _ = ProcessQueue();
        }

        private async Task ProcessNotification(Notification notification)
        {
            await _smsService.SendSms(notification.Message);
        }
    }


    public class Notification
    {
        public string Message { get; set; } = null!;
    }
}
