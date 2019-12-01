// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Usa.chili.Dto;

namespace Usa.chili.Services
{
    public interface IStationService
    {
        Task<StationDto> GetStationInfo(int id);
        Task<List<DropdownDto>> ListAllStations();
        Task<List<DropdownDto>> ListActiveStations();
        Task<List<StationMapDto>> GetStationMapData();
    }
}