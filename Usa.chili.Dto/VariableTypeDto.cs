// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

namespace Usa.chili.Dto
{
    /// <summary>
    /// DTO for variable type.
    /// </summary>
    public class VariableTypeDto
    {
        public string VariableType { get; set; }
        public decimal MetricMin { get; set; }
        public decimal MetricMax { get; set; }
        public string MetricUnit { get; set; }
        public string MetricSymbol { get; set; }
        public decimal EnglishMin { get; set; }
        public decimal EnglishMax { get; set; }
        public string EnglishUnit { get; set; }
        public string EnglishSymbol { get; set; }
    }
}