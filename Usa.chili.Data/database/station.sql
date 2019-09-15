USE chili;

DROP TABLE IF EXISTS chili.station;

CREATE TABLE chili.station (
  ID INT(10) NOT NULL AUTO_INCREMENT,
  StationKey VARCHAR(15) NOT NULL,
  DisplayName VARCHAR(30) NOT NULL,
  IsActive TINYINT(1) NOT NULL,
  PRIMARY KEY (ID)
);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('agricola', 'Agricola', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('andalusia', 'Andalusia', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('ashford_n', 'Ashford North', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('atmore', 'Atmore', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('bayminette', 'Bay Minette', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('castleberry', 'Castleberry', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('disl', 'Dauphin Island', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('dixie', 'Dixie', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('elberta', 'Elberta', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('fairhope', 'Fairhope', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('florala', 'Florala', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('foley', 'Foley', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('gasque', 'Gasque', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('geneva', 'Geneva', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('grandbay', 'Grand Bay', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('jay', 'Jay', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('kinston', 'Kinston', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('leakesville', 'Leakesville', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('loxley', 'Loxley', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('mobiledr', 'Mobile (Dog River)', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('mobileusa', 'Mobile (USA Campus)', 0);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('mobileusaw', 'Mobile (USA Campus West)', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('mtvernon', 'Mount Vernon', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('pascagoula', 'Pascagoula', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('poarch', 'Poarch Creek', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('robertsdale', 'Robertsdale', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('saraland', 'Saraland', 1);

INSERT INTO chili.station (StationKey, DisplayName, IsActive) 
VALUES ('walnuthill', 'Walnut Hill', 0);
