#!/usr/bin/php
<?php
// NOTE: update_yday.php is invoked via LoggerNet TaskMaster,
// It is NOT a web page script
// update_yday.php updates the extremes_yday table with the high
// and low values of selected observations for the previous day
//
// 2010-10-20 dnb Modified dew point calculation to use more accurate formula
// 2010-12-07 dnb Removed dew point limits of below 0 and above 50 Celsius
// 2010-12-07 dnb Provided separate dew point constants for above or below 0 Celsius
// 2011-02-01 dnb Modified Dew point calculation to show N/A if RH > 100 not RH >= 100
// 2011-06-20 dnb Added support for USA Campus West station
// 2011-11-11 dnb Modified to retrieve data from _202 tables instead of retired _224 tables
// 2013-12-18 dnb Added support for Poarch Creek station
// 2018-05-08 wdt Added support for North Ashford station
//
// $db_host='localhost';
$db_host='chiliweb.usouthal.edu';
$db_database='chili';
$db_username='chilistudent';
$db_password='chilistudent';

$ye = array();
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

// Query strings for Yesterday's Extremes returns these variables
// $ye['TS']
$ye_time = 'select ' .
           'TS ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by TS desc limit 1';
// $ye['AirT_2m_TMx']
// $ye['AirT_2m_Max']
$ye_atmx = 'select ' .
           'TS as AirT_2m_TMx,' .
           'AirT_2m as AirT_2m_Max ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by AirT_2m_Max desc, TS asc limit 1';
// $ye['AirT_2m_TMn']
// $ye['AirT_2m_Min']
$ye_atmn = 'select ' .
           'TS as AirT_2m_TMn,' .
           'AirT_2m as AirT_2m_Min ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by AirT_2m_Min asc, TS asc limit 1';
// Get values used to calculate dew point
// $ye['DewPt_2m_TMx']
// $ye['DewPt_2m_Max']           
// $ye['DewPt_2m_TMn']
// $ye['DewPt_2m_Min']           
$ye_dwpt = 'select ' .
           'TS,' .
           'rh(RH_2m) as RH,' .
           'AirT_2m as AirT ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by TS asc';
// $ye['Precip_TB3_Today']
$ye_prcp = 'select ' .
           'sum(Precip_TB3_Tot) as Precip_TB3 ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate()';
// $ye['RH_2m_TMx']
// $ye['RH_2m_Max']           
$ye_rhmx = 'select ' .
           'TS as RH_2m_TMx,' .
           'rh(RH_2m) as RH_2m_Max ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by RH_2m_Max desc, TS asc limit 1';
// $ye['RH_2m_TMn']
// $ye['RH_2m_Min']           
$ye_rhmn = 'select ' .
           'TS as RH_2m_TMn,' .
           'rh(RH_2m) as RH_2m_Min ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'and rh(RH_2m) is not NULL ' .
           'order by RH_2m_Min asc, TS asc limit 1';
// $ye['WndSpd_10m_TMx']
// $ye['WndSpd_10m_Max']
$ye_wsmx = 'select ' .
           'TS as WndSpd_10m_TMx,' .
           'WndSpd_10m_Max ' .
           'from %s_202 ' .
           'where TS >= date_sub(curdate(),INTERVAL 1 DAY) and TS < curdate() ' .
           'order by WndSpd_10m_Max desc, TS asc limit 1';
           
// Iterate over all stations and collect Yesterday's extremes data
foreach ($station_id as $station) {
  // Collect Yesterday's extremes data for the selected station
  // Yesteray's last minute table time stamp
  $query = sprintf($ye_time,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  
  if (mysql_num_rows($result) == 0) {
    echo "No data for " . $station . " station on " . date('Y-m-d', strtotime('yesterday')) . ", skipping station\n";
    continue;
  } else {
    $row = mysql_fetch_assoc($result);
    $ye['TS'] = $row['TS'];
  }
  
  // Max Air Temp
  $query = sprintf($ye_atmx,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['AirT_2m_TMx'] = $row['AirT_2m_TMx'];
  $ye['AirT_2m_Max'] = $row['AirT_2m_Max'];
  
  // Min Air Temp
  $query = sprintf($ye_atmn,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['AirT_2m_TMn'] = $row['AirT_2m_TMn'];
  $ye['AirT_2m_Min'] = $row['AirT_2m_Min'];
  
  // Query today's RH and Temp data for Dew point calculation
  $query = sprintf($ye_dwpt,$station);
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
    $ye['DewPt_2m_Max'] = max($dewpt);
    $ye['DewPt_2m_TMx'] = array_search($ye['DewPt_2m_Max'],$dewpt);
    $ye['DewPt_2m_Min'] = min($dewpt);
    $ye['DewPt_2m_TMn'] = array_search($ye['DewPt_2m_Min'],$dewpt);
  } else {
    $ye['DewPt_2m_Max'] = NULL;
    $ye['DewPt_2m_TMx'] = NULL;
    $ye['DewPt_2m_Min'] = NULL;
    $ye['DewPt_2m_TMn'] = NULL;
  }
  
  // Yesterday's total Precipitation
  $query = sprintf($ye_prcp,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database for precip: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['Precip_TB3_Today'] = $row['Precip_TB3'];
  
  // Max Relative Humidity
  $query = sprintf($ye_rhmx,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['RH_2m_TMx'] = $row['RH_2m_TMx'];
  $ye['RH_2m_Max'] = $row['RH_2m_Max'];
  
  // Min Relative Humidity
  $query = sprintf($ye_rhmn,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['RH_2m_TMn'] = $row['RH_2m_TMn'];
  $ye['RH_2m_Min'] = $row['RH_2m_Min'];
  
  // Max Wind Speed
  $query = sprintf($ye_wsmx,$station);
  $result = mysql_query($query);
  if (!$result) {
    die("Could not query the database: " . mysql_error());
  }
  $row = mysql_fetch_assoc($result);
  $ye['WndSpd_10m_TMx'] = $row['WndSpd_10m_TMx'];
  $ye['WndSpd_10m_Max'] = $row['WndSpd_10m_Max'];
  
  //
  // If no row exists for this station then insert data, otherwise update
  //
  $query = sprintf('select * from extremes_yday where StationKey="%s"',$station);
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
            ' extremes_yday set ' .
            'TS=%s,' .
            'AirT_2m_TMx=%s,' .
            'AirT_2m_Max=%s,' .
            'AirT_2m_TMn=%s,' .
            'AirT_2m_Min=%s,' .
            'DewPt_2m_TMx=%s,' .
            'DewPt_2m_Max=%s,' .
            'DewPt_2m_TMn=%s,' .
            'DewPt_2m_Min=%s,' .
            'Precip_TB3_Today=%s,' .
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
  $ye['TS'] = '"' . $ye['TS'] . '"';
  if (is_null($ye['AirT_2m_TMx'])) {
    $ye['AirT_2m_TMx'] = 'NULL';
  } else {
    $ye['AirT_2m_TMx'] = '"' . $ye['AirT_2m_TMx'] . '"';
  }
  if (is_null($ye['AirT_2m_Max'])) {
    $ye['AirT_2m_Max'] = 'NULL';
  }
  if (is_null($ye['AirT_2m_TMn'])) {
    $ye['AirT_2m_TMn'] = 'NULL';
  } else {
    $ye['AirT_2m_TMn'] = '"' . $ye['AirT_2m_TMn'] . '"';
  }
  if (is_null($ye['AirT_2m_Min'])) {
    $ye['AirT_2m_Min'] = 'NULL';
  }
  if (is_null($ye['DewPt_2m_TMx'])) {
    $ye['DewPt_2m_TMx'] = 'NULL';
  } else {
    $ye['DewPt_2m_TMx'] = '"' . $ye['DewPt_2m_TMx'] . '"';
  }
  if (is_null($ye['DewPt_2m_Max'])) {
    $ye['DewPt_2m_Max'] = 'NULL';
  }
  if (is_null($ye['DewPt_2m_TMn'])) {
    $ye['DewPt_2m_TMn'] = 'NULL';
  } else {
    $ye['DewPt_2m_TMn'] = '"' . $ye['DewPt_2m_TMn'] . '"';
  }
  if (is_null($ye['DewPt_2m_Min'])) {
    $ye['DewPt_2m_Min'] = 'NULL';
  }
  if (is_null($ye['Precip_TB3_Today'])) {
    $ye['Precip_TB3_Today'] = 'NULL';
  }
  if (is_null($ye['RH_2m_TMx'])) {
    $ye['RH_2m_TMx'] = 'NULL';
  } else {
    $ye['RH_2m_TMx'] = '"' . $ye['RH_2m_TMx'] . '"';
  }
  if (is_null($ye['RH_2m_Max'])) {
    $ye['RH_2m_Max'] = 'NULL';
  }
  if (is_null($ye['RH_2m_TMn'])) {
    $ye['RH_2m_TMn'] = 'NULL';
  } else {
    $ye['RH_2m_TMn'] = '"' . $ye['RH_2m_TMn'] . '"';
  }
  if (is_null($ye['RH_2m_Min'])) {
    $ye['RH_2m_Min'] = 'NULL';
  }
  if (is_null($ye['WndSpd_10m_TMx'])) {
    $ye['WndSpd_10m_TMx'] = 'NULL';
  } else {
    $ye['WndSpd_10m_TMx'] = '"' . $ye['WndSpd_10m_TMx'] . '"';
  }
  if (is_null($ye['WndSpd_10m_Max'])) {
    $ye['WndSpd_10m_Max'] = 'NULL';
  }
  
  // Insert prepared values into MySQL query string
  $query = sprintf($qrytpl,
                   $ye['TS'],
                   $ye['AirT_2m_TMx'],
                   $ye['AirT_2m_Max'],
                   $ye['AirT_2m_TMn'],
                   $ye['AirT_2m_Min'],
                   $ye['DewPt_2m_TMx'],
                   $ye['DewPt_2m_Max'],
                   $ye['DewPt_2m_TMn'],
                   $ye['DewPt_2m_Min'],
                   $ye['Precip_TB3_Today'],
                   $ye['RH_2m_TMx'],
                   $ye['RH_2m_Max'],
                   $ye['RH_2m_TMn'],
                   $ye['RH_2m_Min'],
                   $ye['WndSpd_10m_TMx'],
                   $ye['WndSpd_10m_Max'],
                   $station);
  
  // Update extremes_yday table with this stations previous day's extremes
  if (!mysql_query($query)) {
    die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
  }
}
  
mysql_close($connection);
?>
