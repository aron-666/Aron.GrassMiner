using Aron.GrassMiner.Jobs;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            var s =VPNJob.GetDefaultGateway("VPN - VPN Client");
        }
    }
}