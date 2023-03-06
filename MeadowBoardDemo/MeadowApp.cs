using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;

namespace MeadowBoardDemo
{
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        private AppSecrets _appSecrets;

        public override Task Run()
        {
            Console.WriteLine("Run...");

            CycleColors(TimeSpan.FromMilliseconds(1000));
            return base.Run();
        }

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            _appSecrets = new AppSecrets();

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            return base.Initialize();
        }

        void CycleColors(TimeSpan duration)
        {
            while (true)
            {
                ShowColorPulse(Color.Blue, duration);
                ShowColorPulse(Color.Cyan, duration);
                ShowColorPulse(Color.Green, duration);
                ShowColorPulse(Color.GreenYellow, duration);
                ShowColorPulse(Color.Yellow, duration);
                ShowColorPulse(Color.Orange, duration);
                ShowColorPulse(Color.OrangeRed, duration);
                ShowColorPulse(Color.Red, duration);
                ShowColorPulse(Color.MediumVioletRed, duration);
                ShowColorPulse(Color.Purple, duration);
                ShowColorPulse(Color.Magenta, duration);
                ShowColorPulse(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, TimeSpan duration)
        {
            onboardLed.StartPulse(color, duration / 2);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, TimeSpan duration)
        {
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}
