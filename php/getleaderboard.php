<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$username = mysqli_real_escape_string($database, $_GET['username']);
	
    $query = "SELECT * FROM leaderboard ORDER BY ranking";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 
    $num_results = mysqli_num_rows($result);
 
 
	if($num_results > 0)
	{
		$retStr = "";
	
		while($row = mysqli_fetch_array($result))
		{
			$retStr .= $row['username'];
			$retStr .= "," . $row['ranking'];
			$retStr .= "," . $row['matchesWon'];
			$retStr .= "," . $row['matchesLost'];
			$retStr .= "," . $row['matchesTied'];
			$retStr .= "," . $row['highScore'];
			$retStr .= "," . $row['lastPlayed'];
			$retStr .= "\n";
		}
		
		echo $retStr;
	}
	else
	{
		echo 'nouser';
	}
?>