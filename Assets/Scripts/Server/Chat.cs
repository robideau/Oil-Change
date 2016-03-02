#pragma warning disable 618

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/**
 * created by Nick Bramanti
 */


public class Chat : MonoBehaviour {

	public Text chat;

	private string current = string.Empty;
	public List<string> chatLog;
	public Rect textBox = new Rect(Screen.width,Screen.height,250,250);
	public Rect textField = new Rect (100, 100, 100, 100);
	public bool sendMessage = false;
	public Event e;
	public NetworkView nView;
	public InputField chatField;


	public void Start() {
		chatLog = new List<string> ();
		chat = GetComponent<Text> ();
		chatField = GetComponent<InputField> ();
		nView = GetComponent<NetworkView> ();
	}
	private void OnGUI () {
		if (!NetworkMenu.connected) {
			return;
		}
		GUILayout.BeginArea(textBox);
		current = GUILayout.TextField (current);
		e = Event.current;
		if(e.keyCode == KeyCode.Return)	 {
			sendMessage = true;
		}

		//TODO: open chat system when t is pressed

//		if(e.keyCode == KeyCode.T) {
//			if (chatField == null) {
//				chatField.
//			}
//		}
		if (sendMessage) {
			sendMessage = false;
			//don't send an empty message
			if (!string.IsNullOrEmpty (current.Trim ())) {
				//concatenate with username maybe?
				nView.RPC("ChatMessage", RPCMode.All, current);
				current = string.Empty;
			}
		}
		GUILayout.EndArea();
	}

//	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
//		string chatText = current;
//		int text = chatText.
//			if (stream.isWriting) {
//				chatText = Int32.Parse(chat.text);
//				stream.Serialize (ref chatText);
//				print (chatText);
//			} else {
//				stream.Serialize (ref chatText);
//				ChatMessage (chatText.ToString());
//			}
//	}

	[RPC]
	public void ChatMessage (string message) {
		chat.text = message;
		chatLog.Add (message);
	}

}
