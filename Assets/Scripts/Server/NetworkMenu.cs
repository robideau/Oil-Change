using UnityEngine;
using System.Collections;

/**
 * created by Nick Bramanti
 */

public class NetworkMenu : MonoBehaviour {
	//current IP connected to
	public string connectionIP = "127.0.0.1";
	//port number that should be open
	public int portNumber = 7531;
	//connected to the server
	public static bool connected { get; private set; }

	//a client has just connected to our server
	private void OnConnectedToServer() {
		connected = true;
	}
	//the server has been initialized
	private void OnServerInitialized() {
		connected = true;
	}
		

	private void OnGUI() {

		if (!connected) {

			//TODO: 

			//create a new label giving the number of connections 
			GUILayout.Label ("Connections: " + Network.connections.Length);
			//print out the connection IP and the portNumber
			connectionIP = GUILayout.TextField (connectionIP);
			//need to do this so that it can be printed as a string but still give it's value as an integer
			int.TryParse(GUILayout.TextField(portNumber.ToString()), out portNumber);

			if (GUILayout.Button ("Connect")) {
				Network.Connect (connectionIP, portNumber);
			}

			if (GUILayout.Button ("Host")) {
				Network.InitializeServer (2, portNumber,false); 
			}
		} else {
			GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
		}
	}
	//disconnected from the server or the connection was lost
	private void OnDisconnectedFromServer() {
		connected = false;
	}
}
