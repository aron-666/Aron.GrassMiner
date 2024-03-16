using GrassMiner.Models;
using GrassMiner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrassMiner.Controllers
{
    [Authorize]
    public class MinerController : Controller
    {
        private readonly MinerRecord _minerRecord;
        private readonly MinerService _minerService;

        public MinerController(MinerRecord minerRecord, MinerService minerService) 
        {
            this._minerRecord = minerRecord;
            this._minerService = minerService;
        }
        public IActionResult Index()
        {

            return View(_minerRecord);
        }

        public IActionResult Stop()
        {
            _minerService?.Stop();
            return RedirectToAction("");
        }

        public IActionResult Start() 
        {
            _minerService?.Start();
            return RedirectToAction("");
        }

        
        public MinerRecord GetMinerRecord()
        {
            return _minerRecord;
        }
    }
}
