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
    public interface IPublicService
    {
        Task<StationObservationDto> GetStationObservationById(int id);
    }
}