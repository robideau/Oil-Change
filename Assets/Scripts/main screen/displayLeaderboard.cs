using UnityEngine;
using System.Collections;

public class displayLeaderboard : MonoBehaviour {

	private bool updating;
	private string[] leaderboard;

	public void getLeaderboard()
	{
		updating = false;
		Debug.Log("updating leaderboard");
		StartCoroutine(updateRankings());
		Debug.Log("Leaderboard updated");
		StartCoroutine(fetchLeaderboard());
	}

	//updates leaderboard in database
	private IEnumerator updateRankings()
	{
		updating = true;
		string post_url = "http://proj-309-38.cs.iastate.edu/php/updaterankings.php?";
		WWW u_check = new WWW(post_url);
		yield return u_check;
		updating = false;
	}

	private IEnumerator fetchLeaderboard()
	{
		//wait for leaderboards to update
		while (updating)
		{
			yield return new WaitForSeconds(0.1f);
		}

		//connect to server and gather leaderboard
		//returned string is sorted by ranking
		string post_url = "http://proj-309-38.cs.iastate.edu/php/getleaderboard.php";
		WWW lb_check = new WWW(post_url);
		yield return lb_check;
		leaderboard = lb_check.text.Split('\n');
	}

}
