<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$sessionName = mysqli_real_escape_string($database, $_GET['sessionName']);
	
	if(trim($sessionName) == '')
	{
		echo 'empty';
		return;
	}
	
    $query = "SELECT sessionName FROM sessions WHERE sessionName='$sessionName'";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 	
    $num_results = mysqli_num_rows($result);
 
	if($num_results > 0)
	{
		$query = "DELETE FROM sessions WHERE sessionName='$sessionName'";
	
		mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
		
		echo 'success';
	}
	else
	{
		echo 'session_does_not_exist';
	}
?>