#!/usr/bin/php
<?php
// Revision history:
// 2011-09-29 dnb Original version
// 2018-05-08 wdt Added support for North Ashford station
//
// Notes:
// This script implements improvements on an eariler script (insert_loggernet_file.php)
// 1) Inserts only database columns that have a LoggerNet table file counterpart

$db_host = "chiliweb.usouthal.edu";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";

$station_arr = array('agricola',
                     'andalusia',
                     'ashford',
                     'atmore',
                     'bayminette',
                     'castleberry',
                     'disl',
                     'dixie',
                     'elberta',
                     'fairhope',
                     'florala',
                     'foley',
                     'gasque',
                     'geneva',
                     'grandbay',
                     'jay',
                     'kinston',
                     'leakesville',
                     'loxley',
                     'mobiledr',
                     'mobileusa',
                     'mobileusaw',
                     'mtvernon',
					 'ashford_n',
                     'pascagoula',
					 'poarch',
                     'robertsdale',
                     'saraland',
                     'walnuthill');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 4) {
  die("usage: insert_csi_file <stationkey> <table_id> <filename>\n");
}
//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);
if (!in_array($station_key, $station_arr)) {
  die("Invalid station key ($station_key) parameter passed, exiting...\n");
}
//
// Verify that the table_id parameter is valid
//
$table_id = $argv[2];
if ($table_id != "202" &&
    $table_id != "207" &&
    $table_id != "224" &&
    $table_id != "260" &&
    $table_id != "voc") {
  die("Invalid table_id ($table_id) parameter passed, exiting...\n");
}
//
// Verify the requested file exists and open it
//
$fn = $argv[3];
if (file_exists($fn)) {
  $handle = fopen($fn, "r");
  if (!$handle) {
    die("Unable to open $fn , exiting...\n");
  }
} else {
  die("File $fn does not exist, exiting...\n");
}

// Construct the name of the table to be updated
$tbl = $station_key . "_" . $table_id;

// Use new table for 202
if($table_id == '202') {
  $tbl = 'station_data';
}
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
// All CHILI LoggerNet .dat files are currently in TOA5 format,
// which has the following structure:
// 
// Record 1: DataLogger program info, etc.
// Record 2: Field headers for each column of data
// Record 3: Additional, but incomplete, field header 
// Record 4: Record of null fields
// Record 5: Begin data records
// 
// Retrieve the column names for this LoggerNet table from the file
// 
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
$j = array_search('Door_open_1', $csicol);
if ($j) {
  $csicol[$j] = 'Door';
}

//
// Retrieve the column names for this database table
//
$query = sprintf("select * from %s order by TS desc limit 1",$tbl);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error() . "\n");
}
if (mysql_num_rows($result) == 0) {
  die("No data available in $tbl\n");
}
$row = mysql_fetch_assoc($result);
$dbtcnt = count($row);
$dbtcol = array();
for ($c=0; $c < $dbtcnt; $c++) {
  $dbtcol[$c] = mysql_field_name($result, $c);
}
mysql_free_result($result);

while (($rec = fgetcsv($handle, 0, ",")) != FALSE) {
  $query = "insert ignore $tbl set ";
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
  //echo $query . "\n";
  $result = mysql_query($query);
  if (!$result) {
    die("Unable to insert $ts row from $fn into $tbl , \$query : $query\n");
  }
}

mysql_close($connection);
fclose($handle);
?>
