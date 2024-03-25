using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEtherVPNCmdNETCore.VPNClient
{
    public class VpnClient
    {
        private readonly cSoftEtherVPNCmdNETCore cmd = new cSoftEtherVPNCmdNETCore();

        private readonly string host = "localhost";
        private readonly string? password = null;
        
        public VpnClient(string host, string password, string vpncmd)
        {
            this.host = host;
            this.password = password;
            cmd.Binary = vpncmd;
        }

        public VpnClient(string host, string vpncmd)
        {
            this.host = host;
            cmd.Binary = vpncmd;
        }

        public VpnClient(string vpncmd)
        {
            cmd.Binary = vpncmd;
        }

        /// <summary>
        /// 显示版本信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> About()
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "About", "", password: password);

            return output;
        }

        /// <summary>
        /// 设定连接设置的用户认证种类为匿名认证
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountAnonymousSet(string name)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取用于连接设置的客户端证书
        /// </summary>
        /// <param name="name"></param>
        /// <param name="saveCert"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountCertGet(string name, string saveCert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置连接设置的用户认证类型为用户证书认证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadCert"></param>
        /// <param name="loadKey"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountCertSet(string name, string loadCert, string loadKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 禁用连接设置进行通信时的数据压缩
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountCompressDisable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启用连接设置进行通信时的数据压缩
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountCompressEnable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用连接设置，开始连接 VPN Server
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountConnect(string name)
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountConnect", name, password: password);
        }

        /// <summary>
        /// 创建新的连接设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="server"></param>
        /// <param name="hub"></param>
        /// <param name="username"></param>
        /// <param name="nicName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountCreate(string name, string server, string hub, string username, string nicName)
        {
            string output = cmd.ExecuteCommand(host, "CLIENT", "AccountCreate", $"{name} /SERVER:{server} /HUB:{hub} /USERNAME:{username} /NICNAME:{nicName}", password: password).Result;
        }

        /// <summary>
        /// 删除连接设置
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountDelete(string name)
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountDelete", name, password: password);
        }

        /// <summary>
        /// 设置接续设置的高级通信设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxTCP"></param>
        /// <param name="interval"></param>
        /// <param name="ttl"></param>
        /// <param name="half"></param>
        /// <param name="bridge"></param>
        /// <param name="monitor"></param>
        /// <param name="noTrack"></param>
        /// <param name="noQos"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountDetailSet(string name, int maxTCP, int interval, int ttl, bool half, bool bridge, bool monitor, bool noTrack, bool noQos)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 断开连接中的连接设置
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountDisconnect(string name)
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountDisconnect", name, password: password);
        }

        /// <summary>
        /// 禁用连接设置进行通信时的加密
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountEncryptDisable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启用连接设置进行通信的加密
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountEncryptEnable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 导出连接设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="savePath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountExport(string name, string savePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得连接设置的设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<VpnSettings> AccountGet(string name)
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountGet", name, password: password);
            var dict = cmd.ParseCommand(output);
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (var pair in dict)
            {
                newDict.Add(pair.Key.Replace(" ", "").Replace("/", ""), pair.Value);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(newDict);
            VpnSettings settings = Newtonsoft.Json.JsonConvert.DeserializeObject<VpnSettings>(json);
            return settings;
        }

        /// <summary>
        /// 导入连接设置
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountImport(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得连接设置列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Account>> AccountList()
        {
            
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountList", "", password: password);

            List<Account> accounts = new List<Account>();

            using (StringReader reader = new StringReader(output))
            {
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    // Skip header
                    parser.ReadLine();

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        Account account = new Account
                        {
                            VPNConnectionSettingName = fields[0],
                            Status = Enum.Parse<AccountStatus>(fields[1]),
                            VPNServerHostname = fields[2],
                            VirtualHub = fields[3],
                            VirtualNetworkAdapterName = fields[4]
                        };

                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        /// <summary>
        /// 设置连接设置时使用的虚拟 LAN 卡
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nicName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountNicSet(string name, string nicName)
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountNicSet", $"{name} /NICNAME:{nicName}", password: password);

            

        }

        /// <summary>
        /// 设定连接设置的用户证类型为密码认证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AccountPasswordSet(string name, string password, AuthenticationType type)
        {
            throw new NotImplementedException();
            string output = await cmd.ExecuteCommand(host, "CLIENT", "AccountPasswordSet", $"{name} /PASSWORD:{password} /TYPE:{type}", password: password);

        }

        /// <summary>
        /// 将连接设置的连接方法设置为通过 HTTP 代理服务器连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="server"></param>
        /// <param name="password"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountProxyHttp(string name, string server, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将连接设置的连接方法直接设置为 TCP/IP 连接
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountProxyNone(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将连接设置的连接方法设置为通过 SOCKS 代理服务器连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="server"></param>
        /// <param name="password"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountProxySocks(string name, string server, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更改连接设置名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountRename(string name, string newName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置连接设置的连接失败或断开时建立重新连接的次数和间隔
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <param name="interval"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountRetrySet(string name, int num, int interval)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将连接设置的用户认证类型设置为智能卡认证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="certName"></param>
        /// <param name="keyName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountSecureCertSet(string name, string certName, string keyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除连接设置的服务器固有证书
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountServerCertDelete(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 禁用连接设置服务器证书验证选项
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountServerCertDisable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启用连接设置服务器证书验证选项
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountServerCertEnable(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取连接设置的服务器固有证明书
        /// </summary>
        /// <param name="name"></param>
        /// <param name="saveCert"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountServerCertGet(string name, string saveCert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置连接设置的服务器固有证明书
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadCert"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountServerCertSet(string name, string loadCert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设定连接设置连接终端
        /// </summary>
        /// <param name="name"></param>
        /// <param name="server"></param>
        /// <param name="hub"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountSet(string name, string server, string hub)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解除连接设置的启动连接
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountStartupRemove(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设定连接设置的启动连接
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountStartupSet(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前连接设置的状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> AccountStatusGet(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置成在连接到 VPN Server 时不显示连接状态和错误的画面
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountStatusHide(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置成在连接到 VPN Server 时显示连接状态和错误的画面
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountStatusShow(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置用于连接的连接设置的用户名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AccountUsernameSet(string name, string username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加信任的证明机构的证书
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CertAdd(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除信任的证明机构的证书
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CertDelete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得信任的证明机构的证书
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveCertPath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CertGet(string id, string saveCertPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取信任的证明机构的证书列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> CertList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检测 SoftEther VPN 是否能正常运行
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Check()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 禁用保持互联网连接功能
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void KeepDisable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启动 Internet 保持连接功能
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void KeepEnable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取保持互联网连接的功能
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> KeepGet()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置 Internet 保持连接功能
        /// </summary>
        /// <param name="host"></param>
        /// <param name="protocol"></param>
        /// <param name="interval"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void KeepSet(string host, Protocol protocol, int interval)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建新的 X.509 证书和密钥 (1024 位)
        /// </summary>
        /// <param name="certificateName"></param>
        /// <param name="organization"></param>
        /// <param name="organizationUnit"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="locale"></param>
        /// <param name="expires"></param>
        /// <param name="saveCert"></param>
        /// <param name="saveKey"></param>
        /// <param name="signKey"></param>
        /// <param name="signCert"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void MakeCert(string certificateName, string organization, string organizationUnit, string country, string state, string locale, int expires, string saveCert, string saveKey, string signKey = "", string signCert = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新的虚拟 LAN 卡的创建
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task NicCreate(string name)
        {
            await cmd.ExecuteCommand(host, "CLIENT", "NicCreate", name, password: password);
        }

        /// <summary>
        /// 删除虚拟 LAN 卡
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task NicDelete(string name)
        {
            await cmd.ExecuteCommand(host, "CLIENT", "NicDelete", name, password: password);
        }

        /// <summary>
        /// 禁用虚拟 LAN 卡
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task NicDisable(string name)
        {
            await cmd.ExecuteCommand(host, "CLIENT", "NicDisable", name, password: password);
        }

        /// <summary>
        /// 启用虚拟 LAN 卡
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task NicEnable(string name)
        {
            await cmd.ExecuteCommand(host, "CLIENT", "NicEnable", name, password: password);
        }

        /// <summary>
        /// 获取虚拟 LAN 卡的设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> NicGetSetting(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取虚拟 LAN 卡列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Nic>> NicList()
        {
            string output = await cmd.ExecuteCommand(host, "CLIENT", "NicList", "", password: password);
            List<Nic> nics = new List<Nic>();
            using (StringReader reader = new StringReader(output))
            using (TextFieldParser parser = new TextFieldParser(reader))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // 跳过标题行
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    Nic nic = new Nic
                    {
                        VirtualNetworkAdapterName = fields[0],
                        Status = fields[1].Equals("Enabled", StringComparison.OrdinalIgnoreCase),
                        MacAddress = fields[2],
                        Version = fields[3]
                    };

                    nics.Add(nic);
                }
            }
            return nics;
        }

        /// <summary>
        /// 更改虚拟 LAN 卡设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mac"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void NicSetSetting(string name, string mac)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 升级虚拟 LAN 卡设备驱动
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void NicUpgrade(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取为连接到 VPN 客户服务的密码的设定
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> PasswordGet()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置为连接到 VPN 客户服务的密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="remoteOnly"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PasswordSet(string password, bool remoteOnly = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 禁止 VPN 客户服务的远程管理
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoteDisable()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 启用 VPN 客户服务的远程管理
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoteEnable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取使用的智能卡种类的 ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> SecureGet()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取可用的智能卡种类列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SecureCard> SecureList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 选择要使用的智能卡种类
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SecureSelect(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在用户模式下，运行网络流量速度测试工具
        /// </summary>
        /// <param name="hostPort"></param>
        /// <param name="numTCP"></param>
        /// <param name="type"></param>
        /// <param name="span"></param>
        /// <param name="doubleResult"></param>
        /// <param name="raw"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void TrafficClient(string hostPort, int numTCP, TrafficClientType type, int span, bool doubleResult, bool raw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在服务器模式下，运行网络流量速度测试工具
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void TrafficServer(int port)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取 VPN 客户服务的版本信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Dictionary<string, string> VersionGet()
        {
            throw new NotImplementedException();
        }
    }
}
