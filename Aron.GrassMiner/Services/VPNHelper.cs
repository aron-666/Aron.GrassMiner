using Aron.GrassMiner.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aron.GrassMiner.Services
{
    public class VPNHelper
    {
        public static List<VPNConfig> GetConfigs()
        {
            WebClient client = new WebClient { Encoding = Encoding.UTF8 };
            File.WriteAllText("data.csv", client.DownloadString("http://www.vpngate.net/api/iphone/").Replace("*vpn_servers", ""));

            List<VPNConfig> vpns = new List<VPNConfig>();

            using (TextFieldParser parser = new TextFieldParser("data.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip header
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] fields = parser.ReadFields();

                        string config = Decode(fields[14]);

                        vpns.Add(new VPNConfig
                        {
                            Host = fields[1],
                            Location = fields[5],
                            Ping = Convert.ToInt32(fields[3]),
                            Speed = Convert.ToInt32(fields[4]),
                            Port = GetPort(config),
                            Protocol = GetProtocol(config)
                        });
                    }
                    catch (Exception ex)
                    {
                        // 处理异常
                    }
                }
            }
            return vpns;
        }

        public static string Decode(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        public static int GetPort(string config)
        {
            string[] lines = config.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string port = lines[61].Replace("remote ", "");
            if (port.IndexOf(" ") > 0) port = port.Substring(port.IndexOf(" ") + 1);
            return Convert.ToInt32(port);
        }

        public static string GetProtocol(string config)
        {
            string[] lines = config.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string protocol = lines[40].Substring(6, 3);

            return protocol;
        }
    }
}
