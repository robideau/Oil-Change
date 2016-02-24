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
	public Rect textBox = new Rect(0,100,250,250);

	public void Start() {
		chatLog = new List<string> ();
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
		if (GUILayout.Button("Send")) {
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
