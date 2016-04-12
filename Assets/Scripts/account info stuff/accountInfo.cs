using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/**
*create by Ryan Young
    last updated 3/22/16
*/
public class accountInfo : MonoBehaviour {

    private string userName;
    private List<string> friends;
    private int friendsCount;
    private stats playerRecord;
    public Text warning;

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
        StartCoroutine(loadFriends());

        

        //at this point the player is know to exist and stats will access database to get player stats
        playerRecord = new stats(userName);
    }

    internal bool hasFriend(string friend)
    {
        if (friends.Contains(friend))
        {
            return true;
        }
        return false;
    }

    /**
    *loads friends list from the database
    *not functional until php script is finished
    */
    private IEnumerator loadFriends()
    {
        //need to change the php script name here
        string post_url = "http://proj-309-38.cs.iastate.edu/php/getfriends.php?" + "username=" + WWW.EscapeURL(userName);
        WWW f_check = new WWW(post_url);
        yield return f_check;
        if (f_check.error != null)
        {
            Debug.Log("problem loading friends list");
        }
        else if(f_check.text.Equals("none"))
        {
            Debug.Log("you have no friends");
        }
        else
        {
            String[] friends = f_check.text.Split(","[0]);
            foreach(string friend in friends)
            {
                Debug.Log("friend found:" + friend);
                this.addFriend(friend);
                friendsCount++;
            }
        }

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
        friends.Sort();
        return friends.ToArray();
    }

    public void addFriend(string Friend)
    {
        //extend this to check data base to ensure that the friend exists in record
        if (Friend != null)
        {
            friends.Add(Friend);
            friendsCount++;
        }
        
    }

    public stats getStats()
    {
        return playerRecord;
    }

}
