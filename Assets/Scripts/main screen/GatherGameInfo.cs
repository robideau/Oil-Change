using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

/**
created by Ryan Young
    last modified 3/25/16
    used to gather information from create match screen and update database based on this information
*/
public class GatherGameInfo : MonoBehaviour {

    //information to gather from
    //game time limit
    public Dropdown timeLimit;
    //game place limit
    public Dropdown placeLimit;
    //session name
    public InputField gameName;
    //keywords for the session
    public Text keys;
    //current player account information for given session
    public accountInfo player;
    //holds details about the match being made
	public GameObject sessionData;
    //password for the match created
    public InputField pass;
    //check if passwords are enabled
    public Toggle pCheck;
    //warning to indicate possible problems
    public Text warning;

    //used for scene transition from match creation screen to matchmaking screen
    public GameObject curScene;
    public GameObject nextScene;


    //this is part of the solution but is good for appearence sake for now
    public void gatherInfo_addToList()
    {

        //todo--
        //  add the invite only option
        //todo--


        //check that the game Name and password are not too long
        if(gameName.text.Length > 20)
        {
            warning.text = "game name is too long";
            return;
        }
        if(gameName.text.Contains(" ") || gameName.text.Contains("&") || gameName.text.Contains(",") || gameName.text.Contains(";") || gameName.text.Contains("\t") || gameName.text.Contains("\n"))
        {
            warning.text = "game name cannot contain characters: space, '&', ',', ';', tab, or return";
            return;
        }
        if (pass.text.Contains(" ") || pass.text.Contains("&") || pass.text.Contains(",") || pass.text.Contains(";") || pass.text.Contains("\t") || pass.text.Contains("\n"))
        {
            warning.text = "password cannot contain characters: space, '&', ',', ';', tab, or return";
            return;
        }
        if (pass.text.Length > 20)
        {
            warning.text = "password is too long";
            return;
        }

        //game this is being set
        playableGame hostGame = sessionData.GetComponent<playableGame> ();
        //set the BuildLimit for this playableGame
		hostGame.setBuildLimit(Int32.Parse(placeLimit.captionText.text));
        //set the play time for this playableGame in seconds
        string minTime = Regex.Match(timeLimit.captionText.text, @"\d+").Value;
		hostGame.setBuildTime(Int32.Parse(minTime)*60);
        //set the game Name
		hostGame.setName (gameName.text);
        //set the host Name
		hostGame.setHost (player.getName ());
        //set the password empty if none
        hostGame.setPass(pass.text);
        Debug.Log("!" + pass.text + "!");
        //establish user as host
        hostGame.establishHost(true);

        
        //add game session to database with a password confirmation attatched
        if (pCheck.isOn && !pass.text.Equals(""))
        {
            StartCoroutine(submit_match(hostGame, hostGame.getHost(), hostGame.getName(), hostGame.getBuildTime().ToString(), hostGame.getBuildLimit().ToString(),keys.text, hostGame.getPass(), "TRUE"));
        }
        else
        {
            StartCoroutine(submit_match(hostGame, hostGame.getHost(), hostGame.getName(), hostGame.getBuildTime().ToString(), hostGame.getBuildLimit().ToString(),keys.text, hostGame.getPass(), "FALSE"));
        }
            
        
    }

    /**
    *adds the game session to the database
    */
    IEnumerator submit_match(playableGame g, string hostName, string matchName, string b_time, string b_limit, string keywords, string password,string e_only)
    {
        string keyswaped = keywords.Replace('\n', ',');
        keyswaped = keyswaped.Replace(' ', ',');
        Debug.Log(keyswaped);
        //url with information attatched
        string url = "http://proj-309-38.cs.iastate.edu/php/creatematch.php?" + "sessionName=" + matchName + "&hostUser=" + hostName + "&buildTime=";
        url = url + b_time + "&buildLimit=" + b_limit + "&keywords=" + keyswaped + "&pass=" + password + "&inviteOnly=" + e_only;
        Debug.Log(url);
        //wait for confirmation
        WWW g_submit = new WWW(url);
        yield return g_submit;

        //on success reset the fields on the create game menu, transition scene back to matchmaking and move to buildmode for waiting
        if (g_submit.text.Equals("success"))
        {

            gameName.text = "";
            pass.text = "";
            keys.text = "";

            //all screen transitions here
            DontDestroyOnLoad(sessionData);
            curScene.SetActive(false);
            nextScene.SetActive(true);
            SceneManager.LoadScene("buildScreen");
        }
        //session name already exists indicate this in menu and do nothing else
        else if(g_submit.text.Equals("sessionexists"))
        {
            g.reset();
            gameName.text = "";
            warning.text = "game session name already exists";
            
        }
        //some other error print to menu
        else
        {
            g.reset();
            warning.text = g_submit.text;
        }

    }


}
