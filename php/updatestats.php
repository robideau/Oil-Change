<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$username = mysqli_real_escape_string($database, $_GET['username']);
	$result = intval(mysqli_real_escape_string($database, $_GET['result']));
	$score = floatval(mysqli_real_escape_string($database, $_GET['score']));
	
    $query = "SELECT * FROM leaderboard WHERE username='$username'";
	
	$queryResult = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 
    $num_results = mysqli_num_rows($queryResult);
 
	if($num_results > 0)
	{
		$row = mysqli_fetch_array($queryResult);
		$matchesWon = $row['matchesWon'];
		$matchesLost = $row['matchesLost'];
		$matchesTied = $row['matchesTied'];
		$highScore = $row['highScore'];
		
		if($result == 1)
		{
			$matchesWon += 1;
		}
		else if($result == 0)
		{
			$matchesTied += 1;
		}
		else if($result == -1)
		{
			$matchesLost += 1;
		}
		
		if($score < $highScore)
		{
			$highScore = $score;
		}
		
		$rawScore = 2*$matchesWon + $matchesTied - $matchesLost;
		
		$query = "UPDATE leaderboard SET lastPlayed=NOW(), matchesWon=$matchesWon, matchesLost=$matchesLost, " .
		         "matchesTied=$matchesTied, highScore=$highScore, rawScore=$rawScore WHERE username='$username'";
		mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
	}
?>