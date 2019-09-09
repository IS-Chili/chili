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
    /// This controller handles and displays the error page.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// This view displays the error page.
        /// </summary>
        /// <returns>Shared/Error view</returns>
        [Route("Error/{errCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int errCode)
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
