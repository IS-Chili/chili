USE chili;

DROP TABLE IF EXISTS chili.variable_description;

CREATE TABLE chili.variable_description (
  Id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  VariableTypeId INT(10) UNSIGNED NOT NULL,
  VariableName VARCHAR(20) NOT NULL,
  VariableDescription VARCHAR(50) NOT NULL,
  EnglishSymbol VARCHAR(20),
  MetricSymbol VARCHAR(20),
  PRIMARY KEY (Id),
  FOREIGN KEY (VariableTypeId) REFERENCES variable_type (Id)
);

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Precip_TB3_Tot', 'Precipitation per minute (TB3)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Minute Precipitation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Precip_TX_Tot', 'Precipitation per minute (TX)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Minute Precipitation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Precip_TB3_Today', 'Precipitation since midnight (TB3)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Total Precipitation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Precip_TX_Today', 'Precipitation since midnight (TX)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Total Precipitation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilSfcT', 'Soil Surface Temperature'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilT_5cm', 'Soil Temperature at 5cm (1.97in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilT_10cm', 'Soil Temperature at 10cm (3.94in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilT_20cm', 'Soil Temperature at 20cm (7.87in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilT_50cm', 'Soil Temperature at 50cm (19.68in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilT_100cm', 'Soil Temperature at 100cm (39.37in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Temp_C', 'Hydraprobe Soil Temperature at 100cm (39.37in)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'AirT_1pt5m', 'Air Temperature at 1.5m (4.92ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'AirT_2m', 'Air Temperature at 2m (6.56ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'AirT_9pt5m', 'Air Temperature at 9.5m (31.17ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'AirT_10m', 'Air Temperature at 10m (31.81ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Temperature';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilCond_tc', 'Temperature corrected Soil Conductivity'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Conductivity';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'SoilWaCond_tc', 'Temperature corrected Soil Water Conductivity'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Conductivity';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'wfv', 'Soil Water content percentage'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Water Content';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'RH_2m', 'Relative Humidity at 2m (6.56ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Humidity';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'RH_10m', 'Relative Humidity at 10m (31.81ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Humidity';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Pressure_1', 'Air Pressure #1'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Pressure';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Pressure_2', 'Air Pressure #2'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Pressure';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'TotalRadn', 'Total Radiation'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Total Radiation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'QuantRadn', 'Quantum Radiation'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Quantum Radiation';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndDir_2m', 'Wind Direction at 2m (6.56ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Direction';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndDir_10m', 'Wind Direction at 10m (31.81ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Direction';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndSpd_2m', 'Wind Speed at 2m (6.56ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Speed';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndSpd_10m', 'Wind Speed at 10m (31.81ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Speed';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndSpd_Vert', 'Vertical Wind Speed'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Vertical Speed';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndSpd_2m_Max', 'Maximum Wind Speed at 2m (6.56ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Speed';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'WndSpd_10m_Max', 'Maximum Wind Speed at 10m (31.81ft)'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Speed';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Batt', 'Battery Voltage'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Voltage';

INSERT INTO chili.variable_description (VariableTypeId, VariableName, VariableDescription, EnglishSymbol, MetricSymbol) 
SELECT variable_type.id, 'Door', 'Enclosure Door'
FROM chili.variable_type
WHERE variable_type.VariableType = 'Switch';
