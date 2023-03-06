using System;
using Meadow;

namespace MeadowBoardDemo
{
    public class AppSecrets : ConfigurableObject
    {
        public string TwilioAuthToken => GetConfiguredValue() ?? throw new ArgumentNullException();
        public string TwilioSid => GetConfiguredValue() ?? throw new ArgumentNullException();
        public string TwilioNumber => GetConfiguredValue() ?? throw new ArgumentNullException();
        public string WifiSsid => GetConfiguredValue() ?? throw new ArgumentNullException();
        public string WifiPassword => GetConfiguredValue() ?? throw new ArgumentNullException();
    }
}
