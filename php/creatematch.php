<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$sessionName = mysqli_real_escape_string($database, $_GET['sessionName']);
	$hostUser = mysqli_real_escape_string($database, $_GET['hostUser']);
	$buildTime = mysqli_real_escape_string($database, $_GET['buildTime']);
	$buildLimit = mysqli_real_escape_string($database, $_GET['buildLimit']);
	$keywords = mysqli_real_escape_string($database, $_GET['keywords']);
	$pass = mysqli_real_escape_string($database, $_GET['pass']);
	$inviteOnly = mysqli_real_escape_string($database, $_GET['inviteOnly']);
	
	if(trim($sessionName) == '' or trim($hostUser) == '')
	{
		echo 'empty';
		return;
	}
	
    $query = "SELECT sessionName FROM sessions WHERE sessionName='$sessionName'";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 	
    $num_results = mysqli_num_rows($result);
 
	if($num_results > 0)
	{
		echo 'sessionexists';
	}
	else
	{
		//Create entry in users table. User can have up to 25 friends
		//   -> friends list is a comma separated string of length 525
		$query = "INSERT INTO sessions values('$sessionName', '$hostUser', $buildTime, "
		                                    . "$buildLimit, '$keywords', '$pass', $inviteOnly)";
		
		mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
				
		echo 'success';
	}
?>