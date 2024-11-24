using Aron.GrassMiner.Models;
using Aron.GrassMiner.Services;
using OpenQA.Selenium;
using System.Drawing;
using System.Net;
using System.Xml.Linq;

namespace Aron.GrassMiner.Jobs
{
    public class ScreensShotJob(MinerRecord _minerRecord, IMinerService minerService) : IHostedService, IDisposable
    {

        private Timer _timer;
        public int Interval { get; } = 5000;

        public void Execute(object state)
        {
            try
            {
                if (minerService.driver == null)
                {
                    return;
                }


                // 截圖
                Screenshot screenshot = ((ITakesScreenshot)minerService.driver).GetScreenshot();
                _minerRecord.Base64Image = "data:image/png;base64," + screenshot.AsBase64EncodedString;

            }
            catch (Exception e)
            {
            }
            return;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(Interval));
            return Task.CompletedTask;
        }

        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}
