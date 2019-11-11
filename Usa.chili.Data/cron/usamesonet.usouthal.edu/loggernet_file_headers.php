#!/usr/bin/php
<?php
// 2010-01-27 dnb Original version
//            Prints column headers for a TOA5 format LoggerNet file

//
// Verify that the correct number of arguments was specified
//
if ($argc != 2) {
  die("usage: loggernet_file_headers <filename>\n");
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
// Skip past the first header record
// 
fgetcsv($handle, 0, ",");

// Get and print column header record in single column format
$rec = fgetcsv($handle, 0, ",");
for ($i=0; $i<count($rec); $i++) {
  echo "$rec[$i]\n";
}

fclose($handle);
?>
