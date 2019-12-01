#!/usr/bin/php
<?php
// Revision history:
// 2011-09-29 dnb Original version
// 2018-05-08 wdt Added support for North Ashford station
// 2019-11-29 Utilize PDO and the new station and station data tables
//
// Notes:
// This script implements improvements on an eariler script (insert_loggernet_file.php)
// 1) Inserts only database columns that have a LoggerNet table file counterpart

$db_host = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";
$charset = 'latin1';

$dsn = "mysql:host=$db_host;dbname=$db_database;charset=$charset";

$options = [
  PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
  PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
  PDO::ATTR_EMULATE_PREPARES   => false
];

//
// Verify that the correct number of arguments was specified
//
if ($argc != 4) {
  die("usage: insert_csi_file <stationkey> <table_id> <filename>\n");
}

//
// Connect to database
//
try {
  $pdo = new PDO($dsn, $db_username, $db_password, $options);
} catch (\PDOException $e) {
  throw new \PDOException($e->getMessage(), (int)$e->getCode());
}

//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);

$stmt = $pdo->prepare('SELECT station.id FROM chili.station WHERE station.StationKey = :stationKey LIMIT 1');
$stmt->execute(['stationKey' => $station_key]);
$stationId = $stmt->fetchColumn();

if ($stationId == null) {
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
$stmt = $pdo->prepare("SELECT * FROM $tbl ORDER BY TS DESC LIMIT 1");
$stmt->execute();
$row = $stmt->fetch();

if (!$row) {
  die("No data available in $tbl\n");
}

$dbtcol = array();

foreach($row as $key=>$value)
{
  array_push($dbtcol, $key);
}

while (($rec = fgetcsv($handle, 0, ",")) != FALSE) {
  $ts = $rec[0];
  $query = "INSERT IGNORE $tbl SET ";
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

  $stmt = $pdo->prepare($query);
  $stmt->execute();
  if ($stmt->rowCount() == 0) {
    die("Unable to insert $ts row from $fn into $tbl , \$query : $query\n");
  }
}

$pdo = null;
fclose($handle);

echo "Data inserted into $tbl\n";
?>
