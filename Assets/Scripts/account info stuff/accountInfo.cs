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
    private string email;
    private stats playerRecord;

    public accountInfo()
    {

    }

    //added features
    //private IP address for this account?

    /**
    *checks data base for existing player name and loads in player information
    */
	public void loadAccount(string existingPlayer)
    {
        //to do check for player existence
        //load in player friends list
        //load in player IP?
        //load in player email if account name given
        //load in player account name if email given
        //have stats loaded in aswell

        //for now I will just store the player name as existing player and initialize friends list as empty
        //and set email to default
        userName = "player";
        friends = new List<string>();
        email = "@nothing";
        friendsCount = 0;

        //at this point the player is know to exist and stats will access database to get player stats
        playerRecord = new stats(userName);
    }

    /**
    * a new player account is made so send to data base the player name and password
    * initialize other player info to default new player
    */
    public void newAccount(string newPlayerName,string newPlayerEmail, string password)
    {
        //set username email initialize empty friends list and new stats with 0 stats so far
        userName = newPlayerName;
        email = newPlayerEmail;
        friends = new List<string>();
        playerRecord = new stats();
        friendsCount = 0;

        //all the new information will need to be sent to data base via forwardData()
    }

    /**
    *will update player information in the data base or create new player information if needed
    */
    public void forwardData()
    {
        //need to forward current data to database (includes all statistic information)


    }

    /**
    *helper method for gather player data
    */
    private void fetchData()
    {
        //needs to fetch data from database
    }

    /**
    *returns account name
    */
    public string getName()
    {
        return userName;
    }

    /**
    *returns account email
    */
    public string getEmail()
    {
        return email;
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

    public void setEmail(string newEmail)
    {
        email = newEmail;
    }
}
