// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System.Collections.Generic;

namespace Usa.chili.Dto
{
    /// <summary>
    /// DTO for station graph series.
    /// </summary>
    public class StationGraphSeriesDto
    {
        public string Name { get; set; }
        public double LineWidth { get; set; }
        public List<List<double>> Data { get; set; }
    }
}