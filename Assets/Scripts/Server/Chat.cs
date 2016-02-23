#pragma warning disable 618

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;



public class Chat : MonoBehaviour {

	public Text lastChat;

	private string current = string.Empty;
	NetworkView nView;
	public List<string> chatLog;

	public void Start() {
		chatLog = new List<string> ();
	}

	private void OnGUI () {
		if (!NetworkMenu.connected) {
			return;
		}
		GUILayout.BeginHorizontal(GUILayout.Width (250));
		current = GUILayout.TextField (current);
		if (GUILayout.Button("Send")) {
			//don't send an empty message
			if (!string.IsNullOrEmpty (current.Trim ())) {
				ChatMessage (current);
				current = string.Empty;
			}
		}
		GUILayout.EndHorizontal();
		foreach (string x in chatLog) {
			GUILayout.Label (x);
		}
	}

	[RPC]
	public void ChatMessage (string message) {
		lastChat.text = message;
		chatLog.Add (message);
	}
}
