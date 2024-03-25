using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNClient
{
    public class Nic
    {
        public string VirtualNetworkAdapterName { get; set; }
        public bool Status { get; set; }
        public string MacAddress { get; set; }
        public string Version { get; set; }
    }
}
