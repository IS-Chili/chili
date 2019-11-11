#!/usr/bin/php
<?php
//
// qc.php sets QC flags in the various *_202_flags tables based on the
// value of the observations in the related *_202 tables.
//
// By default qc.php will process all of the *_202 tables beginning
// with the next time stamp after the latest one stored in the related
// *_202_flags table.
//
// This default behavior can be overridden using command line switches
// as follows:
// -b "YYYY-MM-DD HH:MM:SS" specifies a time stamp to begin processing
// -e "YYYY-MM-DD HH:MM:SS" specifies a time stamp to end processing
// -s <stationid> specifies a station id. -s can be used multiple times
//
// 2012-04-27 dnb Initial skeletal version
// 2012-09-05 dnb Initial complete version
// 2012-09-06 dnb Modified flag logic to record NULL values as MISSING
//                and to limit flag vaules to a maximum of SUSPECT
//                which prevents artificially high flag values due to
//                observations being used in multiple like sensor tests
// 2012-09-18 dnb Modified like sensor tests so that no sensor is used
//                in more that one like sensor test
//
// Begin Global variables
$flag      = array();
$ls        = array();
$max       = array();
$min       = array();
$stations  = array();
$ts_beg    = NULL;
$ts_end    = NULL;
$ts_last   = NULL;
$allsites  = TRUE;
$sincelast = TRUE;

// QC value definitions
$good    = 0;
$warning = 1;
$suspect = 2;
$missing = 3;

// Database variables
$db_host     = "localhost";
// $db_host     = "chiliweb.usouthal.edu";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";
$mysqli      = NULL;

$all_stations = array('agricola',
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

$instruments  = array('Precip_TB3_Tot',
                      'Precip_TX_Tot',
                      'SoilSfcT',
                      'AirT_1pt5m',
                      'AirT_2m',
                      'AirT_9pt5m',
                      'AirT_10m',
                      'RH_2m',
                      'RH_10m',
                      'Pressure_1',
                      'Pressure_2',
                      'WndSpd_2m',
                      'WndSpd_10m',
                      'WndSpd_Vert');
// End Global variables

function initialize_qc_flags() {
  global $flag, $instruments;
  $flag['TS'] = NULL;
  foreach ($instruments as $obs) {
    $flag[$obs] = 0;
  }
}

// Initialize QC related variables
function initialize_qc_vars() {
  global $ls, $min, $max;

  // Initialize all QC flags to zero
  initialize_qc_flags();

  // Initialize each instrument's acceptable range
  $min['Precip_TB3_Tot'] = 0.0;
  $max['Precip_TB3_Tot'] = 2.25;

  $min['Precip_TX_Tot']  = 0.0;
  $max['Precip_TX_Tot']  = 2.0;

  $min['SoilSfcT']       = -5.5;
  $max['SoilSfcT']       = 53.5;

  $min['AirT_1pt5m']     = -4.0;
  $max['AirT_1pt5m']     = 35.5;

  $min['AirT_2m']        = -4.0;
  $max['AirT_2m']        = 35.5;

  $min['AirT_9pt5m']     = -2.5;
  $max['AirT_9pt5m']     = 35.5;

  $min['AirT_10m']       = -2.0;
  $max['AirT_10m']       = 35.0;

  $min['RH_2m']          = 20.0;
  $max['RH_2m']          = 100.0;

  $min['RH_10m']         = 20.0;
  $max['RH_10m']         = 100.0;

  $min['Pressure_1']     = 990;
  $max['Pressure_1']     = 1030;

  $min['Pressure_2']     = 990;
  $max['Pressure_2']     = 1030;

  $min['WndSpd_2m']      = 0.0;
  $max['WndSpd_2m']      = 6.0;

  $min['WndSpd_10m']     = 0.0;
  $max['WndSpd_10m']     = 7.5;

  $min['WndSpd_Vert']    = -1.5;
  $max['WndSpd_Vert']    = 1.5;

  // Initialize like sensor test values
  // Precipitation
  $ls['PC2_PC1']['v1']   = 'Precip_TX_Tot';
  $ls['PC2_PC1']['v2']   = 'Precip_TB3_Tot';
  $ls['PC2_PC1']['min']  = -0.25;
  $ls['PC2_PC1']['max']  = 0.5;
  // Pressure (station)
  $ls['PR2_PR1']['v1']   = 'Pressure_1';
  $ls['PR2_PR1']['v2']   = 'Pressure_2';
  $ls['PR2_PR1']['min']  = -0.75;
  $ls['PR2_PR1']['max']  = 0.75;
  // Air temp at 2m vs 1.5m
  $ls['T2_T15']['v1']   = 'AirT_1pt5m';
  $ls['T2_T15']['v2']   = 'AirT_2m';
  $ls['T2_T15']['min']  = -2.0;
  $ls['T2_T15']['max']  = 0.9;
  // Air temp at 10m vs 9.5m
  $ls['T10_T95']['v1']   = 'AirT_9pt5m';
  $ls['T10_T95']['v2']   = 'AirT_10m';
  $ls['T10_T95']['min']  = -0.25;
  $ls['T10_T95']['max']  = 0.55;
  // Relative Humidity at 10m vs 2m
  $ls['RH10_RH2']['v1']   = 'RH_2m';
  $ls['RH10_RH2']['v2']   = 'RH_10m';
  $ls['RH10_RH2']['min']  = -20.0;
  $ls['RH10_RH2']['max']  = 5.0;
  // Wind Speed at 10m vs 2m
  $ls['WS10_WS2']['v1']   = 'WndSpd_2m';
  $ls['WS10_WS2']['v2']   = 'WndSpd_10m';
  $ls['WS10_WS2']['min']  = -1.5;
  $ls['WS10_WS2']['max']  = 3.5;

}

function parse_args() {
  global $argc, $argv, $ts_beg, $ts_end, $sincelast, $allsites, $all_stations, $stations;
  for ($i=1; $i<$argc; $i++) {
    switch ($argv[$i]) {
      case "-b":
        if (($i + 1) < $argc) {
          $ts_beg = $argv[++$i];
          $sincelast = FALSE;
        }
        break;
      case "-e":
        if (($i + 1) < $argc) {
          $ts_end = $argv[++$i];
          $sincelast = FALSE;
        }
        break;
      case "-s":
        if (($i + 1) < $argc) {
          if (in_array($argv[$i+1], $all_stations)) {
            $stations[] = $argv[++$i];
            $allsites = FALSE;
          } else {
            die($argv[$i+1] . " is not a valid station id\n");
          }
        }
        break;
    }
  }
  if ($allsites) {
    $stations = $all_stations;
  }
}

function latest_ts($station) {
  global $mysqli;
  $query = sprintf('select TS from %s_202_flags order by TS desc limit 1', $station);
  $ts    = NULL;

  if ($result = $mysqli->query($query)) {
    if ($row = $result->fetch_assoc()) {
      $ts = $row['TS'];
    }
    $result->free();
  }

  return $ts;
}

function mk_select($station) {
  global $mysqli, $instruments, $ts_beg, $ts_end, $ts_last, $allsites, $sincelast;
  $query = 'select TS';
  foreach ($instruments as $obs) {
    $query .= ', ' . $obs;
  }
  $query .= sprintf(' from %s_202', $station);
  if ($sincelast && $ts_last) {
    $query .= sprintf(' where TS > "%s"', $ts_last);
  } else {
    if (isset($ts_beg)&& isset($ts_end)) {
      $query .= sprintf(' where TS >= "%s" and TS <= "%s"', $ts_beg, $ts_end);
    }
    if (!isset($ts_beg)&& isset($ts_end)) {
      $query .= sprintf(' where TS <= "%s"', $ts_end);
    }
    if (isset($ts_beg)&& !isset($ts_end)) {
      $query .= sprintf(' where TS >=  "%s"', $ts_beg);
    }
  }
  $query .= ' order by TS asc';
  return $query;
}

function mk_update($station) {
  global $flag, $instruments;
  $query = sprintf('replace %s_202_flags set TS = "%s"', $station, $flag['TS']);
  foreach ($instruments as $obs) {
    $query .= ', ' . $obs . '_flag' . ' = ' . $flag[$obs];
  }
  return $query;
}

function set_qc_flags($station) {
  global $flag, $instruments, $ls, $max, $min, $mysqli, $sincelast, $ts_last;
  global $good, $warning, $suspect, $missing;

  if ($sincelast) {
    $ts_last = latest_ts($station);
  }

  $query = mk_select($station);

  if ($result = $mysqli->query($query)) {

    while ($row = $result->fetch_assoc()) {
      // Reset all QC flags before processing each row of data
      initialize_qc_flags();
      $flag['TS'] = $row['TS'];

      // Perform instrument/climatological range checks on all observations
      foreach ($instruments as $obs) {
        if ($row[$obs] === NULL) {
          $flag[$obs] = $missing;
        } elseif ($row[$obs] < $min[$obs] || $row[$obs] > $max[$obs] ) {
          ++$flag[$obs];
        }
      }

      // Perform like sensor tests on selected observation pairs
      foreach (array_keys($ls) as $test) {
        if ($flag[$ls[$test]['v1']] == $missing ) continue;
        if ($flag[$ls[$test]['v2']] == $missing ) continue;

        $delta = $row[$ls[$test]['v2']] - $row[$ls[$test]['v1']];
        if ($delta < $ls[$test]['min'] || $delta > $ls[$test]['max'] ) {
          if ($flag[$ls[$test]['v1']] < $suspect) {
            ++$flag[$ls[$test]['v1']];
          }
          if ($flag[$ls[$test]['v2']] < $suspect) {
            ++$flag[$ls[$test]['v2']];
          }
        }
      }

      // Update the station's flag table with test results
      $query = mk_update($station);
      if (!$mysqli->query($query)) {
        printf("Error message: %s\n", $mysqli->error);
        printf("Failing SQL: %s\n", $query);
      }
    }
    $result->close();
  }
  // } else {
    // printf("Station %s returned no rows.\n", $station);
  // }

}

// Main

//
// Extract information from switches and set global variables accordingly
//
parse_args();

//
// Initialize QC variables
//
initialize_qc_vars();

//
// Initialize database object
//
$mysqli = new mysqli($db_host, $db_username, $db_password, $db_database);

if ($mysqli->connect_errno) {
  printf("Connect failed: %s\n", $mysqli->connect_error());
  exit();
}

//
// Process all requested stations
//

foreach ($stations as $station) {
  set_qc_flags($station);
}

//
// Close connection to database
//
$mysqli->close();

exit();
?>
