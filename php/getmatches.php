<?php
    $database = mysqli_connect('mysql.cs.iastate.edu', 'dbu309grp38', 'RM8MjM3yYCw', 'db309grp38') or die('Could not connect: ' . mysqli_connect_error());
	
	$sortBy = mysqli_real_escape_string($database, $_GET['sortBy']);
	$reverse = mysqli_real_escape_string($database, $_GET['reverse']);
	$searchUser = mysqli_real_escape_string($database, $_GET['searchUser']);
		
	$query = "SELECT * FROM sessions";
	
 	if(strlen($sortBy) > 0)
	{
		$query .= " ORDER BY $sortBy";
		if($reverse == 'TRUE')
		{
			$query .= " DESC";
		}
	}

	if(strlen($searchUser) > 0)
	{
		$query .= " WHERE hostUser=$searchUser";
	}
	
	$result = mysqli_query($database, $query) or die('Query failed: ' . mysqli_error());
	
	while($row = mysqli_fetch_assoc($result))
	{
	   $retStr = $row['sessionName'] . "; ";
	   $retStr .= $row['hostUser'] . "; ";
	   $retStr .= $row['buildTime'] . "; ";
	   $retStr .= $row['buildLimit'] . "; ";
	   $retStr .= $row['keywords'] . "; ";
	   $retStr .= $row['pass'] . "; ";
	   $retStr .= $row['inviteOnly'] . "\n";
	   echo $retStr;
	}
?>