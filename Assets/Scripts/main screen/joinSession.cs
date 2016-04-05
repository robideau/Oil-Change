using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/**
*created by Ryan Young
    last modified 3/27/16
*/
public class joinSession : MonoBehaviour {

    public playableGame prefabGame;
    public InputField passField;

    public Text warning;

    private bool verifying = false;
    private bool stillAvailable = false;

    //try to join the given prefab game session
    public void joinGame()
    {
        warning.gameObject.SetActive(false);
        //if these is a password verify correct input
        if (passField.interactable)
        {
            string pass = passField.text;
            if (!pass.Equals(prefabGame.getPass()))
            {
                warning.gameObject.SetActive(true);
                warning.text = "wrong password";
                
                return;
            }
        }
        GameObject sessionData = GameObject.Find("SessionData");


        //start double check
        StartCoroutine(doubleCheckGame());
        //wait for doublecheck to finish and join session if available
        StartCoroutine(SessionTransfer(sessionData));
       
        
    }

    //switch to build mode and make coneection on success
    private IEnumerator SessionTransfer(GameObject sessionData)
    {
        //wait for verification that match is still available
        while (verifying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //if still avaialable join the game
        if (stillAvailable)
        {
            playableGame playersGame = sessionData.GetComponent<playableGame>();
            playersGame.setBuildLimit(prefabGame.getBuildLimit());
            playersGame.setBuildTime(prefabGame.getBuildTime());
            playersGame.setHost(prefabGame.getHost());
            playersGame.setName(prefabGame.getName());
            playersGame.establishHost(false);
            DontDestroyOnLoad(sessionData);
            SceneManager.LoadScene("buildScreen");
        }

    }

    //double check database and remove game from database on successful game session connection
    private IEnumerator doubleCheckGame()
    {
        verifying = true;
        string url = "http://proj-309-38.cs.iastate.edu/php/joinmatch.php?" + "sessionName=" + prefabGame.getName(); 
        WWW g_list = new WWW(url);
        yield return g_list;
        if (g_list.text.Equals("success"))
        {
            stillAvailable = true;
        }
        else
        {
            warning.gameObject.SetActive(true);
            warning.text = g_list.text;
            
        }


        verifying = false;
    }
}
