using GrassMiner.Models;
using GrassMiner.Services;
using OpenQA.Selenium;
using Quartz;
using System.Drawing;
using System.Net;
using System.Xml.Linq;

namespace Aron.GrassMiner.Jobs
{
    public class ExitJob(MinerRecord _minerRecord, IMinerService minerService, IHostApplicationLifetime applicationLifetime) : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (File.Exists("shutdown.flag"))
                {

                    applicationLifetime.StopApplication();
                    File.Delete("shutdown.flag");

                }
            }
            catch (Exception e)
            {
            }
            return Task.CompletedTask;
            
        }

        


    }
}
