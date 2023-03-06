using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;

namespace MeadowBoardDemo.Services
{
    public class StatusLedService
    {
        private readonly RgbPwmLed _onboardLed;

        private CancellationTokenSource? _blinkingCancellationToken;

        public StatusLedService()
        {
            _onboardLed = new RgbPwmLed(
                redPwmPin: MeadowApp.Device.Pins.OnboardLedRed,
                greenPwmPin: MeadowApp.Device.Pins.OnboardLedGreen,
                bluePwmPin: MeadowApp.Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
        }

        public void StartBlinking(Color color)
        {
            _blinkingCancellationToken?.Cancel();
            _blinkingCancellationToken = new CancellationTokenSource();

            var task = new Task(async () =>
            {
                while (!_blinkingCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _onboardLed.SetColor(color);
                        await Task.Delay(TimeSpan.FromMilliseconds(250), _blinkingCancellationToken.Token);
                        _onboardLed.Stop();
                        await Task.Delay(TimeSpan.FromMilliseconds(250), _blinkingCancellationToken.Token);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }

                _onboardLed.Stop();
            });

            task.Start();
        }

        public void StopBlinking()
        {
            _blinkingCancellationToken?.Cancel();
        }
    }
}
