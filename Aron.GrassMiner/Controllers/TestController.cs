using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftEtherVPNCmdNETCore.VPNClient;

namespace Aron.GrassMiner.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            VpnClient vpnClient = new VpnClient("/vpnclient/vpncmd");
            
            return Ok( await vpnClient.About());
        }
    }
}
