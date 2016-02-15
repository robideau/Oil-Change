using UnityEngine;
using System.Collections.Generic;

public class Chat : MonoBehaviour {

	public List<string> chatLog = new List<string>();
	private string current = string.Empty;
	private void OnGUI () {
		GUILayout.BeginHorizontal(GUILayout.Width (250));
		current = GUILayout.TextField (current);
		if (GUILayout.Button ("Send")) {
			//TODO
		}
		GUILayout.EndHorizontal ();
		foreach (string x in chatLog) {
			GUILayout.Label (x);
		}
	}
}
