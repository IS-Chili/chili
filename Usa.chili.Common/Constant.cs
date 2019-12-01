// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

namespace Usa.chili.Common
{
    /// <summary>
    /// The class holds all the common constants used between layers/tiers.
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// Standard datetime format for the application
        /// </summary>
        public const string DATETIME_FORMAT = "MM/dd/yyyy HH:mm:ss";

        // Define some useful conversion constants
        public const double fiveNinths = 5 / 9;
        public const double nineFifths = 9 / 5;
        public const double thirtyTwoPointTwo = 32.2;
        public const double mm2Inches = 0.0393700787;
        public const double mb2InHg = 0.029529983071;
        public const double wpsqm2lymin = 0.00143197;
        public const double mps2Mph = 2.2369363;

        // Define constants for Dew Point calculation (Deg C)
        public const double dpA = 17.271;
        public const double dpB = 237.7;
        
        // Define constants for Heat Index calculation (Deg F)
        public const double hi1 = 42.379;
        public const double hi2 = 2.04901523;
        public const double hi3 = 10.14333127;
        public const double hi4 = 0.22475541;
        public const double hi5 = 0.00683783;
        public const double hi6 = 0.05481717;
        public const double hi7 = 0.00122874;
        public const double hi8 = 0.00085282;
        public const double hi9 = 0.00000199;
        // Define constants for Wind Chill calculation (Deg F)
        public const double wc1 = 35.74;
        public const double wc2 = 0.6215;
        public const double wc3 = 35.75;
        public const double wc4 = 0.4275;
    }
}
