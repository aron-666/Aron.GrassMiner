using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cConnection
    {
        public string connectionName { get; set; }
        public string connectionSource { get; set; }
        public string connectionStart { get; set; } //TODO: Maybe it should be DATETIME
        public string type { get; set; } //TODO: Maybe should be enum type
    }
}
