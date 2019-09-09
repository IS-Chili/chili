// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Usa.chili.Web
{
    /// <summary>
    /// Defines extensions for the Html Helper class, for use in Razor views.
    /// </summary>
    public static class HtmlHelperExtensions
    {

        /// <summary>
        /// Extension method to build date string for nullable DateTime.
        /// </summary>
        /// <param name="htmlHelper">Extension method for.</param>
        /// <param name="datetime">The datetime that will used to generate string.</param>
        /// <returns>String representing date or empty string if the datetime parameter is null.</returns>
        public static IHtmlContent FormatDate(this IHtmlHelper htmlHelper, DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return new HtmlString(datetime.Value.ToString(Constant.DATE_FORMAT));
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        /// <summary>
        /// Extension method to build date string for DateTime.
        /// </summary>
        /// <param name="htmlHelper">Extension method for.</param>
        /// <param name="datetime">The datetime that will used to generate string.</param>
        /// <returns>String representing date or empty string if the datetime is set to MinValue.</returns>
        public static IHtmlContent FormatDate(this IHtmlHelper htmlHelper, DateTime datetime)
        {
            if (datetime.Equals(DateTime.MinValue))
            {
                return new HtmlString(string.Empty);
            }
            else
            {
                return new HtmlString(datetime.ToString(Constant.DATE_FORMAT));
            }
        }

        /// <summary>
        /// Extension method to build date string for Decimal.
        /// </summary>
        /// <param name="htmlHelper">Extension method for.</param>
        /// <param name="number">The decimal that will used to generate string.</param>
        /// <returns>String representing date or empty string if the decimal is set to MinValue.</returns>
        public static IHtmlContent FormatDecimal(this IHtmlHelper htmlHelper, decimal number)
        {
            if (number.Equals(Decimal.MaxValue))
            {
                return new HtmlString(string.Empty);
            }
            else
            {
                return new HtmlString(number.ToString("0.00"));
            }
        }
    }
}
