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
        private readonly IWiFiNetworkAdapter _wifi;

        public WifiService(IMeadowDevice device, Logger logger, AppSecrets appSecrets,
            StatusLedService statusLedService)
        {
            _logger = logger;
            _appSecrets = appSecrets;
            _statusLedService = statusLedService;
            _wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>() ??
                    throw new InvalidOperationException("Can't access Wifi adapter");
        }

        public async Task Connect()
        {
            _statusLedService.StartBlinking(Color.Blue);
            _logger.Info("Connecting to Wifi...");

            if (_wifi.IsConnected)
            {
                _logger.Info($"Wifi connected. IP Address: {_wifi.IpAddress}");
                _statusLedService.StopBlinking();
                return;
            }

            var taskSource = new TaskCompletionSource<object?>();

            var networkConnectionHandler = new NetworkConnectionHandler((sender, args) =>
            {
                taskSource.TrySetResult(null);
            });

            _wifi.NetworkConnected += networkConnectionHandler;

            await _wifi.Connect(_appSecrets.WifiSsid, _appSecrets.WifiPassword);

            await taskSource.Task;

            _wifi.NetworkConnected -= networkConnectionHandler;

            _logger.Info($"Wifi connected. IP Address: {_wifi.IpAddress}");
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
