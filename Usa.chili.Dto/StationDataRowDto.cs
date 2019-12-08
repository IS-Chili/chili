// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;

namespace Usa.chili.Dto
{
    public class StationDataRowDto
    {
        public int? VariableId { get; set; }
        public string VariableDescription { get; set; }
        public string MetricValue { get; set; }
        public string MetricSymbol { get; set; }
        public string EnglishValue { get; set; }
        public string EnglishSymbol { get; set; }
    }
}