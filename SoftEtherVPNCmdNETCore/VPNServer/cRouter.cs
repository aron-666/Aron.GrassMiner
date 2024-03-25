using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cRouter
    {
        public string layer3SwitchName { get; set; }
        public string runningStatus { get; set; } //TODO: Maybe bool
        public string interfaces { get; set; }
        public string routingTables { get; set; }
    }
}
