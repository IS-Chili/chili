// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Common;
using System;

namespace Usa.chili.Domain
{
    /// <summary>
    /// Handle business logic for StationData.
    /// </summary>
    public partial class StationData
    {
        /// <summary>
        /// Gets data for the specified variable with conversion or rounding applied.
        /// </summary>
        /// <param name="isMetricUnits">Returns data in metric units if true, english units if false</param>
        /// <param name="useRound">Returns data with default rounding applied for the variable if true, no rounding if false</param>
        /// <param name="variableTypeEnum">Determines how to convert the value</param>
        /// <param name="variableEnum">Determines the variable value to return</param>
        /// <returns>Value of the variable</returns>
        public double? ValueForVariable(bool isMetricUnits, bool useRound, VariableTypeEnum variableTypeEnum, VariableEnum variableEnum)
        {
            double? value = null;

            switch (variableEnum)
            {
                case VariableEnum.Precip_TB3_Tot:
                    value = this.PrecipTb3Tot;
                    break;
                case VariableEnum.Precip_TX_Tot:
                    value = this.PrecipTxTot;
                    break;
                case VariableEnum.Precip_TB3_Today:
                    value = this.PrecipTb3Today;
                    break;
                case VariableEnum.Precip_TX_Today:
                    value = this.PrecipTxToday;
                    break;
                case VariableEnum.SoilSfcT:
                    value = this.SoilSfcT;
                    break;
                case VariableEnum.SoilT_5cm:
                    value = this.SoilT5cm;
                    break;
                case VariableEnum.SoilT_10cm:
                    value = this.SoilT10cm;
                    break;
                case VariableEnum.SoilT_20cm:
                    value = this.SoilT20cm;
                    break;
                case VariableEnum.SoilT_50cm:
                    value = this.SoilT50cm;
                    break;
                case VariableEnum.SoilT_100cm:
                    value = this.SoilT100cm;
                    break;
                case VariableEnum.Temp_C:
                    value = this.TempC;
                    break;
                case VariableEnum.AirT_1pt5m:
                    value = this.AirT1pt5m;
                    break;
                case VariableEnum.AirT_2m:
                    value = this.AirT2m;
                    break;
                case VariableEnum.AirT_9pt5m:
                    value = this.AirT9pt5m;
                    break;
                case VariableEnum.AirT_10m:
                    value = this.AirT10m;
                    break;
                case VariableEnum.SoilCond_tc:
                    value = this.SoilCondTc;
                    break;
                case VariableEnum.SoilWaCond_tc:
                    value = this.SoilWaCondTc;
                    break;
                case VariableEnum.wfv:
                    value = this.Wfv;
                    break;
                case VariableEnum.RH_2m:
                    value = this.Rh2m;
                    break;
                case VariableEnum.RH_10m:
                    value = this.Rh10m;
                    break;
                case VariableEnum.Pressure_1:
                    value = this.Pressure1;
                    break;
                case VariableEnum.Pressure_2:
                    value = this.Pressure2;
                    break;
                case VariableEnum.TotalRadn:
                    value = this.TotalRadn;
                    break;
                case VariableEnum.QuantRadn:
                    value = this.QuantRadn;
                    break;
                case VariableEnum.WndDir_2m:
                    value = this.WndDir2m;
                    break;
                case VariableEnum.WndDir_10m:
                    value = this.WndDir10m;
                    break;
                case VariableEnum.WndSpd_2m:
                    value = this.WndSpd2m;
                    break;
                case VariableEnum.WndSpd_10m:
                    value = this.WndSpd10m;
                    break;
                case VariableEnum.WndSpd_Vert:
                    value = this.WndSpdVert;
                    break;
                case VariableEnum.WndSpd_2m_Max:
                    value = this.WndSpd2mMax;
                    break;
                case VariableEnum.WndSpd_10m_Max:
                    value = this.WndSpd10mMax;
                    break;
                case VariableEnum.Batt:
                    value = this.Batt;
                    break;
                case VariableEnum.Door:
                    value = this.Door;
                    break;
                default:
                    break;
            }

            if (!isMetricUnits && value.HasValue)
            {
                switch (variableTypeEnum)
                {
                    case VariableTypeEnum.Minute_Precipitation:
                    case VariableTypeEnum.Total_Precipitation:
                        value = value.Value * Constant.mm2Inches;
                        break;
                    case VariableTypeEnum.Temperature:
                        value = value.Value * Constant.nineFifths + 32;
                        break;
                    case VariableTypeEnum.Pressure:
                        value = value.Value * Constant.mb2InHg;
                        break;
                    case VariableTypeEnum.Total_Radiation:
                        value = value.Value * Constant.wpsqm2lymin;
                        break;
                    case VariableTypeEnum.Speed:
                    case VariableTypeEnum.Vertical_Speed:
                        value = value.Value * Constant.mps2Mph;
                        break;
                    default:
                        break;
                }
            }

            if (variableTypeEnum == VariableTypeEnum.Water_Content && value.HasValue)
            {
                value = Math.Round(value.Value * 100, 2);
            }

            if (useRound && value.HasValue)
            {
                switch (variableEnum)
                {
                    case VariableEnum.Precip_TB3_Tot:
                        value = Math.Round(value.Value, 3);
                        break;
                    case VariableEnum.Precip_TX_Tot:
                        value = Math.Round(value.Value, 3);
                        break;
                    case VariableEnum.Precip_TB3_Today:
                        value = Math.Round(value.Value, 3);
                        break;
                    case VariableEnum.Precip_TX_Today:
                        value = Math.Round(value.Value, 3);
                        break;
                    case VariableEnum.SoilSfcT:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilT_5cm:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilT_10cm:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilT_20cm:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilT_50cm:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilT_100cm:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.Temp_C:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.AirT_1pt5m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.AirT_2m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.AirT_9pt5m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.AirT_10m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilCond_tc:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.SoilWaCond_tc:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.wfv:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.RH_2m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.RH_10m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.Pressure_1:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.Pressure_2:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.TotalRadn:
                        value = Math.Round(value.Value, 5);
                        break;
                    case VariableEnum.QuantRadn:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.WndDir_2m:
                        value = Math.Round(value.Value, 1);
                        break;
                    case VariableEnum.WndDir_10m:
                        value = Math.Round(value.Value, 1);
                        break;
                    case VariableEnum.WndSpd_2m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.WndSpd_10m:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.WndSpd_Vert:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.WndSpd_2m_Max:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.WndSpd_10m_Max:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.Batt:
                        value = Math.Round(value.Value, 2);
                        break;
                    case VariableEnum.Door:
                        break;
                    default:
                        break;
                }
            }

            return value;
        }
    }
}