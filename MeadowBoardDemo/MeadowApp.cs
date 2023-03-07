using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using MeadowBoardDemo.Services;

namespace MeadowBoardDemo
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private WifiService _wifiService = null!;
        private MailboxButtonService _mailboxButtonService = null!;
        private WebServerService _webServerService = null!;
        private StorageService _storageService = null!;

        public override async Task Initialize()
        {
            RegisterServices();

            _storageService = Resolver.Services.GetOrCreate<StorageService>();
            _wifiService = Resolver.Services.GetOrCreate<WifiService>();
            _mailboxButtonService = Resolver.Services.GetOrCreate<MailboxButtonService>();
            _webServerService = Resolver.Services.GetOrCreate<WebServerService>();

            await base.Initialize();
        }

        public override async Task Run()
        {
            await _storageService.Initialize();

            await _wifiService.Connect();

            _webServerService.Initialize();

            _mailboxButtonService?.StartListening();

            await base.Run();
        }

        private void RegisterServices()
        {
            Resolver.Services.GetOrCreate<AppSecrets>();
            Resolver.Services.GetOrCreate<StorageService>();
            Resolver.Services.GetOrCreate<StatusLedService>();
            Resolver.Services.GetOrCreate<WifiService>();
            Resolver.Services.GetOrCreate<WebServerService>();
            Resolver.Services.GetOrCreate<SmsService>();
            Resolver.Services.GetOrCreate<SmsQueueingService>();
            Resolver.Services.GetOrCreate<MailboxButtonService>();
        }
    }
}
