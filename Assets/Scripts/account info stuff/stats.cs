using UnityEngine;
using System.Collections;
using System;

public class stats : MonoBehaviour {

    private int playedGames;
    private int wonGames;
    private int lostGames;
    private int tiedGames;
    private int rank;
    private int HighScore;
    private string LastPlayed;

    private bool updating = false;

    /**
    *new player made so all stats default to zero
    */
	public void setNew()
    {
        playedGames = 0;
        wonGames = 0;
        lostGames = 0;
        tiedGames = 0;
        HighScore = 0;
        LastPlayed = "";
        rank = 1;
        

    }

    /**
    *
    *this stats constructor will fetch existing statistics from the data base
    *
    */
    public void loadStats(string playerName)
    {
        //unity does not allow constructor calls from other constructors... go figure
        playedGames = 0;
        wonGames = 0;
        lostGames = 0;
        tiedGames = 0;
        HighScore = 0;
        LastPlayed = "";
        rank = 1;
        updateRank(playerName);
    }

    public void updateRank(string playerName)
    {
        Debug.Log("updating leaderboard");
        StartCoroutine(updateLeaderBoard());
        Debug.Log("Leaderboard updated");
        StartCoroutine(fetchStats(playerName));
    }

    //updates leaderboard in database
    private IEnumerator updateLeaderBoard()
    {
        updating = true;
        string post_url = "http://proj-309-38.cs.iastate.edu/php/updaterankings.php?";
        WWW u_check = new WWW(post_url);
        yield return u_check;
        updating = false;
    }

    /**
*actually fetches player stats
*/
    private IEnumerator fetchStats(string playerName)
    {
        //wait for leaderboards to update
        while (updating)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //connect to server and gather stat information
        string post_url = "http://proj-309-38.cs.iastate.edu/php/profileinfo.php?" + "username=" + WWW.EscapeURL(playerName);
        WWW pStat_check = new WWW(post_url);
        yield return pStat_check;
        string breakDown = pStat_check.text;
        Debug.Log("while fetching stats: " + breakDown);
        //user does not exist could not fetch stats (maybe add a temporary warning about how the player stats failed to load
        if (breakDown.Equals("nouser"))
        {

        }
        //load in player stats as the should be
        else
        {
            string[] eachStat = breakDown.Split(","[0]);
            rank = int.Parse(eachStat[0]);
            Debug.Log("" + rank);
            wonGames = int.Parse(eachStat[0]);
            lostGames = int.Parse(eachStat[1]);
            tiedGames = int.Parse(eachStat[2]);
            HighScore = int.Parse(eachStat[3]);
            LastPlayed = eachStat[4];
        }
        
        
		playedGames = wonGames + lostGames + tiedGames;
    }

    /**
    when a match complete update the player record and forward the updated data.

	UNUSED

    public void completeMatch(int winStatus, int xpEarned)
    {

        //winstatus > 0 is a win < 0 is a loss == 0 is a tie
        playedGames++;
        if(winStatus >= 1)
        {
            wonGames++;
        }
        else if(winStatus <= -1)
        {
            lostGames++;
        }
        else
        {
            tiedGames++;
        }


        //                  todo
        //still need to figure out how ranking updating will work so nothing done with ranking
        //also need to determine how a match is scored and update previous game score/highscore
        //also need to send the updated data still


    }
    */

    /**
    *returns the number of games won
    */
    public int getWins()
    {
        return wonGames;
    }

    /**
    *returns the number of games lost
    */
    public int getLosses()
    {
        return lostGames;
    }

    /**
    *return the number of games tied
    */
    public int getTies()
    {
        return tiedGames;
    }

    /**
    *returns the number of games played
    */
    public int getPlayed()
    {
        return playedGames;
    }

    /**
    *returns score from the best game played
    */
    public int getHighScore()
    {
        return HighScore;
    }

    /**
    *returns score from the last game played
    */
    public string getLastPlayed()
    {
        return LastPlayed;
    }

    /**
    *returns player rank
    */
    public int getRank()
    {
        return rank;
    }

}
