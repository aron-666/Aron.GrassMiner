using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cBridge
    {
        public string number { get; set; } //TODO: Maybe change this to number
        public string virtualHubName { get; set; }
        public string networkAdapterOrTapDeviceName { get; set; }
        public string status { get; set; } //TODO: Maybe change this for enum
    }
}
