#!/usr/bin/php
<?php
// 2011-07-01 dnb Replaced USA Campus with USA Campus West
// 2012-07-26 dnb Removed Dauphin Island station from list
// 2013-01-30 dnb Added message generation time stamp to email
// 2013-04-24 dnb Removed Walnut Hill station from list
// include("db_login.php");
$db_host='localhost';
$db_database='chili';
$db_username='chiliweb';
$db_password='NONE';
// include("station_name.php");
$station_name = array('agricola' => 'Agricola',
                      'andalusia' => 'Andalusia',
                      'ashford' => 'Ashford',
                      'atmore' => 'Atmore',
                      'bayminette' => 'Bay Minette',
                      'castleberry' => 'Castleberry',
                      'dixie' => 'Dixie',
                      'elberta' => 'Elberta',
                      'fairhope' => 'Fairhope',
                      'florala' => 'Florala',
                      'foley' => 'Foley',
                      'gasque' => 'Gasque',
                      'geneva' => 'Geneva',
                      'grandbay' => 'Grand Bay',
                      'jay' => 'Jay',
                      'kinston' => 'Kinston',
                      'leakesville' => 'Leakesville',
                      'loxley' => 'Loxley',
                      'mobiledr' => 'Mobile (Dog River)',
                      'mobileusaw' => 'Mobile (USA Campus West)',
                      'mtvernon' => 'Mount Vernon',
                      'pascagoula' => 'Pascagoula',
                      'robertsdale' => 'Robertsdale',
                      'saraland' => 'Saraland',
		      'poarch' => 'Poarch Creek');
$onehour = 60*60;      // number of seconds in one hour
$oneday = $onehour*24; // number of seconds in one day
$oneweek = $oneday*7;  // number of seconds in one week

$connection = mysql_connect($db_host,$db_username,$db_password);
if (!$connection) {
   die("Could not connect to the database: " . mysql_error());
}
$db_select = mysql_select_db($db_database);
if (!$db_select) {
   die("Could not select the database: " . mysql_error());
}

// If DST is currently in effect decrement the current time by one
// hour in order to match the LoggerNet time which does not use DST
if (date('I') == 1) {
    $now = date('U') - $onehour;
}
else {
    $now = date('U');
}

$headers = 'From: chiliweb@usouthal.edu' . "\r\n" .
    'Reply-To: noreply@usouthal.edu' . "\r\n" .
    'X-Mailer: PHP/' . phpversion();
$message = "";
$subject = "ALERT - WX Communications problems";
$to = "<mesonettech@southalabama.edu>";

foreach ($station_name as $wx => $wx_name) {
    $table = $wx . "_202";
    $query = "select unix_timestamp(ts) from " . $table . " order by ts desc limit 1";
    $result = mysql_query($query);
    if (!$result) {
        die("Could not query the database: " . mysql_error());
    }

    if (mysql_num_rows($result) == 0) {
        die("No data available for table: $table");
    }

    while ($result_row = mysql_fetch_row($result)){
        $wx_time = $result_row[0];
    }
    mysql_free_result($result);
    $wx_delay = $now - $wx_time;
    $fmt_delay = "";
    if ($wx_delay >= $oneweek) {
        $weeks = 0;
        while ($wx_delay >= $oneweek) {
            $weeks += 1;
            $wx_delay -= $oneweek;
        }
        if ($weeks > 1) {
            $fmt_delay .= $weeks . " Weeks";
        } else {
            $fmt_delay .= $weeks . " Week";
        }
    }
    if ($wx_delay >= $oneday) {
        $days = 0;
        while ($wx_delay >= $oneday) {
            $days += 1;
            $wx_delay -= $oneday;
        }
        if ($fmt_delay != "") $fmt_delay .= ", ";
        if ($days > 1) {
            $fmt_delay .= $days . " Days";
        } else {
            $fmt_delay .= $days . " Day";
        }
    }
    if ($wx_delay >= $onehour) {
        $hours = 0;
        while ($wx_delay >= $onehour) {
            $hours += 1;
            $wx_delay -= $onehour;
        }
        if ($fmt_delay != "") $fmt_delay .= ", ";
        if ($hours > 1) {
            $fmt_delay .= $hours . " Hours";
        } else {
            $fmt_delay .= $hours . " Hour";
        }
    }
    if ($fmt_delay != "") {
        $message .= "No data received from " . $wx_name . " in " . $fmt_delay . "\r\n";
    }
}
if ($message == "") {
    $message = "All stations communicated successfully within the last hour";
}
$message .= "Message generated at: " . date('r') . "\r\n";
mail($to, $subject, $message, $headers);
?>
