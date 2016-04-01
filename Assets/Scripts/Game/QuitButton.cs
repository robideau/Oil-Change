/* David Robideau
 * 3/31/2016
 * 
 * This script describes the functionality of the quit button and handles all necessary database operations.
 *
 * Last update - 3/31/2016
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuitButton : MonoBehaviour {

	public NetworkView nv;
	public NetManager netManager;
	public Text opponentStatusText;
	public TransitionHandler handler;
	private playableGame game;

	void Awake() {
		game = GameObject.Find ("SessionData").GetComponent<playableGame> ();
	}

	public void OnClick() {
		nv.RPC ("notifyOpponentOfQuit", RPCMode.All, null);

		//If host quits and nobody has connected yet
		if (game.checkHost() && !handler.playerConnected) {
            //Remove session from database here
            StartCoroutine(removeGame());
		}
        //change these names to bypass login screen on main menu screen
        GameObject.Find("SessionData").name = "SessionStillPlaying";
        GameObject.Find("Script manager").name = "managerDuplicated";
		SceneManager.LoadScene("main screens");
	}

	[RPC] void notifyOpponentOfQuit() {
		opponentStatusText.text = "Opponent has left match.";
		opponentStatusText.color = Color.yellow;
	}


    private IEnumerator removeGame()
    {
        string url = "http://proj-309-38.cs.iastate.edu/php/joinmatch.php?" + "sessionName=" + game.getName();
        WWW g_list = new WWW(url);
        yield return g_list;

    }
}
