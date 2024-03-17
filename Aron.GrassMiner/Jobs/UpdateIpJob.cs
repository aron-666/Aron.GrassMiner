using GrassMiner.Models;
using Quartz;
using System.Net;

namespace Aron.GrassMiner.Jobs
{
    public class UpdateIpJob(MinerRecord _minerRecord) : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                // call https://ifconfig.me to get the public IP address
                _minerRecord.PublicIp = new WebClient().DownloadString("https://ifconfig.me");
            }
            catch (Exception e)
            {
            }
            return Task.CompletedTask;
            
        }


    }
}
