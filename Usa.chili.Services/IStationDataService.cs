// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usa.chili.Domain;
using Usa.chili.Dto;

namespace Usa.chili.Services
{
    /// <summary>
    /// Interface for StationDataService.
    /// </summary>
    public interface IStationDataService
    {
        Task<List<RealtimeDataDto>> ListRealtimeData(bool isMetricUnits, bool? isWindChill);
        Task<StationGraphDto> StationGraphData(int stationId, int variableId, DateTime? date, bool isMetricUnits);
        Task<List<StationData>> MeteorologicalDownloadData(int stationId, DateTime beginDate, DateTime endDate);
        Task<StationDataDto> GetStationData(int stationId, DateTime? dateTime);
    }
}