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
using System;
using System.Text;

public class writeSettings : MonoBehaviour {

	public string filePath = "settings.txt";
	public FileStream file;
	public string displayText = "Full Screen";
	public string resolutionText = "1920*1080";
	public string qualityText = "Low";
	public string audioText = "";
	public Dropdown displayDrop;
	public Dropdown resolutionDrop;
	public Dropdown qualityDrop;
	public Dropdown audioDrop;
	public bool fullScreen = false;

	void Start() {
		//check if file exists
		if(File.Exists("settings.txt")) {
			//file exists so read it and write their settings
			string text = File.ReadAllText("settings.txt");
			Char[] delimiter = {',','\n'};
			//split the string into comma separated values
			//there should be 12 values in the order
			//Type,Number,String,
			String[] strings = text.Split(delimiter);
			displayDrop.value = Int32.Parse (strings [1]);
			resolutionDrop.value = Int32.Parse (strings [4]);
			qualityDrop.value = Int32.Parse (strings [7]);
			audioDrop.value = Int32.Parse (strings [10]);

		} else {
			//make new file and set to default
			displayDrop.value = 0;
			resolutionDrop.value = 0;
			qualityDrop.value = 0;
			audioDrop.value = 0;
			updateFile ();
		}
	}

	//a function that will be called when display is changed
	public void updateDisplay() {
		switch (displayDrop.value) {
		case 0:
			displayText = "0,Full Screen";
			Screen.fullScreen = true;
			break;
		case 1:
			displayText = "1,Windowed";
			Screen.fullScreen = false;
			break;
		}
	}

	//a function that will be called when resolution is changed
	public void updateResolution() {
		switch (resolutionDrop.value) {
		case 0:
			resolutionText = "0,1280*720";
			Screen.SetResolution (1920, 1080, fullScreen, 0);
			break;
		case 1:
			resolutionText = "1,1280*800";
			Screen.SetResolution (1280, 800, fullScreen, 0);
			break;
		case 2:
			resolutionText = "2,1280*960";
			Screen.SetResolution (1280, 960, fullScreen, 0);
			break;
		}
	}

	//a function that will be called when quality is changed
	public void updateQuality() {
		switch (qualityDrop.value) {
		case 0:
			qualityText = "0,Low";
			QualitySettings.SetQualityLevel (0);
			break;
		case 1:
			qualityText = "1,Medium";
			QualitySettings.SetQualityLevel (3);
			break;
		case 2:
			qualityText = "2,High";
			QualitySettings.SetQualityLevel (5);
			break;
		}
	}

	//a function that will be called when audio is changed
	public void updateAudio() {
		switch (audioDrop.value) {
		case 0:
			audioText = "0,Off";
			AudioListener.volume = 0;
			break;
		case 1:
			audioText = "1,Low";
			AudioListener.volume = 0.25F;
			break;
		case 2:
			audioText = "2,Medium";
			AudioListener.volume = 0.5F;
			break;
		case 3:
			audioText = "3,High";
			AudioListener.volume = 0.75F;
			break;
		case 4:
			audioText = "4,Full";
			AudioListener.volume = 1;
			break;
		}
	}

	//called after exiting to update the settings file
	public void updateFile () {
		Debug.Log ("updateFile called");
		//make the string which is 
		string s = "Display," + displayText + "\n";
		s += "Resolution," + resolutionText + "\n";
		s += "Quality," + qualityText + "\n";
		s += "Audio," + audioText;
		Debug.Log (s);
		File.WriteAllText ("settings.txt", s);
	}
}
