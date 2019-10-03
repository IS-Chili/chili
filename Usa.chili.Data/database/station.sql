USE chili;

DROP TABLE IF EXISTS chili.station;

CREATE TABLE chili.station (
  Id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  StationKey VARCHAR(15) NOT NULL,
  DisplayName VARCHAR(30) NOT NULL,
  Latitude DECIMAL(8, 6) NOT NULL,
  Longitude DECIMAL(9, 6) NOT NULL,
  Elevation DECIMAL(5, 2) NOT NULL,
  BeginDate DATETIME NOT NULL,
  EndDate DATETIME NULL,
  IsActive TINYINT(1) NOT NULL,
  PRIMARY KEY (Id)
);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('agricola', 'Agricola', 30.817, -88.5205, 68.58, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('andalusia', 'Andalusia', 31.2865, -86.5028, 109.73, '2010-03-24', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('ashford', 'Ashford', 31.19675, -85.2605, 94.8, '2008-12-13', '2018-04-11', 0);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('ashford_n', 'Ashford North', 31.19675, -85.2605, 94.8, '2018-04-11', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('atmore', 'Atmore', 31.0156, -87.4451, 86.87, '2008-09-09', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('bayminette', 'Bay Minette', 30.8948, -87.797, 81.5, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('castleberry', 'Castleberry', 31.2958, -87.0313, 76.2, '2009-12-27', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('disl', 'Dauphin Island', 30.2467, -88.07712, 1.52, '2008-07-01', '2012-07-24', 0);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('dixie', 'Dixie', 31.162, -86.7033, 89.92, '2010-03-18', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('elberta', 'Elberta', 30.4113, -87.585, 22.1, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('fairhope', 'Fairhope', 30.5412, -87.8819, 38.1, '2008-09-06', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('florala', 'Florala', 31.0011, -86.336, 86.87, '2009-01-18', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('foley', 'Foley', 30.3699, -87.6456, 17.0, '2009-08-03', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('gasque', 'Gasque', 30.2387, -87.854, 1.22, '2010-05-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('geneva', 'Geneva', 31.0608, -85.822, 68.58, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('grandbay', 'Grand Bay', 30.5065, -88.368, 16.46, '2008-12-05', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('jay', 'Jay', 30.9484, -87.1706, 79.25, '2010-02-18', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('kinston', 'Kinston', 31.2212, -86.1687, 80.77, '2010-03-17', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('leakesville', 'Leakesville', 31.1765, -88.6001, 71.6, '2009-10-04', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('loxley', 'Loxley', 30.6403, -87.7315, 56, '2009-09-11', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('mobiledr', 'Mobile (Dog River)', 30.5609, -88.0993, 2.97, '2009-09-22', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('mobileusa', 'Mobile (USA Campus)', 30.7016, -88.182, 38.1, '2008-08-12', '2011-06-24', 0);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('mobileusaw', 'Mobile (USA Campus West)', 30.6944, -88.1944, 51.82, '2011-06-23', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('mtvernon', 'Mount Vernon', 31.0905, -87.9987, 16.04, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('pascagoula', 'Pascagoula', 30.3597, -88.5212, 2.74, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('poarch', 'Poarch Creek', 31.0921, -87.5435, 89.61, '2014-09-01', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('robertsdale', 'Robertsdale', 30.584, -87.7301, 47, '2008-08-12', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('saraland', 'Saraland', 30.8302, -88.0658, 6.1, '2010-01-27', NULL, 1);

INSERT INTO chili.station (StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES ('walnuthill', 'Walnut Hill', 30.89696, -87.47604, 76.2, '2010-03-30', NULL, 0);
