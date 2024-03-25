using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEtherVPNCmdNETCore.VPNClient
{
    /// <summary>
    /// 表示 VPN 设置。
    /// </summary>
    public class VpnSettings
    {
        /// <summary>
        /// 获取或设置 VPN 连接设置名称。
        /// </summary>
        public string VPNConnectionSettingName { get; set; }

        /// <summary>
        /// 获取或设置目标 VPN 服务器主机名。
        /// </summary>
        public string DestinationVPNServerHostName { get; set; }

        /// <summary>
        /// 获取或设置目标 VPN 服务器端口号。
        /// </summary>
        public int DestinationVPNServerPortNumber { get; set; }

        /// <summary>
        /// 获取或设置目标 VPN 服务器虚拟中心名称。
        /// </summary>
        public string DestinationVPNServerVirtualHubName { get; set; }

        /// <summary>
        /// 获取或设置代理服务器类型。
        /// </summary>
        public string ProxyServerType { get; set; }

        /// <summary>
        /// 获取或设置验证服务器证书选项。
        /// </summary>
        public string VerifyServerCertificate { get; set; }

        /// <summary>
        /// 获取或设置用于连接的设备名称。
        /// </summary>
        public string DeviceNameUsedForConnection { get; set; }

        /// <summary>
        /// 获取或设置身份验证类型。
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置在 VPN 通信中使用的 TCP 连接数。
        /// </summary>
        public int NumberOfTCPConnectionsToUseInVPNCommunication { get; set; }

        /// <summary>
        /// 获取或设置在建立每个 TCP 连接之间的时间间隔。
        /// </summary>
        public int IntervalBetweenEstablishingEachTCPConnection { get; set; }

        /// <summary>
        /// 获取或设置每个 TCP 连接的连接寿命。
        /// </summary>
        public string ConnectionLifeOfEachTCPConnection { get; set; }

        /// <summary>
        /// 获取或设置使用半双工模式选项。
        /// </summary>
        public string UseHalfDuplexMode { get; set; }

        /// <summary>
        /// 获取或设置通过 SSL 加密选项。
        /// </summary>
        public string EncryptionBySSL { get; set; }

        /// <summary>
        /// 获取或设置数据压缩选项。
        /// </summary>
        public string DataCompression { get; set; }

        /// <summary>
        /// 获取或设置通过桥接/路由器模式连接选项。
        /// </summary>
        public string ConnectByBridgeRouterMode { get; set; }

        /// <summary>
        /// 获取或设置通过监控模式连接选项。
        /// </summary>
        public string ConnectByMonitoringMode { get; set; }

        /// <summary>
        /// 获取或设置不调整路由表选项。
        /// </summary>
        public string NoAdjustmentForRoutingTable { get; set; }

        /// <summary>
        /// 获取或设置不使用 QoS 控制功能选项。
        /// </summary>
        public string DoNotUseQoSControlFunction { get; set; }
    }

}
