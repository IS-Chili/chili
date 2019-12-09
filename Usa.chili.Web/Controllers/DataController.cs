// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Usa.chili.Common;
using Usa.chili.Domain;
using Usa.chili.Dto;
using Usa.chili.Services;
using Usa.chili.Web.Converters;

namespace Usa.chili.Web.Controllers
{
    /// <summary>
    /// This controller handles and displays the data views.
    /// This controller also returns data as JSON responses.
    /// </summary>
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

        /// <summary>
        /// This view displays the realtime display page.
        /// </summary>
        /// <returns>Data/Realtime view</returns>
        [HttpGet("Realtime")]
        public IActionResult Realtime()
        {
            return View();
        }

        /// <summary>
        /// Gets data for the realtime data table.
        /// </summary>
        /// <param name="isMetricUnits">Returns data in metric units if true, english units if false</param>
        /// <param name="isWindChill">Returns windchill data if true, heatindex data if false</param>
        /// <returns>List of RealtimeDataDtos as JSON</returns>
        [HttpGet("RealtimeData")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<List<RealtimeDataDto>> RealtimeData(bool isMetricUnits, bool? isWindChill)
        {
            return await _stationDataService.ListRealtimeData(isMetricUnits, isWindChill);
        }

        /// <summary>
        /// This view displays the custom products page.
        /// </summary>
        /// <returns>Data/CustomProducts view</returns>
        [HttpGet("CustomProducts")]
        public IActionResult CustomProducts()
        {
            return View();
        }

        /// <summary>
        /// This view displays the metadata page.
        /// </summary>
        /// <returns>Data/Metadata view</returns>
        [HttpGet("Metadata")]
        public IActionResult Metadata(int? id)
        {
            return View();
        }

        /// <summary>
        /// This view displays the station page.
        /// </summary>
        /// <returns>Data/Station view</returns>
        [HttpGet("Station")]
        public IActionResult Station(int? id)
        {
            return View();
        }

        /// <summary>
        /// Gets data for the station data table.
        /// </summary>
        /// <param name="id">Station Id filter</param>
        /// <param name="dateTime">Timestamp filter</param>
        /// <returns>StationDataDto as JSON</returns>
        [HttpGet("StationData")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<StationDataDto> StationData(int id, DateTime? dateTime)
        {
            return await _stationDataService.GetStationData(id, dateTime);
        }

        /// <summary>
        /// Gets data for station info displays.
        /// </summary>
        /// <param name="id">Station Id filter</param>
        /// <returns>StationDto as JSON</returns>
        [HttpGet("StationInfo")]
        public async Task<StationDto> StationInfo(int id)
        {
            return await _stationService.GetStationInfo(id);
        }

        /// <summary>
        /// Gets data for the station map.
        /// </summary>
        /// <returns>List of StationMapDtos as JSON</returns>
        [HttpGet("StationMap")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<List<StationMapDto>> StationMap()
        {
            return await _stationService.GetStationMapData();
        }

        /// <summary>
        /// This view displays the graph page.
        /// </summary>
        /// <returns>Data/Graph view</returns>
        [HttpGet("Graph")]
        public IActionResult Graph()
        {
            return View();
        }

        /// <summary>
        /// Gets data for station info displays.
        /// </summary>
        /// <param name="stationId">Station Id filter</param>
        /// <param name="variableId">Variable Id filter</param>
        /// <param name="date">Timestamp filter</param> 
        /// <param name="isMetricUnits">Returns data in metric units if true, english units if false</param>
        /// <returns>StationGraphDto as JSON</returns>
        [HttpGet("StationGraph")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<StationGraphDto> StationGraph(int stationId, int variableId, DateTime? date, bool isMetricUnits)
        {
            return await _stationDataService.StationGraphData(stationId, variableId, date, isMetricUnits);
        }

        /// <summary>
        /// This view displays the meteorological page.
        /// </summary>
        /// <returns>Data/Meteorological view</returns>
        [HttpGet("Meteorological")]
        public IActionResult Meteorological()
        {
            return View();
        }

        /// <summary>
        /// Processes the download request and returns the file to the client.
        /// </summary>
        /// <returns>CSV or Fixed file</returns>
        [HttpGet("MeteorologicalDownload")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> MeteorologicalDownload(int id, DateTime beginDate, DateTime endDate, DownloadFormatEnum downloadFormat, string variables)
        {
            // Validation:
            // 1. Must have 1 variable selected
            // 2. endDate must come after beginDate
            // 3. Timespan between beginDate and endDate must be 31 or less days
            if(variables == null || beginDate > endDate || (endDate - beginDate).Days > 31) {
                return View("Error", new ErrorDto { StatusCode = 400 });
            }

            // Get station Info
            var stationInfo = await _stationService.GetStationInfo(id);

            // Get data for the file
            var data = await _stationDataService.MeteorologicalDownloadData(id, beginDate, endDate);

            // File extension
            var extension = "csv";
            if(downloadFormat == DownloadFormatEnum.Fixed) {
                extension = "dat";
            }

            // Add Timestamp column for CSV
            if(downloadFormat == DownloadFormatEnum.CSV) {
                variables = "Ts," + variables;
            }

            // Create file name
            var fileName = stationInfo.DisplayName.Replace(" ", "_") 
                + "_" + beginDate.ToString(Constant.DATE_FILE_FORMAT) 
                + "_" + endDate.ToString(Constant.DATE_FILE_FORMAT) 
                + "." + extension;
            
            // Create the memory stream for the file
            var memoryStream = new MemoryStream(WriteCsvToMemory(data, variables.Split(","), downloadFormat));

            // Return the file stream
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = fileName };
        }

        /// <summary>
        /// Processes and writes the CSV or Fixed format.
        /// </summary>
        /// <returns>CSV or Fixed file</returns>
        private byte[] WriteCsvToMemory(IEnumerable<StationData> records, string[] columns, DownloadFormatEnum downloadFormat)
        {
            // Use the streams and writers to generate the file
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                // Remove delimeter for fixed format
                // Remove header row for fixed format
                if(downloadFormat == DownloadFormatEnum.Fixed) {
                    csvWriter.Configuration.Delimiter = "";
                    csvWriter.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.None;
                    csvWriter.Configuration.ShouldQuote = (field, context) => false;
                    csvWriter.Configuration.HasHeaderRecord = false;
                }

                // Configure the columns to write to the file
                var map = new CsvHelper.Configuration.DefaultClassMap<StationData>();
                foreach (var column in columns)
                {
                    var property = typeof(StationData).GetProperty(column.Replace("_", ""));

                    // Use the FixedWidthConverter for the fixed format
                    if(downloadFormat == DownloadFormatEnum.Fixed) {
                        map.Map(typeof(StationData), property).TypeConverter<FixedWidthConverter>();
                    }
                    else {
                        map.Map(typeof(StationData), property);
                    }
                }
                csvWriter.Configuration.RegisterClassMap(map);

                // Write the records and flush
                csvWriter.WriteRecords(records);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// This view displays the QC graphs page.
        /// </summary>
        /// <returns>Data/QC_Graphs view</returns>
        [HttpGet("QC_Graphs")]
        public IActionResult QC_Graphs()
        {
            return View();
        }

        /// <summary>
        /// This view displays the summaries page.
        /// </summary>
        /// <returns>Data/Summaries view</returns>
        [HttpGet("Summaries")]
        public IActionResult Summaries()
        {
            return View();
        }

        /// <summary>
        /// This view displays the VOC page.
        /// </summary>
        /// <returns>Data/VOC view</returns>
        [HttpGet("VOC")]
        public IActionResult VOC()
        {
            return View();
        }

        /// <summary>
        /// Gets data for variable dropdowns.
        /// </summary>
        /// <returns>List of DropdownDtos as JSON</returns>
        [HttpGet("VariableList")]
        public async Task<List<DropdownDto>> VariableList()
        {
            return await _variableService.ListAllVariables();
        }

        /// <summary>
        /// Gets data for the realtime data table's unit symbols.
        /// </summary>
        /// <returns>List of VariableTypeDtos as JSON</returns>
        [HttpGet("VariableTypeList")]
        public async Task<List<VariableTypeDto>> VariableTypeList()
        {
            return await _variableService.ListAllVariableTypes();
        }

        /// <summary>
        /// Gets active stations for station dropdowns.
        /// </summary>
        /// <returns>List of DropdownDtos as JSON</returns>
        [HttpGet("ActiveStationList")]
        public async Task<List<DropdownDto>> ActiveStationList()
        {
            return await _stationService.ListActiveStations();
        }

        /// <summary>
        /// Gets all stations for station dropdowns.
        /// </summary>
        /// <returns>List of DropdownDtos as JSON</returns>
        [HttpGet("StationList")]
        public async Task<List<DropdownDto>> StationList()
        {
            return await _stationService.ListAllStations();
        }

        /// <summary>
        /// Gets data for the Station Observation widget and realtime data row in English units.
        /// </summary>
        /// <param name="id">Station Id filter</param>
        /// <returns>StationObservationDto as JSON</returns>
        [HttpGet("StationObservation")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<StationObservationDto> StationObservationData(int id)
        {
            return await _publicService.GetStationObservation(id);
        }
    }
}