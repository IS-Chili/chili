using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usa.chili.Domain
{
    public partial class Public
    {
        [NotMapped()]
        public double? Rh { get; set; }
        [NotMapped()]
        public double? DewPoint { get; set; }
        [NotMapped()]
        public double? HtIdx { get; set; }
        [NotMapped()]
        public double? Felt { get; set; }

        public double? NormalizeRelativeHumidity()
        {
            if (Rh2m > 110)
                return null;
            else if (Rh2m > 100 && Rh2m <= 110)
                return 100;
            else if (Rh2m >= -1 && Rh2m < 0)
                return 0;
            else if (Rh2m < -1)
                return null;
            else
                return Rh2m;
        }

        public double? CalculateDewPoint()
        {
            double? normalizedRelativeHumidity = NormalizeRelativeHumidity();
            if (normalizedRelativeHumidity == null)
                return null;
            else
                return (AirT2m ?? 0) - ((100 - normalizedRelativeHumidity) / 5);
        }

        public Public ConvertUnits()
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

            // Define some useful conversion constants
            double fiveninths = 5 / 9;
            double ninefifths = 9 / 5;
            double mm2inches = 0.0393700787;
            double mb2inHg = 0.029529983071;
            double wpsqm2lymin = 0.00143197;
            double mps2Mph = 2.2369363;
            // Define constants for Dew Point calculation (Deg C)
            double a = 17.271;
            double b = 237.7;
            // Define constants for Heat Index calculation (Deg F)
            double hi1 = 42.379;
            double hi2 = 2.04901523;
            double hi3 = 10.14333127;
            double hi4 = 0.22475541;
            double hi5 = 0.00683783;
            double hi6 = 0.05481717;
            double hi7 = 0.00122874;
            double hi8 = 0.00085282;
            double hi9 = 0.00000199;
            // Define constants for Wind Chill calculation (Deg F)
            double wc1 = 35.74;
            double wc2 = 0.6215;
            double wc3 = 35.75;
            double wc4 = 0.4275;

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
                double gamma = ((a * AirT2m ?? 0) / (b + AirT2m ?? 0)) + Math.Log(Rh ?? 0 / 100);
                DewPoint = (b * gamma) / (a - gamma);
            }

            string felt = "dy";
            string unit = "en";

            // Prepare English unit versions of the "Current" data values
            // Air Temperature at 2m
            if (unit == "en")
            {
                if (AirT2m != null)
                {
                    AirT2m = Math.Round(ninefifths * AirT2m ?? 0 + 32.2);
                }
                // Dew Point at 2m
                if (DewPoint != null)
                {
                    DewPoint = DewPoint * ninefifths + 32;
                }
                // Wind Speed at 10m
                if (WndSpd10m != null)
                {
                    WndSpd10m = WndSpd10m * mps2Mph;
                }
                // Sea Level Pressure
                if (PressSealev1 != null)
                {
                    PressSealev1 = PressSealev1 * mb2inHg;
                }
                // Total Precipitation Today
                if (PrecipTb3Today != null)
                {
                    PrecipTb3Today = PrecipTb3Today * mm2inches;
                }
            }

            // If neither Heat Index nor Wind Chill was specified
            // then choose one based on current conditions
            if (felt == "dy" && AirT2m != null && WndSpd10m != null)
            {
                // Windchill Temperature is only defined for temperatures
                // at or below 50 degrees F and wind speeds above 3 mph
                if (AirT2m <= 50 && WndSpd10m > 3)
                {
                    felt = "wc";
                }
                else
                {
                    felt = "ht";
                }
            }

            // Calculate either the Heat Index OR the Windchill for each station
            if (felt == "ht")
            {
                // Heat Index calculation per NWS
                if (AirT2m == null ||
                    DewPoint == null ||
                    Rh == null ||
                    AirT2m <= 80 ||
                    DewPoint <= 54 ||
                    Rh <= 40)
                {
                    HtIdx = null;
                }
                else
                {
                    double t = AirT2m ?? 0;
                    double t2 = t * t;
                    double r = Rh ?? 0;
                    double r2 = r * r;
                    double HtIdx = hi2 * t +
                                            hi3 * r -
                                            hi4 * t * r -
                                            hi5 * t2 -
                                            hi6 * r2 +
                                            hi7 * t2 * r +
                                            hi8 * t * r2 -
                                            hi9 * t2 * r2 - hi1;
                    if (HtIdx < AirT2m)
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
                if (AirT2m > 50.0 || WndSpd10m <= 3.0)
                {
                    Felt = null;
                }
                Felt = wc1 +
                        wc2 * AirT2m -
                        wc3 * Math.Pow(WndSpd10m ?? 0, 0.16) +
                        wc4 * AirT2m * Math.Pow(WndSpd10m ?? 0, 0.16);
            }

            if (Felt != null)
            {
                if (unit != "en")
                {
                    Felt = Math.Round(((Felt ?? 0 - 32) * fiveninths), 2);
                }
            }
            else
            {
                Felt = null;
            }

            return this;
        }
    }
}
