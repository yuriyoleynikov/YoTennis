using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoTennis.Services;

namespace YoTennis.Controllers
{
    public class MatchController : Controller
    {
        private IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }
        
        public IActionResult Create()
        {
            return Content("ok");
        }

        public async Task<IActionResult> Index()
        {
            var state = await _matchService.GetState();
            if (state.State == Models.MatchState.NotStarted)
                return View("NotStarted", state);
            return View();
        }
    }
}