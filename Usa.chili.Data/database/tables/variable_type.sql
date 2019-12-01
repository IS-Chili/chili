USE chili;

DROP TABLE IF EXISTS chili.variable_type;

CREATE TABLE chili.variable_type (
  Id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  VariableType VARCHAR(30) NOT NULL,
  MetricMin DECIMAL(6, 2) NOT NULL,
  MetricMax DECIMAL(6, 2) NOT NULL,
  MetricUnit VARCHAR(30) NOT NULL,
  MetricSymbol VARCHAR(30) NOT NULL,
  EnglishMin DECIMAL(6, 2) NOT NULL,
  EnglishMax DECIMAL(6, 2) NOT NULL,
  EnglishUnit VARCHAR(30) NOT NULL,
  EnglishSymbol VARCHAR(30) NOT NULL,
  PRIMARY KEY (Id)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Minute_Precipitation', 0, 10, 'Millimeters', 'mm', 0, 0.5, 'Inches', 'in');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Total_Precipitation', 0, 200, 'Millimeters', 'mm', 0, 8, 'Inches', 'in');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Temperature', -30, 50, 'Degrees Celsius', '&#176;C', -25, 125, 'Degrees Fahrenheit', '&#176;F');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Humidity', 0, 100, 'Percent Humidity', '&#37;', 0, 100, 'Percent Humidity', '&#37;');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Water_Content', 0, 100, 'Soil Water Content Percentage', '&#37;', 0, 100, 'Soil Water Content Percentage', '&#37;');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Conductivity', 0, 1, 'Siemens/Meter', 'siemens/m', 0, 1, 'Siemens/Meter', 'siemens/m');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Pressure', 990, 1030, 'Hectopascals', 'hPa', 29, 30.5, 'Inches of Mercury', 'inHg');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Total_Radiation', -100, 2000, 'Watts/sq Meter', 'W/m&#178;', -0.15, 2.9, 'Langleys/Minute', 'ly/min');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Quantum_Radiation', -100, 2000, 'microE/sq Meter/second', '&#181;E/m&#178;/s', -100, 2000, 'microE/sq Meter/second', '&#181;E/m&#178;/s');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Direction', 0, 360, 'Degrees', '&#176;', 0, 360, 'Degrees', '&#176;');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Speed', 0, 15, 'Meters/second', 'm/s', 0, 40, 'Miles/Hour', 'mph');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Vertical_Speed', -8, 8, 'Meters/second', 'm/s', -20, 20, 'Miles/Hour', 'mph');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Voltage', 8, 14.5, 'Volts', 'v', 8, 14.5, 'Volts', 'v');

INSERT INTO chili.variable_type (VariableType, MetricMin, MetricMax, MetricUnit, MetricSymbol, EnglishMin, EnglishMax, EnglishUnit, EnglishSymbol) 
VALUES ('Switch', -1, 2, 'State', 'state', -1, 2, 'State', 'state');
