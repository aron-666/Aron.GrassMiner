using GrassMiner.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Quartz;
using SoftEtherVPNCmdNETCore.VPNClient;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Aron.GrassMiner.Jobs
{
    public class VPNJob(MinerRecord _minerRecord) : IJob
    {
        private readonly string nicName = "VPN100";
        private string ethName => "vpn_" + nicName.ToLower();
        private readonly string hubName = "VPNGATE";
        private readonly string userName = "vpn";
        private readonly string accountName = "DynamicVPN";
        private string vpnHost = "1.229.239.54";
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                
                
                VpnClient vpnClient = new VpnClient("/vpnclient/vpncmd");
                

                var AccountList = await vpnClient.AccountList();
                if (AccountList.Any(x => x.VPNConnectionSettingName == accountName))
                {
                    await vpnClient.AccountDisconnect(accountName);
                    await vpnClient.AccountDelete(accountName);
                }

                var nicList = await vpnClient.NicList();
                if (nicList.Any(x => x.VirtualNetworkAdapterName == nicName))
                {
                    await vpnClient.NicDelete(nicName);
                }

                await vpnClient.NicCreate(nicName);

                await vpnClient.AccountCreate(accountName, vpnHost + ":995", hubName, userName, nicName);

                await vpnClient.AccountConnect(accountName);

                //run lsattr /etc/resolv.conf
                Process process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/usr/bin/lsattr",
                        Arguments = "/etc/resolv.conf",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                process.WaitForExit();
                process.Dispose();

                SpinWait.SpinUntil(() =>
                {
                    var accountList = vpnClient.AccountList().GetAwaiter().GetResult();
                    Thread.Sleep(1000);
                    return accountList.Any(x => x.VPNConnectionSettingName == accountName && x.Status == AccountStatus.Connected);
                }, 30000);
                Thread.Sleep(1000);

                
                //run dhclient vpn_vpn100
                process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/sbin/dhclient",
                        Arguments = ethName,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                process.WaitForExit();
                process.Dispose();
                Thread.Sleep(5000);

                Console.WriteLine($"eth0: {GetDefaultGateway("eth0")}");
                Console.WriteLine($"vpn_vpn100: {GetVPNDefaultGateway()}");

                // run ip route add 1.232.205.19/32 via 172.19.0.1 dev eth0 
                //ip route del default
                //ip route add default via 10.211.1.1 dev vpn_vpn100

                process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/sbin/ip",
                        Arguments = $"route add {vpnHost}/32 via {GetDefaultGateway("eth0")} dev eth0"
                    }
                };
                Console.WriteLine(process.StartInfo.Arguments);

                process.Start();
                process.WaitForExit();
                process.Dispose();

                process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/sbin/ip",
                        Arguments = $"route del default"
                    }
                };
                Console.WriteLine(process.StartInfo.Arguments);

                process.Start();
                process.WaitForExit();
                process.Dispose();

                process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/sbin/ip",
                        Arguments = $"route add default via {GetVPNDefaultGateway()} dev {ethName}"
                    }
                };
                Console.WriteLine(process.StartInfo.Arguments);



                process.Start();
                process.WaitForExit();
                process.Dispose();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        public static string? GetVPNDefaultGateway()
        {
            string content = File.ReadAllText("/var/lib/dhcp/dhclient.leases");
            string pattern = @"option routers\s+([\d.]+);";
            Match match = Regex.Match(content, pattern);

            if (match.Success)
            {
                string routersAddress = match.Groups[1].Value;
                return routersAddress;
            }
            else
            {
                return null;
            }

        }

        public static IPAddress GetDefaultGateway(string name)
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(x => x.Name == name)
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                // .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                // .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .FirstOrDefault();
        }
    }
}
