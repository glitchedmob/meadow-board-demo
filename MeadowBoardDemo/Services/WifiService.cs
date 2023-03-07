using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation;
using Meadow.Hardware;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class WifiService
    {
        private readonly Logger _logger;
        private readonly AppSecrets _appSecrets;
        private readonly StatusLedService _statusLedService;
        public IWiFiNetworkAdapter WifiAdapter { get; }

        public WifiService(IMeadowDevice device, Logger logger, AppSecrets appSecrets,
            StatusLedService statusLedService)
        {
            _logger = logger;
            _appSecrets = appSecrets;
            _statusLedService = statusLedService;
            WifiAdapter = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>() ??
                    throw new InvalidOperationException("Can't access Wifi adapter");
        }

        public async Task Connect()
        {
            _statusLedService.StartBlinking(Color.Blue);
            _logger.Info("Connecting to Wifi...");

            if (WifiAdapter.IsConnected)
            {
                _logger.Info($"Wifi connected. IP Address: {WifiAdapter.IpAddress}");
                _statusLedService.StopBlinking();
                return;
            }

            var taskSource = new TaskCompletionSource<object?>();

            var networkConnectionHandler = new NetworkConnectionHandler((sender, args) =>
            {
                taskSource.TrySetResult(null);
            });

            WifiAdapter.NetworkConnected += networkConnectionHandler;

            await WifiAdapter.Connect(_appSecrets.WifiSsid, _appSecrets.WifiPassword);

            await taskSource.Task;

            WifiAdapter.NetworkConnected -= networkConnectionHandler;

            _logger.Info($"Wifi connected. IP Address: {WifiAdapter.IpAddress}");
            _statusLedService.StopBlinking();
        }

        public Task Disconnect()
        {
            _statusLedService.StartBlinking(Color.Blue);
            _logger.Info("Wifi disconnecting...");

            // await _wifi.Disconnect(false);

            _logger.Info("Wifi disconnected");
            _statusLedService.StopBlinking();

            return Task.CompletedTask;
        }
    }
}
