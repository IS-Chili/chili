#!/usr/bin/php
<?php
// 2010-09-29 dnb original version
// 2011-06-30 dnb Added support for USA Campus West station

$db_host = "chiliweb.usouthal.edu";
// $db_host = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";
$fn = "C:\\Campbellsci\\LoggerNet\\";

// NOTE: Only the stations equipped with VOC monitors are listed
$station_arr = array('disl',
                     'elberta',
                     'fairhope',
                     'foley',
                     'gasque',
                     'grandbay',
                     'mobiledr',
                     'mobileusa',
                     'mobileusaw',
                     'pascagoula',
                     'robertsdale');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 3) {
  die("usage: insert_voc_file.php <stationkey> <filename>\n");
}
//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);
if (!in_array($station_key, $station_arr)) {
  die("Invalid station key ($station_key) parameter passed, exiting...\n");
}
$table_id = "voc";
//
// Verify the requested file exists and open it
//
$fn = $argv[2];
if (file_exists($fn)) {
  $handle = fopen($fn, "r");
  if (!$handle) {
    die("Unable to open $fn , exiting...\n");
  }
} else {
  die("File $fn does not exist, exiting...\n");
}
//
// Set the number of columns for the VOC table
//
$cols = 16;
//
// Construct the name of the table to be updated
//
$tbl = $station_key . "_" . $table_id;
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
// Save the field headers and skip any unwanted data
//
$junk   = fgetcsv($handle, 2000, ","); // DataLogger program info, ignore
$header = fgetcsv($handle, 2000, ","); // Save the field headers
$junk   = fgetcsv($handle, 2000, ","); // Another field header, ignore
$junk   = fgetcsv($handle, 2000, ","); // Lots of null fields, ignore
// 
// Verify that the name of the 14th column (zero based) in the specified file is "Raegaurd_Avg"
// 
if ($header[14] != 'Raeguard_Avg') {
  die("Column names from $fn do not match $tbl\n");
}
// 
// Verify that the number of columns in the file matches the number of columns in the table
// 
$firstrec = ftell($handle);
$rec = fgetcsv($handle, 0, ",");
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
