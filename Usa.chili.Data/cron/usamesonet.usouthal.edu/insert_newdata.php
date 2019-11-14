#!/usr/bin/php
<?php
//   This script is designed to be invoked by LoggerNet TaskMaster after
//   completing communications with a station. The script will extract
//   the most recent data from the specified station's LoggerNet files and
//   update the appropriate MySQL tables for the web server.
//
// Revision history:
// 2011-09-27 dnb Original version
// 2013-12-18 dnb Added support for Poarch Creek station
// 2018-05-08 wdt Added support for North Ashford station
//
// Notes:
// This script implements improvements on an eariler script (insert_loggernet.php)
// 1) Updates all applicable tables in one run
// 2) Inserts only database columns that have a LoggerNet table counterpart

$db_host = "chiliweb.usouthal.edu";
$db_username =  "chilistudent";
$db_password = "chilistudent";
$db_database = "chili";

$fn_pfx = "C:\\Campbellsci\\LoggerNet\\";

$station_file = array('agricola'    => 'Agricola_TableNNN.dat',
                      'andalusia'   => 'Andalusia_TableNNN.dat',
                      'ashford'     => 'Ashford_TableNNN.dat',
                      'atmore'      => 'Atmore_TableNNN.dat',
                      'bayminette'  => 'Bay Minette_TableNNN.dat',
                      'castleberry' => 'Castleberry_TableNNN.dat',
                      'disl'        => 'Dauphin Island_TableNNN.dat',
                      'dixie'       => 'Dixie_TableNNN.dat',
                      'elberta'     => 'Elberta_TableNNN.dat',
                      'fairhope'    => 'Fairhope_TableNNN.dat',
                      'florala'     => 'Florala_TableNNN.dat',
                      'foley'       => 'Foley_TableNNN.dat',
                      'gasque'      => 'Gasque_TableNNN.dat',
                      'geneva'      => 'Geneva_TableNNN.dat',
                      'grandbay'    => 'Grand Bay_TableNNN.dat',
                      'jay'         => 'Jay_TableNNN.dat',
                      'kinston'     => 'Kinston_TableNNN.dat',
                      'leakesville' => 'Leakesville_TableNNN.dat',
                      'loxley'      => 'Loxley_TableNNN.dat',
                      'mobiledr'    => 'Dog River_TableNNN.dat',
                      'mobileusa'   => 'USA Campus_TableNNN.dat',
                      'mobileusaw'  => 'USA Campus West_TableNNN.dat',
                      'mtvernon'    => 'Mt Vernon_TableNNN.dat',
                      'pascagoula'  => 'Pascagoula_TableNNN.dat',
                      'robertsdale' => 'Robertsdale_TableNNN.dat',
                      'saraland'    => 'Saraland_TableNNN.dat',
                      'walnuthill'  => 'Walnut Hill_TableNNN.dat',
                      'poarch'      => 'Poarch_TableNNN.dat',
					  'ashford_n'   => 'Ashford North_TableNNN.dat');

$voc_station = array('disl',
                     'elberta',
                     'fairhope',
                     'foley',
                     'gasque',
                     'grandbay',
                     'mobiledr',
                     'mobileusaw',
                     'pascagoula',
                     'robertsdale');

//
// Verify that the correct number of arguments was specified
//
if ($argc != 2) {
  die("Station key parameter missing, exiting...\n");
}
//
// Verify that the station key parameter is valid
//
$station_key = strtolower($argv[1]);
if (!array_key_exists($station_key, $station_file)) {
  die("Invalid station key ( $station_key ) parameter passed, exiting...\n");
}
//
// Determine which database tables to update based on station id (VOC or not?)
//
if ($station_key == 'poarch' || $station_key == 'ashford_n') {
	$table = array('202');
} else {
	if (!in_array($station_key, $voc_station)) {
  		$table = array('202','260','224');
	} else {
  		$table = array('202','260','224','voc');
	}
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

foreach ($table as $table_id) {
  //
  // Construct the name of the table to be updated
  //
  $tbl = $station_key . "_" . $table_id;

  // Use new table for 202
  if($table_id == '202') {
    $tbl = 'station_data';
  }

  //
  // Construct the name of the data file to be processed
  //
  $bn = str_replace("NNN",strtoupper($table_id),$station_file[$station_key]);
  $fn = $fn_pfx . $bn;
  //
  // Select the most recent timestamp from the selected station table
  // and retrieve the column names for this database table
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
  $mru_ts = $row['TS'];
  $dbtcnt = count($row);
  $dbtcol = array();
  for ($c=0; $c < $dbtcnt; $c++) {
    $dbtcol[$c] = mysql_field_name($result, $c);
  }
  mysql_free_result($result);

  //
  // Verify the requested file exists, get its size, and open it
  //
  if (file_exists($fn)) {
    $cfs = sprintf("%u",filesize($fn));        // save current file size
    $cft = date("Y-m-d H:i:s",filemtime($fn)); // save current file time
    $handle = fopen($fn, "r");
    if (!$handle) {
      die("Unable to open $fn , exiting...\n");
    }
  } else {
    die("File $fn does not exist, exiting...\n");
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
  // Determine the offset to continue processing this file at
  //
  $query = sprintf('select FileSize from filestat where FileName="%s"', $bn);
  $result = mysql_query($query);
  if (!$result) {
      die("Could not query the database: " . mysql_error() . "\n");
  }
  if (mysql_num_rows($result) == 0) {
    // For a new file set the "previous file size" to 0
    $query = "insert filestat values(\"$bn\",\"0\",\"$cft\")";
    $result = mysql_query($query);
    if (!$result) {
      die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
    }
    $pfs = 0;
    // otherwise set "previous file size" to the stored previous file size
    // unless the current file size is less than pfs. In that case, set
    // previous file size to 0 and read the entire file
  } else {
    $row = mysql_fetch_row($result);
    $pfs = $row[0]; // Previous file size
    if ($cfs < $pfs) {
      $pfs = 0;
    }
  }
  
  // If not starting at the beginning of the file then
  // position to the last record already processed
  // 
  if ($pfs > 0) {
    fseek($handle,$pfs);
  }
  
  // Begin database update after reading a LoggerNet file timestamp
  // greater than the most recent corresponding database table timestamp
  while (($rec = fgetcsv($handle, 0, ",")) != FALSE) {
    $ts = $rec[0];
    if ($ts <= $mru_ts) continue;
    $query = "insert $tbl set ";
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
  // 
  // Update the filestat table to reflect the current size of the file
  // 
  $query = "update filestat set FileSize=\"$cfs\",StatTime=\"$cft\"";
  $query .= " where FileName=\"$bn\"";
  $result = mysql_query($query);
  if (!$result) {
    die("Failed query: $query had mysql_error: " . mysql_error() . "\n");
  }
  fclose($handle);
}
  
mysql_close($connection);
?>
