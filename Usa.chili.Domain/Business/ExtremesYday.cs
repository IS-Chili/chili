// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using Usa.chili.Common;

namespace Usa.chili.Domain
{
    public partial class ExtremesYday
    {
        public ExtremesYday ConvertUnits(bool isMetricUnits)
        {
            // Prepare English unit versions of the "Yesterday's Extremes" data values
            if (!isMetricUnits)
            {
                // Total Precipitation
                if (PrecipTb3Today != null)
                {
                    PrecipTb3Today = PrecipTb3Today * Constant.mm2Inches;
                }
                // Maximum Wind Speed at 10m
                if (WndSpd10mMax != null)
                {
                    WndSpd10mMax = WndSpd10mMax * Constant.mps2Mph;
                }
                // Maximum Air Temperature at 2m
                if (AirT2mMax != null)
                {
                    AirT2mMax = Math.Round(Constant.nineFifths * (AirT2mMax ?? 0) + 32, 2);
                }
                // Minimum Air Temperature at 2m
                if (AirT2mMin != null)
                {
                    AirT2mMin = Math.Round(Constant.nineFifths * (AirT2mMin ?? 0 + 32), 2);
                }
                // Maximum Dew Point at 2m
                if (DewPt2mMax != null)
                {
                    DewPt2mMax = Math.Round(Constant.nineFifths * (DewPt2mMax ?? 0) + 32, 2);
                }
                // Minimum Dew Point at 2m
                if (DewPt2mMin != null)
                {
                    DewPt2mMin = Math.Round(Constant.nineFifths * (DewPt2mMin ?? 0) + 32, 2);
                }
            }

            return this;
        }
    }
}
