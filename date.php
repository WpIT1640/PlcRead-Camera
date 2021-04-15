<?php
//get date and mkdir with date name
$folder_name ;
$year = date("Y");
$month = date("m");
$day = date("d");

$folder_name = "$year/$month/$day";

echo($folder_name);

if (!is_dir($folder_name)){
	if(!mkdir($folder_name, 755, true)) {
		die('Failed to create folders...');
}
}


?>