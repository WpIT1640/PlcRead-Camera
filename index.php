<?php
/*
TODO
setup up a loop so that we have a timer running to grab the photos every so many minutes.

*/
//camera user, pass, and address
$login = 'admin';
$password = 'Texas123';
$url = 'http://172.33.201.34/cgi-bin/snapshot.cgi';

//folder setup using current date 
$folder_name ;
$year = date("Y");
$month = date("m");
$day = date("d");

$folder_name = "$year/$month/$day";
//check if directory exists if not make new folder
if (!is_dir($folder_name)){
	if(!mkdir($folder_name, 755, true)) {
		die('Failed to create folders...');
}
}

//grab photo and save to folder
$ch = curl_init();
$orig = NULL;
curl_setopt($ch, CURLOPT_URL,$url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER,1);
curl_setopt($ch, CURLOPT_HTTPAUTH, CURLAUTH_ANY);
curl_setopt($ch, CURLOPT_USERPWD, "$login:$password");
$result = curl_exec($ch);
curl_close($ch);
$im = imagecreatefromstring($result);
$now = date("U");
$newfile = "/tmp/$now.jpg";

//file_put_contents($newfile, $orig);
$newnew = imagecreatetruecolor(960,540);
imagecopyresized($newnew, $im, 0, 0, 0, 0, 960, 540, 1920, 1080);
$newnewfile = "./$folder_name/$now-r.jpg";
imagejpeg($newnew, $newnewfile);
imagedestroy($im);
imagedestroy($newnew);
//echo "<img src=\"$folder_name/$now-r.jpg\">";
?>