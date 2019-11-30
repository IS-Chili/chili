using Usa.chili.Common;
using System;

namespace Usa.chili.Domain
{
    public partial class StationData
    {
        public double? ValueForVariable(bool isMetricUnits, VariableTypeEnum variableTypeEnum, VariableEnum variableEnum)
        {
            double? value = null;

            switch(variableEnum) {
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

            if(!isMetricUnits && value.HasValue) {
                double ninefifths = 9/5;
                double mm2inches = 0.0393700787;
                double mb2inHg =  0.029529983071;
                double wpsqm2lymin = 0.00143197;
                double mps2Mph = 2.2369363;

                switch(variableTypeEnum) {
                    case VariableTypeEnum.Minute_Precipitation:
                    case VariableTypeEnum.Total_Precipitation:
                        value = value.Value * mm2inches;
                        break;
                    case VariableTypeEnum.Temperature:
                        value = value.Value * ninefifths + 32;
                        break;
                    case VariableTypeEnum.Pressure:
                        value = value.Value * mb2inHg;
                        break;
                    case VariableTypeEnum.Total_Radiation:
                        value = value.Value * wpsqm2lymin;
                        break;
                    case VariableTypeEnum.Speed:
                    case VariableTypeEnum.Vertical_Speed:
                        value = value.Value * mps2Mph;
                        break;
                    default:
                    break;
                }
            }

            if(variableTypeEnum == VariableTypeEnum.Water_Content && value.HasValue) {
                value = Math.Round(value.Value * 100, 2);
            }

            return value;
        }
    }
}