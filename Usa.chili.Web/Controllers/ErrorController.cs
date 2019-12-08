// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Microsoft.AspNetCore.Mvc;
using Usa.chili.Dto;

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
        /// <param name="statusCode">The HTTP status code number</param>
        /// <returns>Shared/Error view with a ErrorDto</returns>
        [Route("Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorDto { StatusCode = statusCode });
        }
    }
}
