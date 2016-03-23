/* David Robideau
 * 3/22/2016
 * 
 * This script handles transitions between build mode and race mode.
 * (De)activates components as necessary, detects transition criteria, and handles data transfers.
 *
 * Last update - 3/22/2016
 */
using UnityEngine;
using System.Collections;

public class TransitionHandler : MonoBehaviour {

	//Empties - hold objects exclusive to either build or race mode for easy deactivation
	public GameObject buildModeComponents;
	public GameObject raceModeComponents;

	//Network manager and data to handle initial connection
	public NetManager netManager;
	public GameObject connectionData;

	//Track scanner and replicator
	public TrackScanner scanner;
	public TrackReplicator replicator;

	//Track data sender
	public SendData dataSender;

	//Canvas items
	public ModularChat chat;
	public GameObject timer;

	//Track data to be sent between players
	private string trackData;

	void Awake() {
		//Retrieve network data, create connection

		//Transition to build mode
		//buildMode ();
	}

	private void buildMode() {
		//Wait for players to finish

		//Scan track, send data
		scanner.scanLevelData();
		scanner.cleanObjectNames ();
		trackData = scanner.getScannedLevelData ();

		//Deactivate build mode components
		buildModeComponents.SetActive(false);

		//Transition to race mode
		raceMode();
	}

	private void raceMode() {
		//Relocate car to start point

		//Replicate track
		replicator.replicateTrack(trackData);

		//Reset timer

		//Collapse chat
		chat.ChatUI.SetActive(false);

		//Wait for players to finish

		//Determine scores and send to final screen
	}
}
