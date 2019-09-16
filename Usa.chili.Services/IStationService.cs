// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Usa.chili.Domain;
using Usa.chili.Dto;

namespace Usa.chili.Services
{
    public interface IStationService
    {
        Task<List<DropdownDto>> ListAllStations();
        Task<List<DropdownDto>> ListActiveStations();
        Task<StationObservationDto> GetStationObservationById(int id);
    }
}