using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNClient
{
    public enum AccountStatus { Connected, Offline, Connecting }
    public enum AuthenticationType { Standard, Radius }
    public enum Protocol { TCP, UDP }
    public enum TrafficClientType { Download, Upload, Full }
}
