// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Usa.chili.Dto;

namespace Usa.chili.Web
{
    /// <summary>
    /// Extension to provide custom exception handling in Startup.
    /// </summary>
    public static class GlobalExceptionHandlerExtension
    {

        /// <summary>
        /// This method will globally handle logging unhandled exceptions.
        /// It will respond with a JSON response for AJAX calls that send the JSON accept header
        /// otherwise it will redirect to the error page.
        /// </summary>
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    // Get InternalServerError status code
                    int statusCode = (int)HttpStatusCode.InternalServerError;

                    // Get response status code
                    context.Response.StatusCode = statusCode;

                    // Log the exception
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                    logger.LogError(exception, "Unexpected Error");

                    // Check if response is expecting JSON
                    var matchText = "JSON";
                    bool requiresJsonResponse = context.Request.GetTypedHeaders().Accept
                        .Any(t => t.Suffix.Value?.ToUpper() == matchText || t.SubTypeWithoutSuffix.Value?.ToUpper() == matchText);

                    // Return a JSON response or redirect to the error page
                    if (requiresJsonResponse)
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto { StatusCode = statusCode }), Encoding.UTF8);
                    }
                    else
                    {
                        context.Response.Redirect("/Error/" + statusCode);
                        await Task.CompletedTask;
                    }
                });
            });
        }
    }
}