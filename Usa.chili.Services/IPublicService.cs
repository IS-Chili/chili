// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System.Threading.Tasks;
using Usa.chili.Dto;

namespace Usa.chili.Services
{
    /// <summary>
    /// Interface for PublicService.
    /// </summary>
    public interface IPublicService
    {
        Task<StationObservationDto> GetStationObservation(int stationId);
    }
}