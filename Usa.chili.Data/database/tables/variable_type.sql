USE chili;

DROP TABLE IF EXISTS chili.variable_type;

CREATE TABLE chili.variable_type (
  Id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  VariableType VARCHAR(30) NOT NULL,
  MetricMin DECIMAL(6, 2) NOT NULL,
  MetricMax DECIMAL(6, 2) NOT NULL,
  MetricUnit VARCHAR(30) NOT NULL,
  MetricShort VARCHAR(30) NOT NULL,
  EnglishMin DECIMAL(6, 2) NOT NULL,
  EnglishMax DECIMAL(6, 2) NOT NULL,
  EnglishUnit VARCHAR(30) NOT NULL,
  EnglishShort VARCHAR(30) NOT NULL,
  PRIMARY KEY (Id)
);

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Minute Precipitation', 0, 10, 'Millimeters', 'mm', 0, 0.5, 'Inches', 'in');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Total Precipitation', 0, 200, 'Millimeters', 0, 8, 'Inches');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Temperature', -30, 50, 'Degrees Celsius', -25, 125, 'Degrees Fahrenheit');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Humidity', 0, 100, 'Percent Humidity', 0, 100, 'Percent Humidity');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Water Content', 0, 100, 'Soil Water Content Percentage', 0, 100, 'Soil Water Content Percentage');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Conductivity', 0, 1, 'Siemens/Meter', 0, 1, 'Siemens/Meter');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Pressure', 990, 1030, 'Hectopascals', 29, 30.5, 'Inches of Mercury');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Total Radiation', -100, 2000, 'Watts/sq Meter', -0.15, 2.9, 'Langleys/Minute');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Quantum Radiation', -100, 2000, 'microE/sq Meter/second', -100, 2000, 'microE/sq Meter/second');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Direction', 0, 360, 'Degrees', 0, 360, 'Degrees');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Speed', 0, 15, 'Meters/second', 0, 40, 'Miles/Hour');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Vertical Speed', -8, 8, 'Meters/second', -20, 20, 'Miles/Hour');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Voltage', 8, 14.5, 'Volts', 8, 14.5, 'Volts');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricShort, EnglishMin, EnglishMax, EnglishUnit, EnglishShort) 
VALUES ('Switch', -1, 2, 'State', -1, 2, 'State');
