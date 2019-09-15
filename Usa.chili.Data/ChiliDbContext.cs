// ********************************************************************************************************************************************
// Copyright (c) 2019
// Author: USA
// Product: CHILI
// Version: 1.0.0
// ********************************************************************************************************************************************

using Microsoft.EntityFrameworkCore;
using Usa.chili.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Usa.chili.Data
{
    public partial class ChiliDbContext : DbContext
    {
        public ChiliDbContext(DbContextOptions<ChiliDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MonthlyQcgraphs>()
                .HasKey(x => new { x.Month, x.Year });

            builder.Entity<Station>()
                .Property(x => x.IsActive)
                .HasConversion(new BoolToZeroOneConverter<Int16>());
        }

        public virtual DbSet<Agricola202> Agricola202 { get; set; }
        public virtual DbSet<Agricola202Flags> Agricola202Flags { get; set; }
        public virtual DbSet<Agricola224> Agricola224 { get; set; }
        public virtual DbSet<Agricola260> Agricola260 { get; set; }
        public virtual DbSet<AgricolaPublicPerf> AgricolaPublicPerf { get; set; }
        public virtual DbSet<Andalusia202> Andalusia202 { get; set; }
        public virtual DbSet<Andalusia202Flags> Andalusia202Flags { get; set; }
        public virtual DbSet<Andalusia224> Andalusia224 { get; set; }
        public virtual DbSet<Andalusia260> Andalusia260 { get; set; }
        public virtual DbSet<AndalusiaPublicPerf> AndalusiaPublicPerf { get; set; }
        public virtual DbSet<Ashford202> Ashford202 { get; set; }
        public virtual DbSet<Ashford202Flags> Ashford202Flags { get; set; }
        public virtual DbSet<Ashford224> Ashford224 { get; set; }
        public virtual DbSet<Ashford260> Ashford260 { get; set; }
        public virtual DbSet<AshfordPublicPerf> AshfordPublicPerf { get; set; }
        public virtual DbSet<Atmore202> Atmore202 { get; set; }
        public virtual DbSet<Atmore202Flags> Atmore202Flags { get; set; }
        public virtual DbSet<Atmore224> Atmore224 { get; set; }
        public virtual DbSet<Atmore260> Atmore260 { get; set; }
        public virtual DbSet<AtmorePublicPerf> AtmorePublicPerf { get; set; }
        public virtual DbSet<Bayminette202> Bayminette202 { get; set; }
        public virtual DbSet<Bayminette202Flags> Bayminette202Flags { get; set; }
        public virtual DbSet<Bayminette224> Bayminette224 { get; set; }
        public virtual DbSet<Bayminette260> Bayminette260 { get; set; }
        public virtual DbSet<BayminettePublicPerf> BayminettePublicPerf { get; set; }
        public virtual DbSet<Castleberry202> Castleberry202 { get; set; }
        public virtual DbSet<Castleberry202Flags> Castleberry202Flags { get; set; }
        public virtual DbSet<Castleberry224> Castleberry224 { get; set; }
        public virtual DbSet<Castleberry260> Castleberry260 { get; set; }
        public virtual DbSet<CastleberryPublicPerf> CastleberryPublicPerf { get; set; }
        public virtual DbSet<Climatedata> Climatedata { get; set; }
        public virtual DbSet<ContactInfo> ContactInfo { get; set; }
        public virtual DbSet<Disl202> Disl202 { get; set; }
        public virtual DbSet<Disl202Flags> Disl202Flags { get; set; }
        public virtual DbSet<Disl224> Disl224 { get; set; }
        public virtual DbSet<Disl260> Disl260 { get; set; }
        public virtual DbSet<DislPublicPerf> DislPublicPerf { get; set; }
        public virtual DbSet<DislVoc> DislVoc { get; set; }
        public virtual DbSet<Dixie202> Dixie202 { get; set; }
        public virtual DbSet<Dixie202Flags> Dixie202Flags { get; set; }
        public virtual DbSet<Dixie224> Dixie224 { get; set; }
        public virtual DbSet<Dixie260> Dixie260 { get; set; }
        public virtual DbSet<DixiePublicPerf> DixiePublicPerf { get; set; }
        public virtual DbSet<Elberta202> Elberta202 { get; set; }
        public virtual DbSet<Elberta202Flags> Elberta202Flags { get; set; }
        public virtual DbSet<Elberta224> Elberta224 { get; set; }
        public virtual DbSet<Elberta260> Elberta260 { get; set; }
        public virtual DbSet<ElbertaPublicPerf> ElbertaPublicPerf { get; set; }
        public virtual DbSet<ElbertaVoc> ElbertaVoc { get; set; }
        public virtual DbSet<Extremes> Extremes { get; set; }
        public virtual DbSet<ExtremesTday> ExtremesTday { get; set; }
        public virtual DbSet<ExtremesYday> ExtremesYday { get; set; }
        public virtual DbSet<Fairhope202> Fairhope202 { get; set; }
        public virtual DbSet<Fairhope202Flags> Fairhope202Flags { get; set; }
        public virtual DbSet<Fairhope224> Fairhope224 { get; set; }
        public virtual DbSet<Fairhope260> Fairhope260 { get; set; }
        public virtual DbSet<FairhopePublicPerf> FairhopePublicPerf { get; set; }
        public virtual DbSet<FairhopeVoc> FairhopeVoc { get; set; }
        public virtual DbSet<Filestat> Filestat { get; set; }
        public virtual DbSet<Florala202> Florala202 { get; set; }
        public virtual DbSet<Florala202Flags> Florala202Flags { get; set; }
        public virtual DbSet<Florala224> Florala224 { get; set; }
        public virtual DbSet<Florala260> Florala260 { get; set; }
        public virtual DbSet<FloralaPublicPerf> FloralaPublicPerf { get; set; }
        public virtual DbSet<Foley202> Foley202 { get; set; }
        public virtual DbSet<Foley202Flags> Foley202Flags { get; set; }
        public virtual DbSet<Foley224> Foley224 { get; set; }
        public virtual DbSet<Foley260> Foley260 { get; set; }
        public virtual DbSet<FoleyPublicPerf> FoleyPublicPerf { get; set; }
        public virtual DbSet<FoleyVoc> FoleyVoc { get; set; }
        public virtual DbSet<Gasque202> Gasque202 { get; set; }
        public virtual DbSet<Gasque202Flags> Gasque202Flags { get; set; }
        public virtual DbSet<Gasque224> Gasque224 { get; set; }
        public virtual DbSet<Gasque260> Gasque260 { get; set; }
        public virtual DbSet<GasquePublicPerf> GasquePublicPerf { get; set; }
        public virtual DbSet<GasqueVoc> GasqueVoc { get; set; }
        public virtual DbSet<Geneva202> Geneva202 { get; set; }
        public virtual DbSet<Geneva202Flags> Geneva202Flags { get; set; }
        public virtual DbSet<Geneva224> Geneva224 { get; set; }
        public virtual DbSet<Geneva260> Geneva260 { get; set; }
        public virtual DbSet<GenevaPublicPerf> GenevaPublicPerf { get; set; }
        public virtual DbSet<Grandbay202> Grandbay202 { get; set; }
        public virtual DbSet<Grandbay202Flags> Grandbay202Flags { get; set; }
        public virtual DbSet<Grandbay224> Grandbay224 { get; set; }
        public virtual DbSet<Grandbay260> Grandbay260 { get; set; }
        public virtual DbSet<GrandbayPublicPerf> GrandbayPublicPerf { get; set; }
        public virtual DbSet<GrandbayVoc> GrandbayVoc { get; set; }
        public virtual DbSet<Jay202> Jay202 { get; set; }
        public virtual DbSet<Jay202Flags> Jay202Flags { get; set; }
        public virtual DbSet<Jay224> Jay224 { get; set; }
        public virtual DbSet<Jay260> Jay260 { get; set; }
        public virtual DbSet<JayPublicPerf> JayPublicPerf { get; set; }
        public virtual DbSet<Kinston202> Kinston202 { get; set; }
        public virtual DbSet<Kinston202Flags> Kinston202Flags { get; set; }
        public virtual DbSet<Kinston224> Kinston224 { get; set; }
        public virtual DbSet<Kinston260> Kinston260 { get; set; }
        public virtual DbSet<KinstonPublicPerf> KinstonPublicPerf { get; set; }
        public virtual DbSet<Leakesville202> Leakesville202 { get; set; }
        public virtual DbSet<Leakesville202Flags> Leakesville202Flags { get; set; }
        public virtual DbSet<Leakesville224> Leakesville224 { get; set; }
        public virtual DbSet<Leakesville260> Leakesville260 { get; set; }
        public virtual DbSet<LeakesvillePublicPerf> LeakesvillePublicPerf { get; set; }
        public virtual DbSet<Loxley202> Loxley202 { get; set; }
        public virtual DbSet<Loxley202Flags> Loxley202Flags { get; set; }
        public virtual DbSet<Loxley224> Loxley224 { get; set; }
        public virtual DbSet<Loxley260> Loxley260 { get; set; }
        public virtual DbSet<LoxleyPublicPerf> LoxleyPublicPerf { get; set; }
        public virtual DbSet<MadisStatus> MadisStatus { get; set; }
        public virtual DbSet<Mobiledr202> Mobiledr202 { get; set; }
        public virtual DbSet<Mobiledr202Flags> Mobiledr202Flags { get; set; }
        public virtual DbSet<Mobiledr224> Mobiledr224 { get; set; }
        public virtual DbSet<Mobiledr260> Mobiledr260 { get; set; }
        public virtual DbSet<MobiledrPublicPerf> MobiledrPublicPerf { get; set; }
        public virtual DbSet<MobiledrVoc> MobiledrVoc { get; set; }
        public virtual DbSet<Mobileusa202> Mobileusa202 { get; set; }
        public virtual DbSet<Mobileusa202Flags> Mobileusa202Flags { get; set; }
        public virtual DbSet<Mobileusa224> Mobileusa224 { get; set; }
        public virtual DbSet<Mobileusa260> Mobileusa260 { get; set; }
        public virtual DbSet<MobileusaPublicPerf> MobileusaPublicPerf { get; set; }
        public virtual DbSet<MobileusaVoc> MobileusaVoc { get; set; }
        public virtual DbSet<Mobileusaw202> Mobileusaw202 { get; set; }
        public virtual DbSet<Mobileusaw202Flags> Mobileusaw202Flags { get; set; }
        public virtual DbSet<Mobileusaw224> Mobileusaw224 { get; set; }
        public virtual DbSet<Mobileusaw260> Mobileusaw260 { get; set; }
        public virtual DbSet<MobileusawPublicPerf> MobileusawPublicPerf { get; set; }
        public virtual DbSet<MobileusawQcRain> MobileusawQcRain { get; set; }
        public virtual DbSet<MobileusawVoc> MobileusawVoc { get; set; }
        public virtual DbSet<MonthlyQcgraphs> MonthlyQcgraphs { get; set; }
        public virtual DbSet<MonthlySummary> MonthlySummary { get; set; }
        public virtual DbSet<Mtvernon202> Mtvernon202 { get; set; }
        public virtual DbSet<Mtvernon202Flags> Mtvernon202Flags { get; set; }
        public virtual DbSet<Mtvernon224> Mtvernon224 { get; set; }
        public virtual DbSet<Mtvernon260> Mtvernon260 { get; set; }
        public virtual DbSet<MtvernonPublicPerf> MtvernonPublicPerf { get; set; }
        public virtual DbSet<NonAmbientSourceData> NonAmbientSourceData { get; set; }
        public virtual DbSet<ObstructionData> ObstructionData { get; set; }
        public virtual DbSet<Pascagoula202> Pascagoula202 { get; set; }
        public virtual DbSet<Pascagoula202Flags> Pascagoula202Flags { get; set; }
        public virtual DbSet<Pascagoula224> Pascagoula224 { get; set; }
        public virtual DbSet<Pascagoula260> Pascagoula260 { get; set; }
        public virtual DbSet<PascagoulaPublicPerf> PascagoulaPublicPerf { get; set; }
        public virtual DbSet<PascagoulaVoc> PascagoulaVoc { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<Poarch202> Poarch202 { get; set; }
        public virtual DbSet<Poarch202Flags> Poarch202Flags { get; set; }
        public virtual DbSet<PoarchPublicPerf> PoarchPublicPerf { get; set; }
        public virtual DbSet<Public> Public { get; set; }
        public virtual DbSet<Rainfall> Rainfall { get; set; }
        public virtual DbSet<Robertsdale202> Robertsdale202 { get; set; }
        public virtual DbSet<Robertsdale202Flags> Robertsdale202Flags { get; set; }
        public virtual DbSet<Robertsdale224> Robertsdale224 { get; set; }
        public virtual DbSet<Robertsdale260> Robertsdale260 { get; set; }
        public virtual DbSet<RobertsdalePublicPerf> RobertsdalePublicPerf { get; set; }
        public virtual DbSet<RobertsdaleVoc> RobertsdaleVoc { get; set; }
        public virtual DbSet<RoughnessData> RoughnessData { get; set; }
        public virtual DbSet<Saraland202> Saraland202 { get; set; }
        public virtual DbSet<Saraland202Flags> Saraland202Flags { get; set; }
        public virtual DbSet<Saraland224> Saraland224 { get; set; }
        public virtual DbSet<Saraland260> Saraland260 { get; set; }
        public virtual DbSet<SaralandPublicPerf> SaralandPublicPerf { get; set; }
        public virtual DbSet<SensorData> SensorData { get; set; }
        public virtual DbSet<SiteData> SiteData { get; set; }
        public virtual DbSet<SiteVisit> SiteVisit { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<Stormtrack> Stormtrack { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Walnuthill202> Walnuthill202 { get; set; }
        public virtual DbSet<Walnuthill202Flags> Walnuthill202Flags { get; set; }
        public virtual DbSet<Walnuthill224> Walnuthill224 { get; set; }
        public virtual DbSet<Walnuthill260> Walnuthill260 { get; set; }
        public virtual DbSet<WalnuthillPublicPerf> WalnuthillPublicPerf { get; set; }
        public virtual DbSet<Windgust> Windgust { get; set; }
        public virtual DbSet<Windtest> Windtest { get; set; }
    }
}
