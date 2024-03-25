using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cRouterInterface
    {
        public string ipAddress { get; set; }
        public string subnetMask { get; set; }
        public string virtualHubName { get; set; }
    }
}
