// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using CsvHelper;
using CsvHelper.Configuration;

namespace Usa.chili.Web.Converters
{
    /// <summary>
    /// Formats numbers to a length of 9 with 3 decimal places.
    /// </summary>
    public class FixedWidthConverter : CsvHelper.TypeConversion.DoubleConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value != null && value is double?)
            {
                return ((double?)value).Value.ToString("F3").PadLeft(9);
            }
            else if (value != null && value is byte?)
            {
                return ((byte?)value).Value.ToString("F3").PadLeft(9);
            }
            else if (value != null && value is int?)
            {
                return ((int?)value).Value.ToString("F3").PadLeft(9);
            }
            else if (value != null && value is short?)
            {
                return ((short?)value).Value.ToString("F3").PadLeft(9);
            }
            // Set NULL fields to -699.999
            else
            {
                return " -699.999";
            }
        }
    }
}