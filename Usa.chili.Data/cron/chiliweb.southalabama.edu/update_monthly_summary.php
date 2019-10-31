#!/usr/bin/php
<?php
// 2013-02-25 dnb original version
// 2013-03-11 dnb Added time stamps for max and min fields

$usage = "Usage: " . basename($argv[0]) . " [YYYY MM]\n";
// $db_host = "chiliweb.usouthal.edu";
$db_host     = "localhost";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";

$ts_tpl   = '%04u-%02u-01 00:00:00';
$stations = array('agricola',
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


//
// Determine whether to report previous month or the specified year and month
//
if ($argc == 1) {
  // No argument case, report previous month
  $end_str   = sprintf($ts_tpl,date('Y'),date('m'));
  $end_tm    = strtotime($end_str);
  $start_tm  = strtotime("-1 month", $end_tm);
  $start_str = date('Y-m-d H:i:s', $start_tm);
  $year      = date('Y',$start_tm);
  $month     = date('m',$start_tm);
} elseif ($argc == 3) {
  // Validate the year and month supplied
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

//
// Initialize database connection
//
$link = mysqli_connect($db_host,$db_username,$db_password,$db_database);
if (!$link) {
  die('Connect Error (' . mysqli_connect_errno() . ') ' . mysqli_connect_error());
}

//
// Function definitions
//
function dew_point($at, $rh) {
  if ($at >= 60 || $rh > 100 || $rh < 1) {
    return NULL;
  } else {
  // Define constants for Dew Point calculation (Deg C)
    if ($at < 0 ) {
      $a = 22.452;
      $b = 272.55;
    } else {
      $a = 17.502;
      $b = 240.97;
    }
    $gamma = (($a * $at) / ($b + $at)) + log($rh / 100);
    return round((($b * $gamma)/($a - $gamma)), 2);
  }
}

function fahrenheit($t) {
  $ninefifths = 9/5;
  return ($ninefifths * $t + 32);
}

function celsius($t) {
  $fiveninths = 5/9;
  return (($t - 32) * $fiveninths);
}

function mph($v) {
  $mps2Mph = 2.2369363;
  return ($v * $mps2Mph);
}

function heat_index($at,$rh) {
  $dp = dew_point($at, $rh);
  if ($dp == NULL) {
    return NULL;
  }
  if (fahrenheit($at) < 80 || $rh < 40 || fahrenheit($dp) < 54) {
    return NULL;
  }
  // Define constants for Heat Index calculation (Deg F)
  $hi1 = 42.379;
  $hi2 = 2.04901523;
  $hi3 = 10.14333127;
  $hi4 = 0.22475541;
  $hi5 = 0.00683783;
  $hi6 = 0.05481717;
  $hi7 = 0.00122874;
  $hi8 = 0.00085282;
  $hi9 = 0.00000199;

  $t  = fahrenheit($at);
  $t2 = $t * $t;
  $r  = $rh;
  $r2 = $r * $r;
  $hi = $hi2 * $t +
        $hi3 * $r -
        $hi4 * $t * $r -
        $hi5 * $t2 -
        $hi6 * $r2 +
        $hi7 * $t2 * $r +
        $hi8 * $t * $r2 -
        $hi9 * $t2 * $r2 - $hi1;
  if ($hi < $t) {
    return NULL;
  } else {
    return (round(celsius($hi), 2));
  }
}

function wind_chill($at,$ws) {
  $at_f   = fahrenheit($at);
  $ws_mph = mph($ws);
  if ($at_f > 50.0 || $ws_mph <= 3.0) {
    return NULL;
  }
  // Define constants for Wind Chill calculation (Deg F)
  $wc1 = 35.74;
  $wc2 = 0.6215;
  $wc3 = 35.75;
  $wc4 = 0.4275;

  $wc = $wc1 +
        $wc2 * $at_f -
        $wc3 * pow($ws_mph, 0.16) +
        $wc4 * $at_f * pow($ws_mph, 0.16);
  return (round(celsius($wc), 2));
}
//
// Process the requested month for all stations
//
$summary = array();
foreach ($stations as $s) {
  //
  // Calculate Heat Index statistics
  //
  $qry_hi = 'select %s_202.TS as TS, %s_202.AirT_2m as AirT_2m, %s_202.RH_2m as RH_2m ' .
            'from %s_202 join %s_202_flags on %s_202.TS = %s_202_flags.TS ' .
            'where %s_202.TS >= "%s" and %s_202.TS < "%s" ' .
            'and %s_202_flags.AirT_2m_flag = 0 and %s_202_flags.RH_2m_flag = 0 ' .
            'order by %s_202.TS asc';
  $query = sprintf($qry_hi,$s,$s,$s,$s,$s,$s,$s,$s,$start_str,$s,$end_str,$s,$s,$s);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $htidx = array();
  while ($row = mysqli_fetch_assoc($result)) {
    $hi = heat_index($row['AirT_2m'], $row['RH_2m']);
    if ($hi) {
      $htidx[$row['TS']] = $hi;
    }
  }
  if (count($htidx)) {
    $himax = max($htidx);
    $himin = min($htidx);
    $summary[$s]['HtIdx_Max']    = $himax;
    $summary[$s]['HtIdx_Max_TS'] = '"' . array_search($himax, $htidx) . '"';
    $summary[$s]['HtIdx_Min']    = $himin;
    $summary[$s]['HtIdx_Min_TS'] = '"' . array_search($himin, $htidx) . '"';
    $hisum = 0;
    foreach ($htidx as $h) {
      $hisum += $h;
    }
    $summary[$s]['HtIdx_Avg'] = round($hisum / count($htidx), 2);
  }
  //
  // Calculate Wind Chill statistics
  //
  $qry_wc = 'select %s_202.TS as TS, %s_202.AirT_2m as AirT_2m, %s_202.WndSpd_10m as WndSpd_10m ' .
            'from %s_202 join %s_202_flags on %s_202.TS = %s_202_flags.TS ' .
            'where %s_202.TS >= "%s" and %s_202.TS < "%s" ' .
            'and %s_202_flags.AirT_2m_flag = 0 and %s_202_flags.WndSpd_10m_flag = 0 ' .
            'order by %s_202.TS asc';
  $query = sprintf($qry_wc,$s,$s,$s,$s,$s,$s,$s,$s,$start_str,$s,$end_str,$s,$s,$s);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $wdchl = array();
  while ($row = mysqli_fetch_assoc($result)) {
    $wc = wind_chill($row['AirT_2m'], $row['WndSpd_10m']);
    if ($wc) {
      $wdchl[$row['TS']] = $wc;
    }
  }
  if (count($wdchl)) {
    $wcmax = max($wdchl);
    $wcmin = min($wdchl);
    $summary[$s]['WdChl_Max']    = $wcmax;
    $summary[$s]['WdChl_Max_TS'] = '"' . array_search($wcmax, $wdchl) . '"';
    $summary[$s]['WdChl_Min']    = $wcmin;
    $summary[$s]['WdChl_Min_TS'] = '"' . array_search($wcmin, $wdchl) . '"';
    $wcsum = 0;
    foreach ($wdchl as $c) {
      $wcsum += $c;
    }
    $summary[$s]['WdChl_Avg'] = round($wcsum / count($wdchl), 2);
  }
  //
  // Calculate Max Wind Gust Speed (at 10m)
  //
  $qry_ws = 'select TS, WndSpd_10m_Max as WndSpd_Max ' .
            'from %s_202 ' .
            'where TS >= "%s" and TS < "%s"';
  $query = sprintf($qry_ws,$s,$start_str,$end_str);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $wdspd = array();
  while ($row = mysqli_fetch_assoc($result)) {
    if (!is_null($row['WndSpd_Max'])) {
      $wdspd[$row['TS']] = $row['WndSpd_Max'];
    }
  }
  if (count($wdspd)) {
    $wsmax = max($wdspd);
    if ($wsmax > 0) {
      $summary[$s]['WdSpd_Max']    = $wsmax;
      $summary[$s]['WdSpd_Max_TS'] = '"' . array_search($wsmax, $wdspd) . '"';
    }
  }
  //
  // Calculate Air Temperature statisitcs (at 2m)
  //
  $qry_at = 'select %s_202.TS as TS, AirT_2m ' .
            'from %s_202 join %s_202_flags on %s_202.TS = %s_202_flags.TS ' .
            'where %s_202.TS >= "%s" and %s_202.TS < "%s" ' .
            'and %s_202_flags.AirT_2m_flag = 0 ';
  $query = sprintf($qry_at,$s,$s,$s,$s,$s,$s,$start_str,$s,$end_str,$s);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $airt = array();
  while ($row = mysqli_fetch_assoc($result)) {
    if (!is_null($row['AirT_2m'])) {
      $airt[$row['TS']] = $row['AirT_2m'];
    }
  }
  if (count($airt)) {
    $atmax = max($airt);
    $atmin = min($airt);
    $summary[$s]['AirT_Max']    = $atmax;
    $summary[$s]['AirT_Max_TS'] = '"' . array_search($atmax, $airt) . '"';
    $summary[$s]['AirT_Min']    = $atmin;
    $summary[$s]['AirT_Min_TS'] = '"' . array_search($atmin, $airt) . '"';
    $atsum = 0;
    foreach ($airt as $t) {
      $atsum += $t;
    }
    $summary[$s]['AirT_Avg'] = round($atsum / count($airt), 2);
  }
  //
  // Calculate TB3 rainfall total
  //
  $qry_tb3 = 'select sum(%s_202.Precip_TB3_Tot) as RainTB3_Tot ' .
             'from %s_202 join %s_202_flags on %s_202.TS = %s_202_flags.TS ' .
             'where %s_202.TS >= "%s" and %s_202.TS < "%s" ' .
             'and %s_202_flags.Precip_TB3_Tot_flag = 0';
  $query = sprintf($qry_tb3,$s,$s,$s,$s,$s,$s,$start_str,$s,$end_str,$s);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $row = mysqli_fetch_assoc($result);
  if (!is_null($row['RainTB3_Tot'])) {
    $summary[$s]['RainTB3_Tot'] = $row['RainTB3_Tot'];
  }
  //
  // Calculate TE525 rainfall total
  //
  $qry_tx = 'select sum(%s_202.Precip_TX_Tot) as RainTX_Tot ' .
            'from %s_202 join %s_202_flags on %s_202.TS = %s_202_flags.TS ' .
            'where %s_202.TS >= "%s" and %s_202.TS < "%s" ' .
            'and %s_202_flags.Precip_TX_Tot_flag = 0';
  $query = sprintf($qry_tx,$s,$s,$s,$s,$s,$s,$start_str,$s,$end_str,$s);
  $result = mysqli_query($link,$query);
  if (!$result) {
    die("Could not query the database: " . mysqli_error($link));
  }
  $row = mysqli_fetch_assoc($result);
  if (!is_null($row['RainTX_Tot'])) {
    $summary[$s]['RainTX_Tot'] = $row['RainTX_Tot'];
  }
}

foreach ($summary as $s => $stnsum) {
  $update = "insert monthly_summary set Year=\"$year\",Month=\"$month\",StationKey=\"$s\"";
  foreach ($stnsum as $k => $v) {
    $update .= ",$k=$v";
  }
  $update .= " on duplicate key update ";
  foreach ($stnsum as $k => $v) {
    $update .= "$k=$v,";
  }
  $update = rtrim($update,",");
  $result = mysqli_query($link,$update);
  if (!$result) {
    die("Could not update the database: " . mysqli_error($link));
  }
}
mysqli_close($link);
?>
