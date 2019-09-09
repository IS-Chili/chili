// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Common;
using Usa.chili.Data;
using Usa.chili.Dto;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Usa.chili.Services
{
    public class StationService: IStationService
    {
        private readonly ILogger _logger;
        private readonly ChiliDbContext _dbContext;

        static StationService()
        {
        }

        public StationService(ILogger<StationService> logger, ChiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // TODO: Replace with a database call
        public async Task<List<Station>> ListStations() {
            return await Task.FromResult(new List<Station> {
                new Station {
                    ID = 1,
                    DisplayName = "Agricola"
                },
                new Station {
                    ID = 2,
                    DisplayName = "Andalusia"
                },
                new Station {
                    ID = 3,
                    DisplayName = "Atmore"
                },
                new Station {
                    ID = 4,
                    DisplayName = "Bay Minette"
                },
                new Station {
                    ID = 5,
                    DisplayName = "Castleberry"
                },
                new Station {
                    ID = 6,
                    DisplayName = "Dixie"
                },
                new Station {
                    ID = 7,
                    DisplayName = "Elberta"
                },
                new Station {
                    ID = 8,
                    DisplayName = "Fairhope"
                },
                new Station {
                    ID = 9,
                    DisplayName = "Florala"
                },
                new Station {
                    ID = 10,
                    DisplayName = "Foley"
                },
                new Station {
                    ID = 11,
                    DisplayName = "Gasque"
                },
                new Station {
                    ID = 12,
                    DisplayName = "Geneva"
                },
                new Station {
                    ID = 13,
                    DisplayName = "Grand Bay"
                },
                new Station {
                    ID = 14,
                    DisplayName = "Jay"
                },
                new Station {
                    ID = 15,
                    DisplayName = "Kinston"
                },
                new Station {
                    ID = 16,
                    DisplayName = "Leakesville"
                },
                new Station {
                    ID = 17,
                    DisplayName = "Loxley"
                },
                new Station {
                    ID = 18,
                    DisplayName = "Mobile (Dog River)"
                },
                new Station {
                    ID = 19,
                    DisplayName = "USA Campus West"
                },
                new Station {
                    ID = 20,
                    DisplayName = "Mount Vernon"
                },
                new Station {
                    ID = 21,
                    DisplayName = "Pascagoula"
                },
                new Station {
                    ID = 22,
                    DisplayName = "Poarch Creek"
                },
                new Station {
                    ID = 23,
                    DisplayName = "Robertsdale"
                },
                new Station {
                    ID = 24,
                    DisplayName = "Saraland"
                }
            });
        }

        // TODO: Replace with a database call
        public async Task<StationObservationDto> GetStationObservationById(int id) {
            List<Station> stations = await this.ListStations();

            return await Task.FromResult(new StationObservationDto {
                StationName = stations.Where(x => x.ID == id).First().DisplayName,
                StationTimestamp = DateTime.Now,
                AirTemperature = 81.98,
                DewPoint = 75.71,
                RealHumidity = 81.35,
                WindDirection = 264.91,
                WindSpeed = 0.73,
                Pressure = 29.96,
                Precipitation = 1.0
            });
        }
    }
}
