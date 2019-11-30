// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Collections.Generic;

namespace Usa.chili.Dto
{
    public class StationGraphDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public DateTime? FirstDateTimeEntry { get; set; }
        public DateTime? LastDateTimeEntry { get; set; }
        public List<StationGraphSeriesDto> Series { get; set; }
    }
}