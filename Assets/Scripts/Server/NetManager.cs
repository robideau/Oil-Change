/* David Robideau
 * 3/1/2016
 * 
 * This script handles peer-to-peer server initialization, connection, and synchronization during gameplay.
 * Hosting is handled using the Unity MasterServer - by default hosted by Unity, but can be hosted locally
 *
 * Last update - 3/21/2016
 */

using UnityEngine;
using System.Collections;

public class NetManager : MonoBehaviour {

	//Game tracker - synchronizes player data with server as needed
	public GameObject GameTracker;

	//Game name and room name - room name can be modified by host player in game settings
	private const string typeName = "OilChangeSession";
	public string gameName = "DefaultRoom";

	//Server listening port - default 7531, can be modified by host player in game settings
	public int listeningPort;
	public string masterServerIP;
	public int masterServerPort;
	public bool useLocalMasterServer;

	//Hostlist - used to connect to specific server
	private HostData[] hostList;

	//First routine called by server
	private void StartServer() {
		Network.natFacilitatorIP = masterServerIP;
		Network.natFacilitatorPort = 50005;
		Network.InitializeServer (2, listeningPort, true); //Initialize server - max. 2 connections, use NAT if public address
		if (useLocalMasterServer) {
			MasterServer.ipAddress = masterServerIP;
		}
		MasterServer.port = masterServerPort;
		MasterServer.RegisterHost(typeName, gameName);

		//Console log - for debug
		print ("Server started on listening port " + listeningPort);
		print ("Registered on Master Server as room " + gameName + " of type " + typeName);
		print ("Master Server port " + masterServerPort);
		if (useLocalMasterServer) {
			print ("Master Server IP " + masterServerIP);
		}
	}

	//Executed immediately after StartServer()
	void OnServerInitialized() {
		print ("Server has been initialized on port " + listeningPort);
	}

	//Displays network GUI - for debug only, player will not see this

	void OnGUI() {
		if (!Network.isClient && !Network.isServer) { //If not currently connected to a server
			if (GUI.Button (new Rect (100, 100, 200, 100), "Run StartServer()")) {
				StartServer ();
			}
			if (GUI.Button (new Rect (100, 200, 200, 100), "Run RefreshHostList()")) {
				RefreshHostList ();
			}
			if (hostList != null) {
				if (GUI.Button (new Rect (100, 300, 200, 100), hostList [0].gameName)) {
					JoinServer (hostList [0]);
				}
			}
		}
	}

	//Request list of Oil Change sessions available to join
	void RefreshHostList() {
		MasterServer.RequestHostList (typeName); //Request all Oil Change sessions from master server
												 //NOTE - no reason master server should ever host anything other than Oil Change
	}

	//Triggered on MasterServerEvent - for example, HostListReceived
	void OnMasterServerEvent(MasterServerEvent e) {
		if (e == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList ();
		}
	}

	//Join a server using specified host data
	void JoinServer(HostData hd) {
		Network.Connect (hd);
	}

	//Called automatically upon connection
	void OnConnectedToServer() {
		print ("Server joined.");
		GameTracker.SetActive (true);
	}

	void OnPlayerConnected() {
		print ("Player joined your server.");
		GameTracker.SetActive (true);
	}

	//Called on destruction of NetworkManager object - disconnect, clear ports, etc.
	void OnDestroy() {
		Network.Disconnect();
		MasterServer.ClearHostList ();
	}
}