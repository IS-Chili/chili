// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Usa.chili.Common;

namespace Usa.chili.Domain
{
    /// <summary>
    /// Handle business logic for Public.
    /// </summary>
    public partial class Public
    {
        // Calculated Relative Humidity
        [NotMapped()]
        public double? Rh { get; set; }

        // Calculated Dew Point
        [NotMapped()]
        public double? DewPoint { get; set; }

        // Calculated Felt
        [NotMapped()]
        public double? Felt { get; set; }

        // Determines if Felt is Windchill or Heatindex
        [NotMapped()]
        public bool IsWindChill { get; set; }

        /// <summary>
        /// Normalizes relative humidity 
        /// </summary>
        /// <returns>Normalized relative humidity</returns>
        public double? NormalizeRelativeHumidity()
        {
            if (Rh2m > 110)
                return null;
            else if (Rh2m > 100 && Rh2m <= 110)
                return 100.0;
            else if (Rh2m >= -1 && Rh2m < 0)
                return 0;
            else if (Rh2m < -1)
                return null;
            else
                return Rh2m;
        }

        /// <summary>
        /// Calculates dew point
        /// </summary>
        /// <returns>Dew point</returns>
        public double? CalculateDewPoint()
        {
            double? normalizedRelativeHumidity = NormalizeRelativeHumidity();
            if (normalizedRelativeHumidity == null)
                return null;
            else
                return (AirT2m ?? 0) - ((100.0 - normalizedRelativeHumidity) / 5.0);
        }

        /// <summary>
        /// Performs any necessary calculations and conversions
        /// </summary>
        /// <param name="isMetricUnits">Returns data in metric units if true, english units if false</param>
        /// <param name="isWindChill">Returns windchill data if true, heatindex data if false</param>
        /// <returns>Public with calculations and conversions performed</returns>
        public Public ConvertUnits(bool isMetricUnits, bool? isWindChill)
        {
            // If Wind Direction is slightly negative adjust it
            if (WndDir10m != null && WndDir10m < 0)
            {
                if (WndDir10m < -2)
                {
                    WndDir10m = null;
                }
                else
                {
                    WndDir10m += 360;
                }
            }

            DewPoint = CalculateDewPoint();
            Rh = NormalizeRelativeHumidity();

            // Define constants for Dew Point calculation (Deg C)
            double a = 17.271;
            double b = 237.7;

            // Calculate Dew point for "Current" data values
            // Dew Point at 2m
            if (AirT2m == null ||
                Rh == null ||
                AirT2m >= 60 ||
                // AirT2m < 0 ||
                Rh > 100 ||
                Rh < 1)
            {
                DewPoint = null;
            }
            else
            {
                // Define constants for Dew Point calculation (Deg C)
                if (AirT2m < 0)
                {
                    a = 22.452;
                    b = 272.55;
                }
                else
                {
                    a = 17.502;
                    b = 240.97;
                }
                double gamma = ((a * (AirT2m ?? 0)) / (b + (AirT2m ?? 0))) + Math.Log((Rh ?? 0) / 100.0);
                DewPoint = (b * gamma) / (a - gamma);
            }

            double AirT2m_en = Math.Round(Constant.nineFifths * (AirT2m ?? 0) + 32, 2);
            double DewPoint_en = (DewPoint ?? 0) * Constant.nineFifths + 32;
            double WndSpd10m_en = (WndSpd10m ?? 0) * Constant.mps2Mph;

            // Prepare English unit versions of the "Current" data values
            if (!isMetricUnits)
            {
                // Air Temperature at 2m
                if (AirT2m != null)
                {
                    AirT2m = AirT2m_en;
                }
                // Dew Point at 2m
                if (DewPoint != null)
                {
                    DewPoint = DewPoint_en;
                }
                // Wind Speed at 10m
                if (WndSpd10m != null)
                {
                    WndSpd10m = WndSpd10m_en;
                }
                // Sea Level Pressure
                if (PressSealev1 != null)
                {
                    PressSealev1 = PressSealev1 * Constant.mb2InHg;
                }
                // Total Precipitation Today
                if (PrecipTb3Today != null)
                {
                    PrecipTb3Today = PrecipTb3Today * Constant.mm2Inches;
                }
            }

            // If neither Heat Index nor Wind Chill was specified
            // then choose one based on current conditions
            if (!isWindChill.HasValue && AirT2m != null && WndSpd10m != null)
            {
                // Windchill Temperature is only defined for temperatures
                // at or below 50 degrees F and wind speeds above 3 mph
                if (AirT2m_en <= 50 && WndSpd10m_en > 3)
                {
                    this.IsWindChill = true;
                    isWindChill = true;
                }
                else
                {
                    this.IsWindChill = false;
                    isWindChill = false;
                }
            }
            else {
                this.IsWindChill = isWindChill.Value;
            }

            // Calculate either the Heat Index OR the Windchill for each station
            if (isWindChill.HasValue && !isWindChill.Value)
            {
                // Heat Index calculation per NWS
                if (AirT2m == null ||
                    DewPoint == null ||
                    Rh == null ||
                    AirT2m_en <= 80 ||
                    DewPoint_en <= 54 ||
                    Rh <= 40)
                {
                    Felt = null;
                }
                else
                {
                    double t = AirT2m_en;
                    double t2 = t * t;
                    double r = Rh ?? 0;
                    double r2 = r * r;
                    Felt = Constant.hi2 * t +
                            Constant.hi3 * r -
                            Constant.hi4 * t * r -
                            Constant.hi5 * t2 -
                            Constant.hi6 * r2 +
                            Constant.hi7 * t2 * r +
                            Constant.hi8 * t * r2 -
                            Constant.hi9 * t2 * r2 - Constant.hi1;
                    if (Felt < AirT2m_en)
                    {
                        Felt = null;
                    }
                }
            }
            else
            {
                // Windchill calculation per NWS
                if (AirT2m == null || WndSpd10m == null)
                {
                    Felt = null;
                }
                else if (AirT2m_en > 50.0 || WndSpd10m_en <= 3.0)
                {
                    Felt = null;
                }
                else {
                    Felt =  Constant.wc1 +
                            Constant.wc2 * AirT2m_en -
                            Constant.wc3 * Math.Pow(WndSpd10m_en, 0.16) +
                            Constant.wc4 * AirT2m_en * Math.Pow(WndSpd10m_en, 0.16);
                }
            }

            if (Felt != null)
            {
                if (isMetricUnits)
                {
                    Felt = Math.Round(((Felt ?? 0) - 32) * Constant.fiveNinths, 2);
                }
            }

            return this;
        }
    }
}
