// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Usa.chili.Common;

namespace Usa.chili.Web.Converters
{
    /// <summary>
    /// Converts all DateTime objects to the correct format string in JSON.
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        // Converts string to a DateTime object
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        // Converts a DateTime object to the correct format string
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Constant.DATETIME_FORMAT));
        }
    }
}