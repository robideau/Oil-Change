using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
*created by Ryan Young\
    last modified 3/31/16
*used with find match button to find a match and join it. if not matches found will try to add a new default match
*/
public class findMatchAutomatically : MonoBehaviour {

    public childHolder gameAvailable;
    public GameObject sessionData;
    public managePlayableGameDisplay updateList;
    public accountInfo player;
    public Text warning;

    private bool verifyingExistance = false;
    private bool stillAvailable = false;
    private bool tryingToJoin = false;
    private bool matchFound = false;

	public void findAndJoinGame()
    {
        updateList.upDateGames();
        StartCoroutine(getMatch());
    }

    private IEnumerator getMatch()
    {
        //wait for 1 second to load in most updated current games list
        yield return new WaitForSeconds(1);
        //obtain reference to available games list
        List<GameObject> joinableList = gameAvailable.getChildList();
        //try to join each game until successful join (avoid games with passwords)
        foreach (GameObject game in joinableList)
        {
            if (game.GetComponent<InputField>() != null)
            {
                //password needed skip
                if (game.GetComponent<InputField>().interactable)
                {
                    Debug.Log("password found");
                    continue;
                }
                else
                {
                    playableGame prefabGame = game.GetComponent<playableGame>();
                    //start double check
                    StartCoroutine(doubleCheckGame(prefabGame));
                    //wait for doublecheck to finish and join session if available
                    StartCoroutine(SessionTransfer(sessionData, prefabGame));

                    //wait for session transfer to complete
                    while (tryingToJoin)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }

                    //if game Session found return since player is joining a match
                    if (matchFound)
                    {
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("null field");
            }
        }

        if (!matchFound)
        {
            //if no game was found create a new game with default settings
            StartCoroutine(add_Default());
        }
    }

    /**
    *adds the game session to the database
    */
    IEnumerator add_Default()
    {
        //set up default game information
        playableGame hostGame = sessionData.GetComponent<playableGame>();
        hostGame.setBuildLimit(50);
        hostGame.setBuildTime(120);
        hostGame.setHost(player.getName());
        hostGame.establishHost(true);
        hostGame.setPass("");

        //keep trying to add the session until sucess or notified of some problem
        bool failed = true;
        int failCount = 0;
        while (failed)
        {
            //url with information attatched
            string url = "http://proj-309-38.cs.iastate.edu/php/creatematch.php?" + "sessionName=" + player.getName() + "DEF" + failCount + "&hostUser=" + player.getName() + "&buildTime=";
            url = url + 120 + "&buildLimit=" + 50 + "&keywords=default" + "&pass=" + "&inviteOnly=" + "FALSE";
            //update gameName
            hostGame.setName(player.getName() + "DEF" + failCount);
            Debug.Log(url);
            //wait for confirmation
            WWW g_submit = new WWW(url);
            yield return g_submit;

            //on success reset the fields on the create game menu, transition scene back to matchmaking and move to buildmode for waiting
            if (g_submit.text.Equals("success"))
            {
                //all screen transitions here
                DontDestroyOnLoad(sessionData);
                failed = false;
                SceneManager.LoadScene("buildScreen");
            }
            //session name alread exists indicate this in menu and do nothing else
            else if (g_submit.text.Equals("sessionexists"))
            {
                failCount++;

            }
            //some other error print to menu
            else
            {
                warning.text = "server problem";
                break;
            }
        }
    }

    //switch to build mode and make coneection on success
    private IEnumerator SessionTransfer(GameObject sessionData, playableGame prefabGame)
    {
        tryingToJoin = true;
        //wait for verification that match is still available
        while (verifyingExistance)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //if still avaialable join the game
        if (stillAvailable)
        {
            matchFound = true;
            playableGame playersGame = sessionData.GetComponent<playableGame>();
            playersGame.setBuildLimit(prefabGame.getBuildLimit());
            playersGame.setBuildTime(prefabGame.getBuildTime());
            playersGame.setHost(prefabGame.getHost());
            playersGame.setName(prefabGame.getName());
            playersGame.establishHost(false);
            DontDestroyOnLoad(sessionData);
            SceneManager.LoadScene("buildScreen");
        }
        tryingToJoin = false;
    }

    //double check database and remove game from database on successful game session connection
    private IEnumerator doubleCheckGame(playableGame prefabGame)
    {
        verifyingExistance = true;
        string url = "http://proj-309-38.cs.iastate.edu/php/joinmatch.php?" + "sessionName=" + prefabGame.getName();
        WWW g_list = new WWW(url);
        yield return g_list;
        if (g_list.text.Equals("success"))
        {
            stillAvailable = true;
        }
        verifyingExistance = false;
    }
}
