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
  PRIMARY KEY (Id),
  UNIQUE(StationKey)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (201, 'agricola', 'Agricola', 30.817, -88.5205, 68.58, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (702, 'andalusia', 'Andalusia', 31.2865, -86.5028, 109.73, '2010-03-24', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1000, 'ashford', 'Ashford', 31.19675, -85.2605, 94.8, '2008-12-13', '2018-04-11', 0);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1001, 'ashford_n', 'Ashford North', 31.19675, -85.2605, 94.8, '2018-04-11', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (501, 'atmore', 'Atmore', 31.0156, -87.4451, 86.87, '2008-09-09', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (401, 'bayminette', 'Bay Minette', 30.8948, -87.797, 81.5, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1301, 'castlebery', 'Castleberry', 31.2958, -87.0313, 76.2, '2009-12-27', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (303, 'disl', 'Dauphin Island', 30.2467, -88.07712, 1.52, '2008-07-01', '2012-07-24', 0);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (502, 'dixie', 'Dixie', 31.162, -86.7033, 89.92, '2010-03-18', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (403, 'elberta', 'Elberta', 30.4113, -87.585, 22.1, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (404, 'fairhope', 'Fairhope', 30.5412, -87.8819, 38.1, '2008-09-06', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (701, 'florala', 'Florala', 31.0011, -86.336, 86.87, '2009-01-18', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (405, 'foley', 'Foley', 30.3699, -87.6456, 17.0, '2009-08-03', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (407, 'gasque', 'Gasque', 30.2387, -87.854, 1.22, '2010-05-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (901, 'geneva', 'Geneva', 31.0608, -85.822, 68.58, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (304, 'grandbay', 'Grand Bay', 30.5065, -88.368, 16.46, '2008-12-05', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1601, 'jay', 'Jay', 30.9484, -87.1706, 79.25, '2010-02-18', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1401, 'kinston', 'Kinston', 31.2212, -86.1687, 80.77, '2010-03-17', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1201, 'leakesville', 'Leakesville', 31.1765, -88.6001, 71.6, '2009-10-04', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (406, 'loxley', 'Loxley', 30.6403, -87.7315, 56, '2009-09-11', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (305, 'mobiledr', 'Mobile (Dog River)', 30.5609, -88.0993, 2.97, '2009-09-22', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (302, 'mobileusa', 'Mobile (USA Campus)', 30.7016, -88.182, 38.1, '2008-08-12', '2011-06-24', 0);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (307, 'mobileusaw', 'Mobile (USA Campus West)', 30.6944, -88.1944, 51.82, '2011-06-23', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (301, 'mtvernon', 'Mount Vernon', 31.0905, -87.9987, 16.04, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (101, 'pascagoula', 'Pascagoula', 30.3597, -88.5212, 2.74, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (503, 'poarch', 'Poarch Creek', 31.0921, -87.5435, 89.61, '2014-09-01', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (402, 'robertsdale', 'Robertsdale', 30.584, -87.7301, 47, '2008-08-12', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (306, 'saraland', 'Saraland', 30.8302, -88.0658, 6.1, '2010-01-27', NULL, 1);

INSERT INTO chili.station (Id, StationKey, DisplayName, Latitude, Longitude, Elevation, BeginDate, EndDate, IsActive) 
VALUES (1501, 'walnuthill', 'Walnut Hill', 30.89696, -87.47604, 76.2, '2010-03-30', NULL, 0);

-- Change table engines for public, extremes_tday, and extremes_yday tables to support foreign keys
ALTER TABLE chili.public
ENGINE = InnoDB;

ALTER TABLE chili.extremes_tday
ENGINE = InnoDB;

ALTER TABLE chili.extremes_yday
ENGINE = InnoDB;

-- Add foreign key to the public, extremes_tday, and extremes_yday tables
ALTER TABLE chili.public
ADD FOREIGN KEY (StationKey) REFERENCES station(StationKey);

ALTER TABLE chili.extremes_tday
ADD FOREIGN KEY (StationKey) REFERENCES station(StationKey);

ALTER TABLE chili.extremes_yday
ADD FOREIGN KEY (StationKey) REFERENCES station(StationKey);