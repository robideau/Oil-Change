<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
		
    $query = "SELECT username,rawScore FROM leaderboard ORDER BY rawScore DESC";
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
 
    $num_results = mysqli_num_rows($result);
	
	if($num_results > 0)
	{
		$curNumber = 1;
		$prevRank = 1;
		$prevScore = null;
		$ranking = null;
		
		while($row = mysqli_fetch_array($result))
		{
			$username = $row['username'];
			$rawScore = $row['rawScore'];
			
			if($rawScore != $prevScore)
			{
				$ranking = $curNumber;
			}
			else
			{
				$ranking = $prevRank;
			}
		
			$query = "UPDATE leaderboard SET ranking=$ranking WHERE username='$username'";
			mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
			
			$prevRank = $ranking;
			$prevScore = $rawScore;
			$curNumber += 1;
		}
	}
?>