#!/usr/bin/php
<?php
// 2010-01-19 dnb original version
//            Modified insert_loggernet.php to read from a specified file
//            and insert only records that do not already exist in table
// 2010-01-26 dnb Removed record length limit on fgetcsv call
//            dnb Modified station_file hash to station_arr array
//            dnb Added verification that table_id parameter matches file tablecode
// 2010-01-29 dnb Added support for Table260
// 2010-03-23 dnb Added support for Andalusia,Gasque,Jay,Kinston,Saraland,Walnut Hill
// 2011-06-30 dnb Added support for USA Campus West
// 2018-05-08 wdt Added support for North Ashford station

$db_host = "chiliweb.usouthal.edu";
// $db_host = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";
$fn = "C:\\Campbellsci\\LoggerNet\\";
$table_code = 2; // record array element containing TableCode

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
  die("usage: insert_loggernet_file <stationkey> <table_id> <filename>\n");
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
    $table_id != "260") {
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
//
// Set the number of columns for the table specified
//
if ($table_id == "202") {
  $cols = 80;
} else {
  $cols = 202;
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
// Skip past the header records
// 
for ($i=0; $i < 4; $i++) {
  fgetcsv($handle, 0, ",");
}
// 
// Verify that the tablecode in the specified file matches the table_id from the command line
// and that the number of columns in the file matches the number of columns in the table
// 
$firstrec = ftell($handle);
$rec = fgetcsv($handle, 0, ",");
if ($rec[$table_code] != $table_id) {
  die("Table_id specified ($table_id) does not match table_code ($rec[$table_code]) from $fn\n");
}
if (count($rec) != $cols) {
  die("Column count in table: $tbl ($cols)\n  does not match file: " . $fn . " (" . count($rec) . ")\n");
}
// Reset file pointer to the first data record
fseek($handle, $firstrec);

while (($rec = fgetcsv($handle, 0, ",")) != FALSE) {
  $query = "insert ignore $tbl values(";
  // Build query string and set empty or "NAN" fields to NULL so MySQL will store a NULL
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

mysql_close($connection);
fclose($handle);
?>
