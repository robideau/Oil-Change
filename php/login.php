<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$username = mysqli_real_escape_string($database, $_GET['username']);
	$password = mysqli_real_escape_string($database, $_GET['password']);
	
    $query = "SELECT username,pass FROM users WHERE username='$username'";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 
    $num_results = mysqli_num_rows($result);
 
	if($num_results > 0)
	{
		$row = mysqli_fetch_array($result);
		if($password == $row['pass'])
		{
			echo 't';
			
			//Update 'last played' entry in leaderboards
			$query = "UPDATE leaderboard SET lastPlayed=NOW() WHERE username='$username'";
			mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
		}
		else
		{
			echo 'f';
		}
	}
	else
	{
		echo 'nouser';
	}
?>