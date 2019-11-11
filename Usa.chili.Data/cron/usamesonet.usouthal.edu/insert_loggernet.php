#!/usr/bin/php
<?php
//   This script is designed to be invoked by LoggerNet TaskMaster after
//   completing communications with a station. The script will extract
//   the most recent data from the station and table specified on the
//   command line and update the appropriate MySQL table for the web server.
//
// Revision history:
// 2010-01-15 dnb Original version
// 2010-01-19 dnb Modified to fseek to last processed record in table file
// 2010-01-26 dnb Removed record length limit on fgetcsv call
// 2010-01-29 dnb Added support for Table260
// 2010-03-18 dnb Added support for Dixie
// 2010-03-23 dnb Added support for Andalusia,Gasque,Jay,Kinston,Saraland,Walnut Hill
// 2010-09-30 dnb Added support for VOC tables at VOC monitor equipped sites
// 2011-06-30 dnb Added support for USA Campus West station
// 2018-05-08 wdt Added support for Ashford North station

$db_host = "chiliweb.usouthal.edu";
// $db_host = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";
$fn = "C:\\Campbellsci\\LoggerNet\\";

$station_file = array('agricola'    => 'Agricola_TableNNN.dat',
                      'andalusia'   => 'Andalusia_TableNNN.dat',
                      'ashford'     => 'Ashford_TableNNN.dat',
                      'atmore'      => 'Atmore_TableNNN.dat',
                      'bayminette'  => 'Bay Minette_TableNNN.dat',
                      'castleberry' => 'Castleberry_TableNNN.dat',
                      'disl'        => 'Dauphin Island_TableNNN.dat',
                      'dixie'       => 'Dixie_TableNNN.dat',
                      'elberta'     => 'Elberta_TableNNN.dat',
                      'fairhope'    => 'Fairhope_TableNNN.dat',
                      'florala'     => 'Florala_TableNNN.dat',
                      'foley'       => 'Foley_TableNNN.dat',
                      'gasque'      => 'Gasque_TableNNN.dat',
                      'geneva'      => 'Geneva_TableNNN.dat',
                      'grandbay'    => 'Grand Bay_TableNNN.dat',
                      'jay'         => 'Jay_TableNNN.dat',
                      'kinston'     => 'Kinston_TableNNN.dat',
                      'leakesville' => 'Leakesville_TableNNN.dat',
                      'loxley'      => 'Loxley_TableNNN.dat',
                      'mobiledr'    => 'Dog River_TableNNN.dat',
                      'mobileusa'   => 'USA Campus_TableNNN.dat',
                      'mobileusaw'  => 'USA Campus West_TableNNN.dat',
                      'mtvernon'    => 'Mt Vernon_TableNNN.dat',
					  'ashford_n'   => 'Ashford North_TableNNN.dat',
                      'pascagoula'  => 'Pascagoula_TableNNN.dat',
					  'poarch'      => 'Poarch_TableNNN.dat',
                      'robertsdale' => 'Robertsdale_TableNNN.dat',
                      'saraland'    => 'Saraland_TableNNN.dat',
                      'walnuthill'  => 'Walnut Hill_TableNNN.dat');

$voc_station = array('disl',
                     'elberta',
                     'fairhope',
                     'foley',
                     'gasque',
                     'grandbay',
                     'mobiledr',
                     'mobileusaw',
                     'pascagoula',
                     'robertsdale');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 3) {
  die("Station key and/or Table ID parameter missing, exiting...\n");
}
//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);
if (!array_key_exists($station_key, $station_file)) {
  die("Invalid station key ( $station_key ) parameter passed, exiting...\n");
}
//
// Verify that the table id parameter is valid and set column count
//
$table_id = strtolower($argv[2]);
switch ($table_id) {
  case '202':
    $cols = 80;
    break;
  case '207':
  case '224':
  case '260':
    $cols = 202;
    break;
  case 'voc':
    if (!in_array($station_key, $voc_station)) {
      die("The station at $station_key is not VOC monitor equipped, exiting...\n");
    }
    $cols = 16;
    break;
  default:
    die("Invalid table id ( $table_id ) parameter passed, exiting...\n");
}

//
// Construct the name of the table to be updated
//
$tbl = $station_key . "_" . $table_id;

// Use new table for 202
if($table_id == '202') {
  $tbl = 'station_data';
}
//
// Construct the name of the data file to be processed
//
$bn = str_replace("NNN",strtoupper($table_id),$station_file[$station_key]);
$fn .= $bn;
//
// Connect to database and select for use
//
$connection = mysql_connect($db_host,$db_username,$db_password);
if (!$connection) {
  die("Could not connect to the database: " . mysql_error() . "\n");
}
$db_select = mysql_select_db($db_database);
if (!$db_select) {
  die("Could not select the database: " . mysql_error() . "\n");
}
//
// Select the most recent timestamp from the selected station table
//
$query = sprintf("select TS from %s order by TS desc limit 1",$tbl);
$result = mysql_query($query);
if (!$result) {
    die("Could not query the database: " . mysql_error() . "\n");
}
if (mysql_num_rows($result) == 0) {
  die("No data available in $tbl\n");
}
$row = mysql_fetch_row($result);
$mru_ts = $row[0];
mysql_free_result($result);
//
// Verify the requested file exists, get its size, and open it
//
if (file_exists($fn)) {
  $cfs = sprintf("%u",filesize($fn));        // save current file size
  $cft = date("Y-m-d H:i:s",filemtime($fn)); // save current file time
  $handle = fopen($fn, "r");
  if (!$handle) {
    die("Unable to open $fn , exiting...\n");
  }
} else {
  die("File $fn does not exist, exiting...\n");
}

//
// Determine the offset to continue processing this file at
//
$query = sprintf('select FileSize from filestat where FileName="%s"', $bn);
$result = mysql_query($query);
if (!$result) {
    die("Could not query the database: " . mysql_error() . "\n");
}
if (mysql_num_rows($result) == 0) {
  // Insert initial entry for new file with a file size of 0
  $query = "insert filestat values(\"$bn\",\"0\",\"$cft\")";
  $result = mysql_query($query);
  if (!$result) {
    die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
  }
  $pfs = 0;
} else {
  $row = mysql_fetch_row($result);
  $pfs = $row[0]; // Previous file size
  if ($cfs < $pfs) {
    $pfs = 0;
  }
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
// If starting at the beginning of the file, skip the headers
// otherwise position to the last record processed
// 
if ($pfs == 0) {
  for ($i=0; $i < 4; $i++) {
    fgetcsv($handle, 0, ",");
  }
} else {
  fseek($handle,$pfs);
}

// Begin database update after reading a table file timestamp greater
// than the most recent database timestamp
while (($rec = fgetcsv($handle, 0, ",")) != FALSE) {
  $ts = $rec[0];
  if ($ts <= $mru_ts) continue;
  $query = "insert $tbl values(";
  // Set any empty or "NAN" fields to NULL so MySQL will store a NULL
  // and build query string
  for ($c=0; $c < $cols; $c++) {
    if (strlen($rec[$c]) == 0 || $rec[$c] == "NAN") {
      $rec[$c] = "NULL";
    } else {
      $rec[$c] = '"' . $rec[$c] . '"';
    }
    $query .= $rec[$c] . ",";
  }
  $query = substr($query,0,-1);
  $query .= ")";
  $result = mysql_query($query);
  if (!$result) {
    die("Unable to insert $ts row from $fn into $tbl , \$query : $query\n");
  }
}
// 
// Update the filestat table to reflect the current size of the file
// 
$query = "update filestat set FileSize=\"$cfs\",StatTime=\"$cft\"";
$query .= " where FileName=\"$bn\"";
$result = mysql_query($query);
if (!$result) {
  die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
}
mysql_close($connection);
fclose($handle);
?>
