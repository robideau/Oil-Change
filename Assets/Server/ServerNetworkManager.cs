using UnityEngine;
using System.Collections;

public class ServerNetworkManager : MonoBehaviour {

	//the maximum number of clients allowed on the server
	public int maxClients;
	//port number to be listening to
	public int listenPort;
	//if Network Address Translation is on or not
	//need to read up on NAT and NAT algorithm
	//allows for mapping multiple addresses to a single destination router
	public bool NATon;

	void startServer() {
		Network.InitializeServer(maxClients, listenPort, NATon);
	}

	void stopServer() {
		Network.Disconnect;
	}

	void Update() {
		if (this.serverStarted) {
			if (Network.peerType == NetworkPeerType.Connecting) {
				//message stating that it is connecting
				//spinning bar/loading icon?
			} else {
				//message stating that the players are connected to server
			}
		}
	}
}
