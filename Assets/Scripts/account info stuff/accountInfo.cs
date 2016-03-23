using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
*create by Ryan Young March 2
*/
public class accountInfo : MonoBehaviour {

    private string userName;
    private List<string> friends;
    private int friendsCount;
    private stats playerRecord;

    public accountInfo()
    {

    }

    //added features
    //private IP address for this account?

    /**
    *checks data base for existing player name and loads in player information
    *!!!this method should only be called when a confirmed player account is found!!!
    *
    */
	public void loadAccount(string existingPlayer)
    {
        //          todo
        //load in player friends list
        //load in player IP?
        //have stats loaded in

        //set player name to previously found player name
        userName = existingPlayer;
        friends = new List<string>();
        friendsCount = 0;

        //at this point the player is know to exist and stats will access database to get player stats
        playerRecord = new stats(userName);
    }

    /**
    * a new player account is made so send to data base the player name and password
    * initialize other player info to default new player
    */
    public void newAccount(string newPlayerName, string password)
    {
        //set username email initialize empty friends list and new stats with 0 stats so far
        userName = newPlayerName;
        friends = new List<string>();
        playerRecord = new stats();
        friendsCount = 0;

    }

    /**
    *will update player information in the data base or create new player information if needed
    */
    IEnumerator forwardData()
    {
        //need to forward current data to database (includes all statistic information)
        yield return new WaitForSeconds(0.1f);

    }

    /**
    *returns account name
    */
    public string getName()
    {
        return userName;
    }


    public string[] getFriends()
    {
        string[] friendsArray = new string[friendsCount];
        for(int i = 0; i < friendsCount; i++)
        {
            friendsArray[i] = friends[i];
        }

        return friendsArray;
    }

    public void addFriend(string Friend)
    {
        //extend this to check data base to ensure that the friend exists in record
        if (Friend != null)
        {
            friends.Add(Friend);
        }
        
    }

    public stats getStats()
    {
        return playerRecord;
    }

}
