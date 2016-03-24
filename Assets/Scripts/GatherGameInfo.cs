using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

/**
created by Ryan Young

*/

public class GatherGameInfo : MonoBehaviour {

    //information to gather from
    public Dropdown timeLimit;
    public Dropdown placeLimit;
    public InputField gameName;
    public Text keys;
    public accountInfo player;
	public GameObject sessionData;
    public InputField pass;
    public Toggle pCheck;
    public Text warning;

    public GameObject curScene;
    public GameObject nextScene;

    //      these will be needed for setting up menu for other players
    //GameObject prefabclone;
    //public GameObject prefabParent;

    //this is part of the solution but is good for appearence sake for now
    public void gatherInfo_addToList()
    {

        //todo--
        //  add the invite only option
        //  extend for passwords
        //  once host names are figured out change the gameobject name to host's Game
        //todo--

        //          this will be needed for setting up the menu for other players
        //prefabclone =Instantiate(prefab);
        //prefabclone.transform.SetParent(prefabParent.transform);
        //playableGame reference = prefabclone.GetComponent<playableGame>();
        //reference.setBuildLimit(Int32.Parse(placeLimit.captionText.text));
        //reference.setBuildTime(Int32.Parse(minTime)*60);
        //reference.setName(gameName.text);
        //reference.setKeys(keycollection);
        //reference.setHost (player.getName ());
        if(gameName.text.Length > 20)
        {
            warning.text = "game name is too long";
            return;
        }
        if(pass.text.Length > 20)
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
        //set the name for this game
        
		hostGame.setName (gameName.text);
        //break they keys text down into an array that will be deep copied into the playable game
        string keysToAdd = keys.text;
        char[] delim = { ',', ' ', '\n' };
        //string[] keycollection = keysToAdd.Split(delim);
        

		hostGame.setHost (player.getName ());

        hostGame.setPass(pass.text);

        //          this will be needed for manage playable game update the prefabs text to clearly display the game information
        //prefabclone.name = "game by " + player.getName() ;
        //updateText updates = prefabclone.GetComponent<updateText>();
        //updates.updateLarge(gameName.text,player.getName(),keycollection,0, placeLimit.captionText.text, timeLimit.captionText.text);

        //reset the create match screen details
        keys.text = "";
        
        if (pCheck.isOn && !pass.text.Equals(""))
        {
            StartCoroutine(submit_match(hostGame, hostGame.getHost(), hostGame.getName(), hostGame.getBuildTime().ToString(), hostGame.getBuildLimit().ToString(),keysToAdd, hostGame.getPass(), "TRUE"));
        }
        else
        {
            StartCoroutine(submit_match(hostGame, hostGame.getHost(), hostGame.getName(), hostGame.getBuildTime().ToString(), hostGame.getBuildLimit().ToString(),keysToAdd, hostGame.getPass(), "FALSE"));
        }
            
        
    }

    IEnumerator submit_match(playableGame g, string hostName, string matchName, string b_time, string b_limit, string keywords, string password,string e_only)
    {
        string url = "http://proj-309-38.cs.iastate.edu/php/creatematch.php?" + "sessionName=" + matchName + "&hostUser=" + hostName + "&buildTime=";
        url = url + b_time + "&buildLimit=" + b_limit + "&keyword=" + keywords + "&pass=" + password + "&inviteOnly=" + e_only;
        WWW g_submit = new WWW(url);
        yield return g_submit;
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
        else if(g_submit.text.Equals("sessionexists"))
        {
            g.reset();
            gameName.text = "";
            warning.text = "game session name already exists";
            
        }
        else
        {
            g.reset();
            warning.text = g_submit.text;
        }

    }


}
