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
    /// <summary>
    /// DTO for station data.
    /// </summary>
    public class StationDataDto
    {
        public DateTime? FirstDateTimeEntry { get; set; }
        public DateTime? LastDateTimeEntry { get; set; }
        public List<StationDataRowDto> StationDataRows { get; set; }
    }
}