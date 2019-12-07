// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Usa.chili.Web.Converters
{
    /// <summary>
    /// Converts all doubles to a string with two decimal places in JSON.
    /// </summary>
    public class DoubleConverter : JsonConverter<double?>
    {
        // Converts string to a double
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(double?));
            return double.Parse(reader.GetString());
        }

        // Converts double to a Fixed-point string (Rounds up)
        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value != null ? value.Value.ToString("F", CultureInfo.InvariantCulture) : null);
        }
    }
}