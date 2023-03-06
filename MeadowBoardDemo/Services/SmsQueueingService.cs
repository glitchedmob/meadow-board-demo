using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowBoardDemo.Services
{
    public class SmsQueueingService
    {
        private readonly WifiService _wifiService;
        private readonly SmsService _smsService;
        private readonly Queue<string> _queue = new Queue<string>();
        private bool _processingQueue = false;

        public SmsQueueingService(WifiService wifiService, SmsService smsService)
        {
            _wifiService = wifiService;
            _smsService = smsService;
        }

        public void QueueMessage(string message)
        {
            _queue.Enqueue(message);

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

            while (_queue.TryDequeue(out var message))
            {
                await ProcessMessage(message);
            }

            await _wifiService.Disconnect();

            _processingQueue = false;

            _ = ProcessQueue();
        }

        private async Task ProcessMessage(string message)
        {
            await _smsService.SendSms(message);
        }
    }
}
