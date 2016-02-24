#pragma warning disable 618

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;



public class Chat : MonoBehaviour {

	public Text chat;

	private string current = string.Empty;
	NetworkView nView;
	public List<string> chatLog;
	public Rect textBox = new Rect(Screen.width,Screen.height,250,250);
	public bool sendMessage = false;
	public bool openChat = false;

	public void Start() {
		chatLog = new List<string> ();
	}

	public void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			sendMessage = true;
		}
		if(Input.GetKeyDown(KeyCode.C) {

		}
	}

	private void Awake() {
		chat = GetComponent<Text> ();
	}

	private void OnGUI () {
		if (!NetworkMenu.connected) {
			return;
		}
		GUILayout.BeginArea(textBox);
		current = GUILayout.TextField (current);
		if (sendMessage) {
			sendMessage = false;
			//don't send an empty message
			if (!string.IsNullOrEmpty (current.Trim ())) {
				ChatMessage (current);
				current = string.Empty;
			}
		}
		GUILayout.EndArea();
	}

	public void ChatMessage (string message) {
		chat.text = message;
		chatLog.Add (message);
	}
}
