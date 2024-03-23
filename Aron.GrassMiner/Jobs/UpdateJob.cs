using GrassMiner.Models;
using Quartz;
using System.Net;
using System.Xml.Linq;

namespace Aron.GrassMiner.Jobs
{
    public class UpdateJob(MinerRecord _minerRecord) : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                // call https://ifconfig.me to get the public IP address
                try
                {
                    _minerRecord.PublicIp = new WebClient().DownloadString("https://ifconfig.me");
                }
                catch (Exception ex)
                {
                    _minerRecord.PublicIp = "Error to get your public ip.";
                }

                // call https://raw.githubusercontent.com/aron-666/Aron.GrassMiner/master/Aron.GrassMiner/Aron.GrassMiner.csproj to get the latest version
                var latestVersion = new WebClient().DownloadString("https://raw.githubusercontent.com/aron-666/Aron.GrassMiner/master/Aron.GrassMiner/Aron.GrassMiner.csproj");

                _minerRecord.LastAppVersion = parseVersion(latestVersion);
            }
            catch (Exception e)
            {
            }
            return Task.CompletedTask;
            
        }

        private string parseVersion(string xml)
        {
            try
            {
                // 載入 XML 檔案
                XDocument doc = XDocument.Parse(xml);

                // 找到 PropertyGroup 元素
                XElement propertyGroup = doc.Descendants("PropertyGroup").FirstOrDefault();

                if (propertyGroup != null)
                {
                    // 找到 AssemblyVersion 元素
                    XElement assemblyVersionElement = propertyGroup.Element("AssemblyVersion");

                    if (assemblyVersionElement != null)
                    {
                        // 取得 AssemblyVersion 的值
                        string assemblyVersion = assemblyVersionElement.Value;
                        return assemblyVersion;
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }


    }
}
