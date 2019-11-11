#!/usr/bin/php
<?php
// 2010-02-02 dnb Original version
// 2018-05-08 wdt Added support for North Ashford station

//   This script is designed to be invoked by LoggerNet TaskMaster after
//   the insert_loggernet script has completed updating the station's
//   260 (hourly) table. This script then compares the 260 table's extremes
//   (maxes and mins) with the values in the extremes table and updates the
//   extremes table as appropriate.

$db_host = "chiliweb.usouthal.edu";
// $db_host = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";

$station_name = array('agricola',
                      'ashford',
                      'atmore',
                      'bayminette',
                      'castleberry',
                      'disl',
                      'elberta',
                      'fairhope',
                      'florala',
                      'foley',
                      'gasque',
                      'geneva',
                      'grandbay',
                      'leakesville',
                      'loxley',
                      'mobiledr',
                      'mobileusa',
                      'mtvernon',
					  'ashford_n',
                      'pascagoula',
					  'poarch',
                      'robertsdale');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 2) {
  die("usage: update_extremes <station_key>\n");
}

//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);
if (!in_array($station_key, $station_name)) {
  die("Invalid station key parameter ($station_key) passed, exiting...\n");
}

//
// Construct the name of the hourly table to be read
//
$hourly_tbl = $station_key . "_260";

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
// Select the most recent row from the station's hourly table
//
$query = sprintf('select * from %s order by TS desc limit 1', $hourly_tbl);
$result = mysql_query($query);
if (!$result) {
    die('Could not query the database: ' . mysql_error() . "\n");
}
if (mysql_num_rows($result) == 0) {
  die("No data available in $hourly_tbl\n");
}
$row = mysql_fetch_assoc($result);
mysql_free_result($result);

//
// Select the station's row from the extremes table
//
$query = sprintf("select * from extremes where StationKey =\"%s\"",$station_key);
$result = mysql_query($query);
if (!$result) {
    die("Could not query the database: " . mysql_error() . "\n");
}
if (mysql_num_rows($result) == 0) {
  die("No data available in extremes for $station_key\n");
}
$extremes = mysql_fetch_assoc($result);
mysql_free_result($result);

//
// Compare the station's hourly extremes to those in the extremes table
//
$initqry = 'update extremes set ';
$query = $initqry;

foreach (array_keys($extremes) as $key) {
  if ($key == 'StationKey') continue;

  $tpos = strpos($key,'_M');
  if ($tpos === FALSE) {
    $tpos = strpos($key,'_T');
    if ($tpos === FALSE) {
      die("Unexpected key name ($key) encountered, exiting...\n");
    }
  }
  $obs = substr($key, 0, $tpos);
  $typ = substr($key, $tpos);

  if ((substr($typ, 0, 4) != '_Max') && (substr($typ, 0, 4) != '_Min')) continue;
  if ($row[$key] === NULL) continue;
  if (substr($typ, 0, 4) == '_Max') {
    $sfx = str_replace('Max','TMx',$typ);
  } else {
    $sfx = str_replace('Min','TMn',$typ);
  }
  $date_hourly   = strtotime(substr($row[$obs . $sfx], 0 , 10));
  $date_extremes = strtotime(substr($extremes[$obs . $sfx], 0 , 10));
  if (($extremes[$key] === NULL) ||
      ($date_hourly > $date_extremes) ||
      ((substr($typ, 0, 4) == '_Max') && ($row[$key] > $extremes[$key])) ||
      ((substr($typ, 0, 4) == '_Min') && ($row[$key] < $extremes[$key]))) {
    $query .= $key . '="' . $row[$key] . '",';
    $query .= $obs . $sfx . '="' . $row[$obs . $sfx] . '",';
  }
}
if (strlen($query) > strlen($initqry)) {
  $query = substr($query, 0,strlen($query)-1) . ' where StationKey="' . $station_key . '"';
  if (!mysql_query($query)) {
    die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
  }
}
mysql_close($connection);
?>
