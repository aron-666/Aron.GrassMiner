using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cRouteTableEntry
    {
        public string networkAddress { get; set; }
        public string subnetMask { get; set; }
        public string gatewayAddress { get; set; }
        public int metric { get; set; }
    }
}
