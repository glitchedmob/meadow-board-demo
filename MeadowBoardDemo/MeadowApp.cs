using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using MeadowBoardDemo.Services;

namespace MeadowBoardDemo
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private MailboxButtonService _mailboxButtonService = null!;

        public override async Task Run()
        {
            _mailboxButtonService.StartListening();

            await base.Run();
        }

        public override async Task Initialize()
        {
            Resolver.Services.GetOrCreate<AppSecrets>();
            Resolver.Services.GetOrCreate<StatusLedService>();
            Resolver.Services.GetOrCreate<WifiService>();
            Resolver.Services.GetOrCreate<SmsService>();
            Resolver.Services.GetOrCreate<SmsQueueingService>();
            _mailboxButtonService = Resolver.Services.GetOrCreate<MailboxButtonService>();


            await base.Initialize();
        }
    }
}
