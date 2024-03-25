using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cHub
    {
        public string virtualHubName { get; set; }
        public string status { get; set; } //TODO: Maybe it should be enum
        public string type { get; set; } //TODO: Should be enum {Standalone, Static, Dynamic} maybe others also
        public int users { get; set; }
        public int groups { get; set; }
        public int sessions { get; set; }
        public int macTables { get; set; }
        public int ipTables { get; set; }
        public int numLogins { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime lastCommunication { get; set; }
        public string transferBytes { get; set; }
        public string transferPackets { get; set; }
    }
}
