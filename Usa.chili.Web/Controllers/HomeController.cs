// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Microsoft.AspNetCore.Mvc;

namespace Usa.chili.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("Donate")]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpGet("MesonetInformation")]
        public IActionResult MesonetInformation()
        {
            return View();
        }

        // TODO: Move to home page
        [HttpGet("StationMap")]
        public IActionResult StationMap()
        {
            return View();
        }
    }
}