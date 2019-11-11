#!/usr/bin/php
<?php
// 2010-01-27 dnb Original version
//            Verify that the number of data columns in the file match
//            the number of columns specified for the files table_code
// 2010-01-29 dnb Added support for Table260

$table_code = 2; // record array element containing TableCode
$table_cols = array(202 => 80,
	            207 => 202,
	            224 => 202,
	            260 => 202);
//
// Verify that the correct number of arguments was specified
//
if ($argc != 2) {
  die("usage: loggernet_file_check <filename>\n");
}


//
// Verify the requested file exists and open it
//
$fn = $argv[1];
if (file_exists($fn)) {
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
// Skip past the header records
// 
for ($i=0; $i < 4; $i++) {
  fgetcsv($handle, 0, ",");
}

// Get first data record and verify column count against table_code specs
$rec = fgetcsv($handle, 0, ",");
$rectc = $rec[$table_code];
if (count($rec) != $table_cols[$rectc]) {
  echo "ERROR: column count (" . count($rec) . ") in $fn DOES NOT MATCH\n";
  echo "       column count ($table_cols[$rectc]) for table_code $rectc \n";
} else {
  echo "CORRECT: column count (" . count($rec) . ") in $fn matches\n";
  echo "         column count ($table_cols[$rectc]) for table_code $rectc\n";
}

fclose($handle);
?>
