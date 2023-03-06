using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Hardware;
using Meadow.Logging;

namespace MeadowBoardDemo.Services
{
    public class WifiService
    {
        private readonly Logger _logger;
        private readonly AppSecrets _appSecrets;
        private readonly IWiFiNetworkAdapter _wifi;

        public WifiService(IMeadowDevice device, Logger logger, AppSecrets appSecrets)
        {
            _logger = logger;
            _appSecrets = appSecrets;
            _wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>() ?? throw new InvalidOperationException("Can't access Wifi adapter");
        }

        public async Task Connect()
        {
            var taskSource = new TaskCompletionSource<object?>();

            _wifi.NetworkConnected += (sender, args) =>
            {
                taskSource.TrySetResult(null);
            };

            await _wifi.Connect(_appSecrets.WifiSsid, _appSecrets.WifiPassword);

            await taskSource.Task;

            _logger.Info($"Wifi connected. IP Address: {_wifi.IpAddress}");
        }

        public async Task Disconnect()
        {
            await _wifi.Disconnect(true);

            _logger.Info("Wifi disconnected");
        }
    }
}
