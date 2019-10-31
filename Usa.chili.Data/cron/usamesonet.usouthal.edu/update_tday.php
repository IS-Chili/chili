#!/usr/bin/php
<?php
// NOTE: update_tday.php is invoked via LoggerNet TaskMaster,
// It is NOT a web page script
// update_tday.php updates the extremes_tday table with the high
// and low values of selected observations for the current day
//
// 2010-10-20 dnb Modified dew point calculation to use more accurate formula
// 2010-12-07 dnb Removed dew point limits of below 0 and above 50 Celsius
// 2010-12-07 dnb Provided separate dew point constants for above or below 0 Celsius
// 2011-02-01 dnb Modified Dew point calculation to show N/A if RH > 100 not RH >= 100
// 2011-06-30 dnb Added support for USA Campus West station
// 2013-12-18 dnb Added support for Poarch Creek station
// 2018-05-08 wdt Added support for North Ashford station
//
// $db_host='localhost';
$db_host='chiliweb.usouthal.edu';
$db_database='chili';
$db_username='chiliupd';
$db_password='NONE';

$te = array();
$station_id = array('agricola',
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
                    'poarch',
					'ashford_n');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 2) {
  die("usage: update_tday <station_key>\n");
}

//
// Verify that the station key parameter is valid
//
$station = strtolower($argv[1]);
if (!in_array($station, $station_id)) {
  die("Invalid station key parameter ($station) passed, exiting...\n");
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

// Query strings for Today's Extremes returns these variables
// $te['TS']
$te_time = 'select ' .
           'TS ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by TS desc limit 1';
// $te['AirT_2m_TMx']
// $te['AirT_2m_Max']
$te_atmx = 'select ' .
           'TS as AirT_2m_TMx,' .
           'AirT_2m as AirT_2m_Max ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by AirT_2m_Max desc, TS asc limit 1';
// $te['AirT_2m_TMn']
// $te['AirT_2m_Min']
$te_atmn = 'select ' .
           'TS as AirT_2m_TMn,' .
           'AirT_2m as AirT_2m_Min ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by AirT_2m_Min asc, TS asc limit 1';
// Get values used to calculate dew point
// $te['DewPt_2m_TMx']
// $te['DewPt_2m_Max']           
// $te['DewPt_2m_TMn']
// $te['DewPt_2m_Min']           
$te_dwpt = 'select ' .
           'TS,' .
           'rh(RH_2m) as RH,' .
           'AirT_2m as AirT ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by TS asc';
// $te['RH_2m_TMx']
// $te['RH_2m_Max']           
$te_rhmx = 'select ' .
           'TS as RH_2m_TMx,' .
           'rh(RH_2m) as RH_2m_Max ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by RH_2m_Max desc, TS asc limit 1';
// $te['RH_2m_TMn']
// $te['RH_2m_Min']           
$te_rhmn = 'select ' .
           'TS as RH_2m_TMn,' .
           'rh(RH_2m) as RH_2m_Min ' .
           'from %s_202 ' .
           'where TS >= curdate() and rh(RH_2m) is not NULL ' .
           'order by RH_2m_Min asc, TS asc limit 1';
// $te['WndSpd_10m_TMx']
// $te['WndSpd_10m_Max']
$te_wsmx = 'select ' .
           'TS as WndSpd_10m_TMx,' .
           'WndSpd_10m_Max ' .
           'from %s_202 ' .
           'where TS >= curdate() ' .
           'order by WndSpd_10m_Max desc, TS asc limit 1';
           
// Collect Today's extremes data for the selected station
// Today's recent minute table time stamp
$query = sprintf($te_time,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}

if (mysql_num_rows($result) == 0) {
  die("No data for " . $station . " station on " . date('Y-m-d') . " exiting now\n");
} else {
  $row = mysql_fetch_assoc($result);
  $te['TS'] = $row['TS'];
}

// Max Air Temp
$query = sprintf($te_atmx,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$row = mysql_fetch_assoc($result);
$te['AirT_2m_TMx'] = $row['AirT_2m_TMx'];
$te['AirT_2m_Max'] = $row['AirT_2m_Max'];

// Min Air Temp
$query = sprintf($te_atmn,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$row = mysql_fetch_assoc($result);
$te['AirT_2m_TMn'] = $row['AirT_2m_TMn'];
$te['AirT_2m_Min'] = $row['AirT_2m_Min'];

// Query today's RH and Temp data for Dew point calculation
$query = sprintf($te_dwpt,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$dewpt = array();
// Calculate all valid dew points in today's observations
while ($row = mysql_fetch_assoc($result)) {
  if (is_null($row['AirT']) ||
      is_null($row['RH']) ||
      $row['AirT'] >= 60 ||
      // $row['AirT'] < 0 ||
      $row['RH'] > 100 ||
      $row['RH'] <= 1) {
    continue;
  } else {
    // Define constants for Dew Point calculation (Deg C)
    if ($row['AirT'] < 0 ) {
      $a = 22.452;
      $b = 272.55;
    } else {
      $a = 17.502;
      $b = 240.97;
    }
    $gamma = (($a * $row['AirT'])/($b + $row['AirT'])) + log($row['RH'] / 100);
    $dewpt[$row['TS']] = ($b * $gamma)/($a - $gamma);
  }
}
// Store Max and Min Dew point values and times
if (count($dewpt) > 0) {
  $te['DewPt_2m_Max'] = max($dewpt);
  $te['DewPt_2m_TMx'] = array_search($te['DewPt_2m_Max'],$dewpt);
  $te['DewPt_2m_Min'] = min($dewpt);
  $te['DewPt_2m_TMn'] = array_search($te['DewPt_2m_Min'],$dewpt);
} else {
  $te['DewPt_2m_Max'] = NULL;
  $te['DewPt_2m_TMx'] = NULL;
  $te['DewPt_2m_Min'] = NULL;
  $te['DewPt_2m_TMn'] = NULL;
}

// Max Relative Humidity
$query = sprintf($te_rhmx,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$row = mysql_fetch_assoc($result);
$te['RH_2m_TMx'] = $row['RH_2m_TMx'];
$te['RH_2m_Max'] = $row['RH_2m_Max'];

// Min Relative Humidity
$query = sprintf($te_rhmn,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$row = mysql_fetch_assoc($result);
$te['RH_2m_TMn'] = $row['RH_2m_TMn'];
$te['RH_2m_Min'] = $row['RH_2m_Min'];

// Max Wind Speed
$query = sprintf($te_wsmx,$station);
$result = mysql_query($query);
if (!$result) {
  die("Could not query the database: " . mysql_error());
}
$row = mysql_fetch_assoc($result);
$te['WndSpd_10m_TMx'] = $row['WndSpd_10m_TMx'];
$te['WndSpd_10m_Max'] = $row['WndSpd_10m_Max'];

//
// If no row exists for this station then insert data, otherwise update
//
$query = sprintf('select * from extremes_tday where StationKey="%s"',$station);
$result = mysql_query($query);
if (!$result) {
    die("Could not query the database: " . mysql_error());
}
if (mysql_num_rows($result) == 0) {
  $op = 'insert';
} else {
  $op = 'update';
}

// Build template for query string
$qrytpl = $op . 
          ' extremes_tday set ' .
          'TS=%s,' .
          'AirT_2m_TMx=%s,' .
          'AirT_2m_Max=%s,' .
          'AirT_2m_TMn=%s,' .
          'AirT_2m_Min=%s,' .
          'DewPt_2m_TMx=%s,' .
          'DewPt_2m_Max=%s,' .
          'DewPt_2m_TMn=%s,' .
          'DewPt_2m_Min=%s,' .
          'RH_2m_TMx=%s,' .
          'RH_2m_Max=%s,' .
          'RH_2m_TMn=%s,' .
          'RH_2m_Min=%s,' .
          'WndSpd_10m_TMx=%s,' .
          'WndSpd_10m_Max=%s';
if ($op == 'update') {
  $qrytpl .= ' where StationKey="%s"';
} else {
  $qrytpl .= ',StationKey="%s"';
}

// Prepare all values for use in MySQL query
$te['TS'] = '"' . $te['TS'] . '"';
if (is_null($te['AirT_2m_TMx'])) {
  $te['AirT_2m_TMx'] = 'NULL';
} else {
  $te['AirT_2m_TMx'] = '"' . $te['AirT_2m_TMx'] . '"';
}
if (is_null($te['AirT_2m_Max'])) {
  $te['AirT_2m_Max'] = 'NULL';
}
if (is_null($te['AirT_2m_TMn'])) {
  $te['AirT_2m_TMn'] = 'NULL';
} else {
  $te['AirT_2m_TMn'] = '"' . $te['AirT_2m_TMn'] . '"';
}
if (is_null($te['AirT_2m_Min'])) {
  $te['AirT_2m_Min'] = 'NULL';
}
if (is_null($te['DewPt_2m_TMx'])) {
  $te['DewPt_2m_TMx'] = 'NULL';
} else {
  $te['DewPt_2m_TMx'] = '"' . $te['DewPt_2m_TMx'] . '"';
}
if (is_null($te['DewPt_2m_Max'])) {
  $te['DewPt_2m_Max'] = 'NULL';
}
if (is_null($te['DewPt_2m_TMn'])) {
  $te['DewPt_2m_TMn'] = 'NULL';
} else {
  $te['DewPt_2m_TMn'] = '"' . $te['DewPt_2m_TMn'] . '"';
}
if (is_null($te['DewPt_2m_Min'])) {
  $te['DewPt_2m_Min'] = 'NULL';
}
if (is_null($te['RH_2m_TMx'])) {
  $te['RH_2m_TMx'] = 'NULL';
} else {
  $te['RH_2m_TMx'] = '"' . $te['RH_2m_TMx'] . '"';
}
if (is_null($te['RH_2m_Max'])) {
  $te['RH_2m_Max'] = 'NULL';
}
if (is_null($te['RH_2m_TMn'])) {
  $te['RH_2m_TMn'] = 'NULL';
} else {
  $te['RH_2m_TMn'] = '"' . $te['RH_2m_TMn'] . '"';
}
if (is_null($te['RH_2m_Min'])) {
  $te['RH_2m_Min'] = 'NULL';
}
if (is_null($te['WndSpd_10m_TMx'])) {
  $te['WndSpd_10m_TMx'] = 'NULL';
} else {
  $te['WndSpd_10m_TMx'] = '"' . $te['WndSpd_10m_TMx'] . '"';
}
if (is_null($te['WndSpd_10m_Max'])) {
  $te['WndSpd_10m_Max'] = 'NULL';
}

// Insert prepared values into MySQL query string
$query = sprintf($qrytpl,
                 $te['TS'],
                 $te['AirT_2m_TMx'],
                 $te['AirT_2m_Max'],
                 $te['AirT_2m_TMn'],
                 $te['AirT_2m_Min'],
                 $te['DewPt_2m_TMx'],
                 $te['DewPt_2m_Max'],
                 $te['DewPt_2m_TMn'],
                 $te['DewPt_2m_Min'],
                 $te['RH_2m_TMx'],
                 $te['RH_2m_Max'],
                 $te['RH_2m_TMn'],
                 $te['RH_2m_Min'],
                 $te['WndSpd_10m_TMx'],
                 $te['WndSpd_10m_Max'],
                 $station);

// Update extremes_tday table with this stations new highs and lows                 
if (!mysql_query($query)) {
  die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
}
mysql_close($connection);
?>
