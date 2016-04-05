/*
 * Written by Nick Bramanti 
 * 3/29/16
 * 
 * This script will be used to update the configuration file for settings
 */
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class writeSettings : MonoBehaviour {

	public string filePath = "settings.txt";
	public FileStream file;
	public string displayText = "Option A";
	public string resolutionText = "Option A";
	public string qualityText = "Option A";
	public string audioText = "Option A";
	public Dropdown displayDrop;
	public Dropdown resolutionDrop;
	public Dropdown qualityDrop;
	public Dropdown audioDrop;

	void Start() {
		//new FileStream with OpenOrCreate mode which opens the file if it is there
		//or creates the file if it is not
		file = new FileStream (filePath, FileMode.OpenOrCreate);
	}

	//a function that will be called when display is changed
	public void updateDisplay() {
		switch (displayDrop.value) {
			case 0:
				displayText = "Option A";
				break;
			case 1:
				displayText = "Option B";
				break;
			case 2:
				displayText = "Option C";
				break;
		}
		updateFile (0);
	}

	//a function that will be called when resolution is changed
	public void updateResolution() {
		switch (resolutionDrop.value) {
		case 0:
			resolutionText = "Option A";
			break;
		case 1:
			resolutionText = "Option B";
			break;
		case 2:
			resolutionText = "Option C";
			break;
		}
		updateFile (1);
	}

	//a function that will be called when quality is changed
	public void updateQuality() {
		switch (qualityDrop.value) {
		case 0:
			qualityText = "Option A";
			break;
		case 1:
			qualityText = "Option B";
			break;
		case 2:
			qualityText = "Option C";
			break;
		}
		updateFile (2);
	}

	//a function that will be called when audio is changed
	public void updateAudio() {
		switch (audioDrop.value) {
		case 0:
			audioText = "Option A";
			break;
		case 1:
			audioText = "Option B";
			break;
		case 2:
			audioText = "Option C";
			break;
		}
		updateFile (3);
	}

	//called after each change to update the settings file
	public void updateFile (int i) {
		
	}
}
