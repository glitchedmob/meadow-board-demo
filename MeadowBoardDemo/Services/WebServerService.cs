using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class WebServerService
    {
        private readonly WifiService _wifiService;
        private readonly Logger _logger;
        private MapleServer _mapleServer = null!;

        public WebServerService(WifiService wifiService, Logger logger)
        {
            _wifiService = wifiService;
            _logger = logger;
        }

        public void Initialize()
        {
            _mapleServer = new MapleServer(_wifiService.WifiAdapter.IpAddress, logger: _logger, processMode: RequestProcessMode.Serial);
            _mapleServer.Start();
        }
    }
}
