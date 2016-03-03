using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {

    private int playedGames;
    private int wonGames;
    private int lostGames;
    private int rank;
    private int xp;


	public stats()
    {
        playedGames = 0;
        wonGames = 0;
        lostGames = 0;
        rank = 1;
        xp = 0;
    }

    public stats(string playerName)
    {
        //statistics will need to be loaded from data base

    }

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

        //may consider losing xp for lost games and using rank to match players in ranked matches
        xp += xpEarned;

        //some algorithm in deciding a level up in rank


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
    *returns the number of games played
    */
    public int getPlayed()
    {
        return playedGames;
    }

    /**
    *returns player rank
    */
    public int getRank()
    {
        return rank;
    }

    /**
    *returns player current rank xp
    */
    public int getXp()
    {
        return xp;
    }

    /**
    *returns current needed xp for next rank
    */
    public int getRankXp()
    {
        return 0;
    }
}
