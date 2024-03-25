using SoftEtherVPNCmdNETCore.VPNClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public interface iVPNServer
    {
        void About();
        Dictionary<string, string> ServerInfoGet();
        Dictionary<string, string> ServerStatusGet();
        void ListenerCreate(int port);
        void ListenerDelete(int port);
        List<cListener> ListenerList();
        void ListenerEnable(int port);
        void ListenerDisable(int port);
        void ServerPasswordSet(string password);
        //TODO: Check all Cluster commands by creating cluster for their output
        Dictionary<string, string> ClusterSettingGet();
        void ClusterSettingStandalone();
        void ClusterSettingController(int weight, bool only);
        void ClusterSettingMember(string serverPort, string ip, string ports, string password, int weight);
        object ClusterMemberList(); 
        object ClusterMemberInfoGet(string id);
        void ClusterMemberCertGet(string id, string saveCert);
        object ClusterConnectionStatusGet();
        //TODO END
        void ServerCertGet(string cert);
        void ServerKeyGet(string key);
        void ServerCertSet(string loadCert, string loadKey);
        cServerCipher ServerCipherGet(); //TODO: CSV not supported must parse text and fill cServerCipher
        void ServerCipherSet(string name);
        string Debug(string id, string arg);
        void Crash(bool yes);
        void Flush();
        void KeepEnable();
        void KeepDisable();
        void KeepSet(string hostPort, Protocol protocol, int interval);
        Dictionary<string, string> KeepGet();
        void SyslogEnable(int level, string hostPort);
        void SyslogDisable();
        Dictionary<string, string> SyslogGet();
        List<cConnection> ConnectionList();
        Dictionary<string, string> ConnectionGet(string name);
        void ConnectionDisconnect(string name);
        List<string> BirdgeDeviceList();
        List<cBridge> BridgeList();
        void BridgeCreate(string hubName, string device, bool tap);
        void BridgeDelete(string hubName, string device);
        Dictionary<string, string> Caps();
        void Reboot(bool resetConfig = false);
        void ConfigGet(string path);
        void ConfigSet(string path);
        List<cRouter> RouterList();
        void RouterAdd(string name);
        void RouterDelete(string name);
        void RouterStart(string name);
        void RouterStop(string name);
        List<cRouterInterface> RouterIfList(string name);
        void RouterIfAdd(string name, string hub, string ipWithSubnetMask);
        void RouterIfDel(string name, string hub);
        List<cRouteTableEntry> RouterTableList(string name);
        void RouterTableAdd(string name, string network, string gateway, int metric);
        void RouterTableDel(string name, string network, string gateway, int metric);
        List<cLogFile> LogFileList();
        void LogFileGet(string name, string server, string savePath);
        void HubCreate(string name, string password);
        void HubCreateDynamic(string name, string password);
        void HubCreateStatic(string name, string password);
        void HubDelete(string name);
        void HubSetStatic(string name);
        void HubSetDynamic(string name);
        List<cHub> HubList();
        void Hub(string name);
        void MakeCert(string certificateName, string organization, string organizationUnit, string country, string state, string locale, int expires, string saveCert, string saveKey, string signKey = "", string signCert = "");
        void TrafficClient(string hostPort, int numTCP, TrafficClientType type, int span, bool doubleResult, bool raw);
        //TODO: Can be tricky to be used as this command does not exits until enter is sent.
        void TrafficServer(int port);
        void Check();
        void IPsecEnable(bool l2Tp, bool l2TpRaw, bool ethRip, string psk, string defaultHub);
        Dictionary<string, string> IPsecGet();
        void EtherIpClientAdd(string id, string hub, string username, string password);
        void EtherIpClientDelete(string id);
        List<cEtherIpClient> EtherIpClientList();
        void OpenVpnEnable(bool yes, string ports);
        Dictionary<string, string> OpenVpnGet();
        void OpenVpnMakeConfig(string zipFileName);
        void SstpEnable(bool yes);
        Dictionary<string, string> SstpGet();
        void ServerCertRegenerate(string certificateName);
        void VpnOverIcmpDnsEnable(bool icmp, bool dns);
        Dictionary<string, string> VpnOverIcmpDnsGet();
        Dictionary<string, string> DynamicDnsGetStatus();
        void DynamicDnsSetHostname(string hostname);
        Dictionary<string, string> VpnAzureGetStatus();
        void VpnAzureSetEnable(bool yes);
    }
}
