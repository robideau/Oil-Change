using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {

    private int playedGames;
    private int wonGames;
    private int lostGames;
    private int tiedGames;
    private int rank;
    private int HighScore;
    private int LastScore;

    /**
    *new player made so all stats default to zero
    */
	public stats()
    {
        playedGames = 0;
        wonGames = 0;
        lostGames = 0;
        tiedGames = 0;
        HighScore = 0;
        LastScore = 0;
        rank = 1;
        

    }

    /**
    *
    *this stats constructor will fetch existing statistics from the data base
    *
    */
    public stats(string playerName)
    {
        //unity does not allow constructor calls from other constructors... go figure
        playedGames = 0;
        wonGames = 0;
        lostGames = 0;
        tiedGames = 0;
        HighScore = 0;
        LastScore = 0;
        rank = 1;
        fetchStats(playerName);
    }

    /**
    *actually fetches player stats
    */
    IEnumerator fetchStats(string playerName)
    {
        //connect to server and gather stat information
        string post_url = "http://proj-309-38.cs.iastate.edu/php/profileinfo.php?" + "username=" + WWW.EscapeURL(playerName);
        WWW pStat_check = new WWW(post_url);
        yield return pStat_check;
        string breakDown = pStat_check.text;
        
        //user does not exist could not fetch stats (maybe add a temporary warning about how the player stats failed to load
        if (breakDown.Equals("nouser"))
        {

        }
        //load in player stats as the should be
        else
        {
            string[] eachStat = breakDown.Split(","[0]);
            rank = int.Parse(eachStat[0]);
            wonGames = int.Parse(eachStat[0]);
            lostGames = int.Parse(eachStat[1]);
            tiedGames = int.Parse(eachStat[2]);
            HighScore = int.Parse(eachStat[3]);
            LastScore = int.Parse(eachStat[4]);
        }
        
        

    }

    /**
    *when a match complete update the player record and forward the updated data.
    */
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
    public int getLastScore()
    {
        return LastScore;
    }

    /**
    *returns player rank
    */
    public int getRank()
    {
        return rank;
    }

}
