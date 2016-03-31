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
		}

		SceneManager.LoadScene("main screens");
		//Bypass login screen somehow?
	}

	[RPC] void notifyOpponentOfQuit() {
		opponentStatusText.text = "Opponent has left match.";
		opponentStatusText.color = Color.yellow;
	}

}
