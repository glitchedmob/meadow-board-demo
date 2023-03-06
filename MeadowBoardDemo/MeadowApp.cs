using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using MeadowBoardDemo.Services;

namespace MeadowBoardDemo
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private DatabaseService _databaseService = null!;
        private WifiService _wifiService = null!;
        private MailboxButtonService _mailboxButtonService = null!;

        public override async Task Run()
        {
            _databaseService?.Initialze();

            await _wifiService.Connect();

            _mailboxButtonService?.StartListening();

            await base.Run();
        }

        public override async Task Initialize()
        {
            RegisterServices();

            _databaseService = Resolver.Services.GetOrCreate<DatabaseService>();
            _wifiService = Resolver.Services.GetOrCreate<WifiService>();
            _mailboxButtonService = Resolver.Services.GetOrCreate<MailboxButtonService>();

            await base.Initialize();
        }

        private void RegisterServices()
        {
            Resolver.Services.GetOrCreate<AppSecrets>();
            Resolver.Services.GetOrCreate<DatabaseService>();
            Resolver.Services.GetOrCreate<StatusLedService>();
            Resolver.Services.GetOrCreate<WifiService>();
            Resolver.Services.GetOrCreate<SmsService>();
            Resolver.Services.GetOrCreate<SmsQueueingService>();
            Resolver.Services.GetOrCreate<MailboxButtonService>();
        }
    }
}
