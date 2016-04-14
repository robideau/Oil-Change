<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$username = mysqli_real_escape_string($database, $_GET['username']);
	$password = mysqli_real_escape_string($database, $_GET['password']);
	
	if($username == '' or $password == '')
	{
		echo 'empty';
		return;
	}
	
    $query = "SELECT username,pass FROM users WHERE username='$username'";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 
    $num_results = mysqli_num_rows($result);
 
	if($num_results > 0)
	{
		echo 'userexists';
	}
	else
	{
		//Create entry in users table. User can have up to 25 friends
		//   -> friends list is a comma separated string of length 525
		$query = "INSERT INTO users values('$username', '$password', '')";
		mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
		
		//Create entry in leaderboards table with default values
		$query = "INSERT INTO leaderboard values('$username', 0, 0, 0, 0, 100.0, NOW(), 0)";
		mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
		
		echo 'success';
	}
?>