using Aron.GrassMiner.Jobs;
using SoftEtherVPNCmdNETCore.VPNClient;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            string vpnName = "test";
            VpnClient vpnClient = new VpnClient(@"C:\Program Files\SoftEther VPN Client\vpncmd.exe");

            await vpnClient.AccountCreate(vpnName, "aronhome.com:1194", "ncku", "vms2", "VPN5");
            await vpnClient.AccountPasswordSet(vpnName, "JIHc76aasf614dcStgnEws55c4dvbnn", AuthenticationType.Standard);
            await vpnClient.AccountConnect(vpnName);

            Assert.Pass();
        }


    }
}