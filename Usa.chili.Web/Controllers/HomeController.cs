// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Microsoft.AspNetCore.Mvc;

namespace Usa.chili.Web.Controllers
{
    /// <summary>
    /// This controller handles and displays the home views.
    /// </summary>
    [Route("")]
    public class HomeController : Controller
    {
        /// <summary>
        /// This view displays the index page.
        /// </summary>
        /// <returns>Home/Index view</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This view displays the about page.
        /// </summary>
        /// <returns>Home/About view</returns>
        [HttpGet("About")]
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// This view displays the donate page.
        /// </summary>
        /// <returns>Home/Donate view</returns>
        [HttpGet("Donate")]
        public IActionResult Donate()
        {
            return View();
        }

        /// <summary>
        /// This view displays the mesonet information page.
        /// </summary>
        /// <returns>Home/MesonetInformation view</returns>
        [HttpGet("MesonetInformation")]
        public IActionResult MesonetInformation()
        {
            return View();
        }
    }
}