// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

namespace Usa.chili.Dto
{
    public class ErrorDto
    {
        public int StatusCode { get; set; }
        public string Message {
            get {
                switch (StatusCode) {
                    case 400:
                        return "Bad request";
                    case 403:
                        return "Forbidden: Not authorized";
                    case 404:
                        return "Page not found";
                    case 408:
                        return "The server timed out";
                    case 500:
                        return "Internal Server Error";
                    default:
                        return "Something happended";
                }
            }
        }
    }
}