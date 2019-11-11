#!/usr/bin/php
<?php
// 2009-12-17 dnb Original version
//   This script is designed to be invoked by LoggerNet TaskMaster after
//   completing communications with a station. The script will extract
//   the most recent data from the station's Public (realtime) table and
//   update the MySQL database "public" table on the web server.
// 2010-01-13 dnb Added support for Castleberry station
// 2010-02-08 dnb Added support for collecting polling performance statistics
// 2010-03-23 dnb Added support for Andalusia,Gasque,Jay,Kinston,Saraland,Walnut Hill
// 2010-09-21 dnb Removed Lat,Lon, and Elev updates until Datalogger program is fixed
// 2010-09-27 dnb Added support for VOC (Raeguard) monitor
// 2011-06-30 dnb Added support for USA Campus West station
// 2011-09-30 dnb Added support for soil sensor data
// 2011-09-30 dnb Implemented modifications to ease future CSI field changes
// 2013-12-18 dnb Added support for Poarch Creek station
// 2018-05-08 wdt Added support for North Ashford station


// Use script start time to estimate Public table data arrival time
$arrive_ts = date("\"Y-m-d H:i:s\"");

$db_host     = "chiliweb.usouthal.edu";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";

//                      'mobileusa'   => 'USA Campus_Public.dat',
$station_file = array('agricola'    => 'Agricola_Public.dat',
                      'andalusia'   => 'Andalusia_Public.dat',
                      'ashford'     => 'Ashford_Public.dat',
                      'atmore'      => 'Atmore_Public.dat',
                      'bayminette'  => 'Bay Minette_Public.dat',
                      'castleberry' => 'Castleberry_Public.dat',
                      'disl'        => 'Dauphin Island_Public.dat',
                      'dixie'       => 'Dixie_Public.dat',
                      'elberta'     => 'Elberta_Public.dat',
                      'fairhope'    => 'Fairhope_Public.dat',
                      'florala'     => 'Florala_Public.dat',
                      'foley'       => 'Foley_Public.dat',
                      'gasque'      => 'Gasque_Public.dat',
                      'geneva'      => 'Geneva_Public.dat',
                      'grandbay'    => 'Grand Bay_Public.dat',
                      'jay'         => 'Jay_Public.dat',
                      'kinston'     => 'Kinston_Public.dat',
                      'leakesville' => 'Leakesville_Public.dat',
                      'loxley'      => 'Loxley_Public.dat',
                      'mobiledr'    => 'Dog River_Public.dat',
                      'mobileusaw'  => 'USA Campus West_Public.dat',
                      'mtvernon'    => 'Mt Vernon_Public.dat',
                      'pascagoula'  => 'Pascagoula_Public.dat',
                      'robertsdale' => 'Robertsdale_Public.dat',
                      'saraland'    => 'Saraland_Public.dat',
                      'walnuthill'  => 'Walnut Hill_Public.dat',
                      'poarch'      => 'Poarch_Public.dat',
					  'ashford_n'   => 'Ashford North_Public.dat');

//
// Verify that the station key parameter exists and is valid
//
if ($argc != 2) {
  die("Station key parameter omitted, exiting...\n");
}
$station_key = strtolower($argv[1]);
if (!array_key_exists($station_key, $station_file)) {
   die("Invalid station key parameter passed, exiting...\n");
}
//
// Open the requested file and extract field names and data
//
if (file_exists($station_file[$station_key])) {
  $handle = fopen($station_file[$station_key], "r");
  if (!$handle) {
     die("Unable to open $station_file[$station_key] , exiting...\n");
  }
} else {
  die("File $station_file[$station_key] does not exist, exiting...\n");
}

// 
// All CHILI LoggerNet .dat files are currently in TOA5 format,
// which has the following structure:
// 
// Record 1: DataLogger program info, etc.
// Record 2: Field headers for each column of data
// Record 3: Additional, but incomplete, field header 
// Record 4: Record of null fields
// Record 5: Begin data records
// 
// Save the field headers and skip any unwanted data
          fgetcsv($handle, 0, ","); // DataLogger program info, ignore
$csicol = fgetcsv($handle, 0, ","); // Save the field headers
          fgetcsv($handle, 0, ","); // Another field header, ignore
          fgetcsv($handle, 0, ","); // Lots of null fields, ignore
// Modify LoggerNet column names to match database table column names
$csicnt = count($csicol);
$csicol[0] = 'TS';
$csicol[1] = 'RecId';
for ($c=2; $c < $csicnt; $c++) {
  $csicol[$c] = preg_replace('/\((\d+)\)$/', '_${1}', $csicol[$c]);
}

// Begin processing the data records
// Read the entire file in case there are multiple records, keep the last one
while (($data = fgetcsv($handle, 0, ",")) != FALSE) {
  $rec = $data;
}
fclose($handle);

//
// Connect to database and select for use
//
$connection = mysql_connect($db_host,$db_username,$db_password);
if (!$connection) {
  die("Could not connect to the database: " . mysql_error());
}
$db_select = mysql_select_db($db_database);
if (!$db_select) {
  die("Could not select the database: " . mysql_error());
}
//
// If no row exists for this station, create one now
//
$query = sprintf("select * from public where StationKey='%s'",$station_key);
$result = mysql_query($query);
if (!$result) {
    die("Could not query the database: " . mysql_error());
}
if (mysql_num_rows($result) == 0) {
  $query = sprintf("insert into public set StationKey='%s'",$station_key);
  $result = mysql_query($query);
  if (!$result) {
      die("Unable to insert null row for new station: " . mysql_error());
  }
}
//
// Retrieve the column names for this database table
//
$query = sprintf("select * from public where StationKey='%s'",$station_key);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error() . "\n");
}
$row = mysql_fetch_assoc($result);
$dbtcnt = count($row);
$dbtcol = array();
for ($c=0; $c < $dbtcnt; $c++) {
  $dbtcol[$c] = mysql_field_name($result, $c);
}
mysql_free_result($result);

$query = "update public set ";
// Set any empty or "NAN" fields to NULL so MySQL will store a NULL
// and build query string
for ($c=0; $c < $csicnt; $c++) {
  if (!in_array($csicol[$c], $dbtcol)) continue;
  $query .= $csicol[$c] . "=";
  if (strlen($rec[$c]) == 0 || $rec[$c] == "NAN") {
    $rec[$c] = "NULL";
  } else {
    $rec[$c] = '"' . $rec[$c] . '"';
  }
  $query .= $rec[$c] . ",";
}
$query = substr($query,0,-1);
$query .= sprintf(" where StationKey='%s'",$station_key);
//echo $query . "\n";
$result = mysql_query($query);
if (!$result) {
  die("Unable to update " . $station_key . "station: " . mysql_error());
}

//
// Update the polling performance statistics for this station
//
$query = sprintf("insert ignore %s_public_perf values(%s,%s)",
           $station_key, $rec[0], $arrive_ts);
$result = mysql_query($query);
if (!$result) {
  die("Unable to update " . $station_key . "_public_perf: " . mysql_error());
}

mysql_close($connection);
?>
