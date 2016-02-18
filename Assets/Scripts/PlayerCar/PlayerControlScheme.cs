/* David Robideau
 * 2/18/2016
 * 
 * This class allows the player to define their own control scheme.
 *
 * Last update - 2/18/2016
 */
using UnityEngine;
using System.Collections;

public class PlayerControlScheme : MonoBehaviour {

	public UnityEngine.UI.Text controlsDisplay;

	public string accelerate;
	public string decelerate;
	public string turnRight;
	public string turnLeft;
	public string shortReset;
	public string fullReset;
	public string cameraControl;
	public string replayGhost;

	//Alter controlsDisplay text to reflect player-selected controls
	public void displayControls() {
		if (controlsDisplay != null) {
			string controlsText = string.Format ("Controls: \n" +
			                      "Accel. - {0}\n" +
			                      "Decel. - {1}\n" +
			                      "Turn Right - {2}\n" +
			                      "Turn Left - {3}\n" +
			                      "Short reset - {4}\n" +
			                      "Full reset - {5}\n" +
			                      "Camera - {6}\n" +
			                      "Ghost replay - {7}\n",
				                      accelerate, decelerate, turnRight, turnLeft, shortReset, fullReset, cameraControl, replayGhost);
			controlsDisplay.text = controlsText;
		}
	}

}
