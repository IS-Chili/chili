#!/usr/bin/php
<?php
// 2011-03-11 dnb original version
// 2011-06-30 dnb Added support for USA Campus West
// 2013-12-18 dnb Added support for Poarch Creek

$usage = "Usage: " . basename($argv[0]) . " [YYYY MM]\n";
// $db_host = "chiliweb.usouthal.edu";
$db_host = "localhost";
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
                     'pascagoula',
                     'robertsdale',
                     'saraland',
                     'walnuthill',
                     'poarch');
$date_tpl = '%04u-%02u-01 00:00:00';
$ts_tpl   = '%04u-%02u-01 00:00:00';
$qry_cnt   = 'select count(*) from %s_202 where TS >= "%s" and TS < "%s"';
$qry_sum   = 'select sum(Precip_TB3_Tot) as Precip_TB3, sum(Precip_TX_Tot) as Precip_TX from %s_202 where TS >= "%s" and TS < "%s"';
$qry_ins   = 'insert ignore rainfall values("%s","%s","%s","%01.2f","%01.2f","%01.2f")';

//
// Determine whether to update previous month or
// the specified year and month
//
if ($argc == 1) {
  // No argument case, update previous month
  $end_str   = sprintf($ts_tpl,date('Y'),date('m'));
  $end_tm    = strtotime($end_str);
  $start_tm  = strtotime("-1 month", $end_tm);
  $start_str = date('Y-m-d H:i:s', $start_tm);
  $year      = date('Y',$start_tm);
  $month     = date('m',$start_tm);
} elseif ($argc == 3) {
  // Year and month supplied, update accordingly
  if (!is_numeric($argv[1])) {
    echo "$argv[1] is not a valid year\n";
    die($usage);
  }
  if ($argv[1] > 9999 || $argv[1] < 1000) {
    echo "$argv[1] is not a valid year\n";
    die($usage);
  }
  if (!is_numeric($argv[2])) {
    echo "$argv[2] is not a valid month\n";
    die($usage);
  }
  if ($argv[2] < 1 || $argv[2] > 12) {
    echo "$argv[2] is not a valid month\n";
    die($usage);
  }
  $year  = sprintf('%04s', intval($argv[1]));
  $month = sprintf('%02s', intval($argv[2]));
  $start_str = sprintf($ts_tpl,$year,$month);
  $start_tm  = strtotime($start_str);
  $end_tm    = strtotime("+1 month", $start_tm);
  $end_str   = date('Y-m-d H:i:s', $end_tm);
} else {
  // Invalid parameter count
  echo "Invalid parameter count: $argc\n";
  die($usage);
}

$days = date('t',$start_tm);
$total = $days * 1440;

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
// Process the requested month for all stations
//
foreach ($station_arr as $station) {
  //echo "Processing $year-$month for station $station\n";
  $query = sprintf($qry_cnt,$station,$start_str,$end_str);
  //echo " using: $query\n";
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error() . "\n");
  }
  $row = mysql_fetch_row($result);
  $collected = $row[0];
  if ($collected > 0) {
    //echo "There are $collected rainfall data records for $station during $year-$month\n";
    $query = sprintf($qry_sum,$station,$start_str,$end_str);
    $result = mysql_query($query);
    if (!$result) {
      die("Could not query the database: " . mysql_error() . "\n");
    }
    $row = mysql_fetch_assoc($result);
    $pctcoll = ($collected/$total)*100;
    $query = sprintf($qry_ins,$year,$month,$station,$row['Precip_TB3'],$row['Precip_TX'],$pctcoll);
    $result = mysql_query($query);
    if (!$result) {
      die("Unable to insert $year,$month,$station row into rainfall table, \$query : $query\n");
    }
  }
}
mysql_close($connection);
?>
