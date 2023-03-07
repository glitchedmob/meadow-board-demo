using System;
using System.Linq;
using System.Threading;
using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowBoardDemo.Services;

namespace MeadowBoardDemo.RequestHandlers
{
    public class MailboxLogsHandler : RequestHandlerBase
    {
        public override bool IsReusable => true;

        private readonly StorageService _storageService;

        public MailboxLogsHandler()
        {
            _storageService = Resolver.Services.GetOrCreate<StorageService>();
        }

        [HttpGet("/mailbox-logs")]
        public IActionResult MailboxLogs()
        {
            var logs = _storageService.GetMailboxLogs();

            return new JsonResult(logs.Select(log => log.ToDictionary()).ToList());
        }
    }
}
