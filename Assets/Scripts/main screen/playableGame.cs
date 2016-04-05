using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
*create by Ryan Young
    last updated 3/24/16
*/
public class playableGame : MonoBehaviour {
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	/**
    *
    *name for this playable game
    *
    */
    private string gameName = "";

    /**
    *
    *name of the game creater (if any)
    *
    */
    private string host = "";

    /**
    *
    *build timelimit in seconds for the game being made
    *
    */
    private int buildTime;

    /**
    *
    *building block limit given to players for this playerable game
    *
    */
    private int buildLimit;
    
    /**
    *
    *array of keywords that describe the gameplay expected for this game
    *
    */
    //only 10 possible keywords for now...may change later
    private string[] keywords = new string[10];

    /**
    *total space used by keywords array, resized if needed on adder and setter methods
    */
    private int keySize = 10;

    /**
    password for this game session
    */
    private string password;

    /**
    *verifiy if the game session held is the hosts such that a joinable connection is set up
    */
    private bool isHost = false;

    //need to add array of game mods?
    //should password go here?

    /**
    *
    *default constructor for playableGame object, has no name or host
    *and has buildTime set to 120 and build limit set to 30
    *
    */
	public playableGame()
    {
        buildTime = 120;
        buildLimit = 30;
    }

    /**
    *
    *makes a playable game object based on paramaters
    *gameName = name of this game
    *buildTime = buld time for this game
    *buildLimit = build time limit for this game
    *host = player name who made this match
    *
    */
    public playableGame(string gameName, int buildTime, int buildLimit, string host, string password)
    {
        this.gameName = gameName;
        this.buildTime = buildTime;
        this.buildLimit = buildLimit;
        this.host = host;
        this.password = password;
    }

    /**
    *set the game name
    */
    public void setName(string gameName)
    {
        this.gameName = gameName;
    }

    /**
    *set the host name
    */
    public void setHost(string host)
    {
        this.host = host;
    }

    /**
    *set the build time
    */
    public void setBuildTime(int buildTime)
    {
        this.buildTime = buildTime;
    }

    /**
    *set the build time limit
    */
    public void setBuildLimit(int buildLimit)
    {
        this.buildLimit = buildLimit;
    }

    /**
    *set the keywords to given array of strings
    */
    public void setKeys(string[] keys)
    {
        keywords = new string[keys.Length];

        for(int i = 0; i < keys.Length; i++)
        {
            keywords[i] = keys[i];
        }
    }

    /**
    *set the password for this game session
    */
    public void setPass(string pass)
    {
        password = pass;
    }

    /**
    *sets whether this session is a host session
    */
    public void establishHost(bool isHost)
    {
        this.isHost = isHost;
    }

    /**
    *add the given keyword to array of keywords
    */
    public void addKey(string key)
    {
        //maximum number of keys changed
        if(keywords.Length == keySize)
        {
            keySize = keySize + keySize;
            string[] keys = new string[keySize];
            keywords.CopyTo(keys,0);
            keywords = keys;
        }

        keywords[keywords.Length] = key;

    }

    /**
    *get the game name
    */
    public string getName()
    {
        return gameName;
    }

    /**
    *get the host name
    */
    public string getHost()
    {
        return host;
    }

    /**
    *get the games build limit
    */
    public int getBuildLimit()
    {
        return buildLimit;
    }

    /**
    *get the game build time
    */
    public int getBuildTime()
    {
        return buildTime;
    }

    /**
    *get the password for this game session
    */
    public string getPass()
    {
        return password;
    }

    /**
    *returns true if session is for a host false otherwise
    */
    public bool checkHost()
    {
        return isHost;
    }

    /**
    *reset the information for playable game
    */
    public void reset()
    {
        gameName = null;
        buildTime = 120;
        buildLimit = 30;
        host = null;
        keywords = null;
        password = null;
    }

    /**
    *get a deep copy of all the keywords describing the game.
    */
    public string[] getKeyWords()
    {
        string[] copy = new string[keywords.Length];
        keywords.CopyTo(copy, 0);
        return copy;
    }
}
