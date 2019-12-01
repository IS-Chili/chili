// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Usa.chili.Common;
using Usa.chili.Data;
using Usa.chili.Domain;
using Usa.chili.Dto;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace Usa.chili.Services
{
    public class VariableService: IVariableService
    {
        private readonly ILogger _logger;
        private readonly ChiliDbContext _dbContext;

        static VariableService()
        {
        }

        public VariableService(ILogger<VariableService> logger, ChiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<DropdownDto>> ListAllVariables() {
            return await _dbContext.VariableDescription
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new DropdownDto {
                    Id = x.Id,
                    Text = x.VariableDescription1
                })
                .ToListAsync();
        }

        public async Task<List<VariableTypeDto>> ListAllVariableTypes() {
            return await _dbContext.VariableType
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new VariableTypeDto {
                    VariableType = x.VariableType1,
                    MetricMin = x.MetricMin,
                    MetricMax = x.MetricMax,
                    MetricUnit = x.MetricUnit,
                    MetricSymbol = x.MetricSymbol,
                    EnglishMin = x.EnglishMin,
                    EnglishMax = x.EnglishMax,
                    EnglishUnit = x.EnglishUnit,
                    EnglishSymbol = x.EnglishSymbol
                })
                .ToListAsync();
        }
    }
}
