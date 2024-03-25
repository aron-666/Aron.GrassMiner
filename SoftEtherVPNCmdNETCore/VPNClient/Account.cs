using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNClient
{
    public class Account
    {
        public string VPNConnectionSettingName { get; set; }
        public AccountStatus Status { get; set; }
        public string VPNServerHostname { get; set; }
        public string VirtualHub { get; set; }
        public string VirtualNetworkAdapterName { get; set; }
    }
}
