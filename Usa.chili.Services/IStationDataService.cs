﻿// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usa.chili.Dto;

namespace Usa.chili.Services
{
    public interface IStationDataService
    {
        Task<List<RealtimeDataDto>> ListRealtimeData();
        Task<StationGraphDto> StationGraphData(int stationId, int variableId, DateTime? date, bool isMetricUnits);
    }
}