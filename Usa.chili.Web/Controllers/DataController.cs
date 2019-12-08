// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Usa.chili.Dto;
using Usa.chili.Services;

namespace Usa.chili.Web.Controllers
{
    [Route("[controller]")]
    public class DataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IStationService _stationService;
        private readonly IStationDataService _stationDataService;
        private readonly IPublicService _publicService;
        private readonly IVariableService _variableService;

        public DataController(
            ILogger<DataController> logger,
            IStationService stationService,
            IStationDataService stationDataService,
            IPublicService publicService,
            IVariableService variableService)
        {
            _logger = logger;
            _stationService = stationService;
            _stationDataService = stationDataService;
            _publicService = publicService;
            _variableService = variableService;
        }

        [HttpGet("Realtime")]
        public IActionResult Realtime()
        {
            return View();
        }

        [HttpGet("RealtimeData")]
        public async Task<List<RealtimeDataDto>> RealtimeData(bool isMetricUnits, bool? isWindChill)
        {
            return await _stationDataService.ListRealtimeData(isMetricUnits, isWindChill);
        }

        [HttpGet("CustomProducts")]
        public IActionResult CustomProducts()
        {
            return View();
        }

        [HttpGet("Metadata")]
        public IActionResult Metadata(int? id) => View();

        [HttpGet("Station")]
        public IActionResult Station(int? id)
        {
            return View();
        }

        [HttpGet("StationData")]
        public async Task<StationDataDto> StationData(int id, DateTime? dateTime)
        {
            return await _stationDataService.GetStationData(id, dateTime);
        }

        [HttpGet("StationInfo")]
        public async Task<StationDto> StationInfo(int id)
        {
            return await _stationService.GetStationInfo(id);
        }

        [HttpGet("StationMap")]
        public async Task<List<StationMapDto>> StationMap()
        {
            return await _stationService.GetStationMapData();
        }

        [HttpGet("Graph")]
        public IActionResult Graph()
        {
            return View();
        }

        [HttpGet("StationGraph")]
        public async Task<StationGraphDto> StationGraph(int stationId, int variableId, DateTime? date, bool isMetricUnits)
        {
            return await _stationDataService.StationGraphData(stationId, variableId, date, isMetricUnits);
        }

        [HttpGet("Meteorological")]
        public IActionResult Meteorological()
        {
            return View();
        }

        [HttpGet("QC_Graphs")]
        public IActionResult QC_Graphs()
        {
            return View();
        }

        [HttpGet("Summaries")]
        public IActionResult Summaries()
        {
            return View();
        }

        [HttpGet("VOC")]
        public IActionResult VOC()
        {
            return View();
        }

        [HttpGet("VariableList")]
        public async Task<List<DropdownDto>> VariableList()
        {
            return await _variableService.ListAllVariables();
        }

        [HttpGet("VariableTypeList")]
        public async Task<List<VariableTypeDto>> VariableTypeList()
        {
            return await _variableService.ListAllVariableTypes();
        }

        [HttpGet("ActiveStationList")]
        public async Task<List<DropdownDto>> ActiveStationList()
        {
            return await _stationService.ListActiveStations();
        }

        [HttpGet("StationList")]
        public async Task<List<DropdownDto>> StationList()
        {
            return await _stationService.ListAllStations();
        }

        [HttpGet("StationObservation")]
        public async Task<StationObservationDto> StationObservationData(int id)
        {
            return await _publicService.GetStationObservationById(id);
        }
    }
}