﻿/*
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
		//check for file if its there read and write in settings or else create the file
	}

	//a function that will be called when display is changed
	public void updateDisplay() {
		switch (displayDrop.value) {
		case 0:
			displayText = "Full Screen";
			fullScreen = true;
			break;
		case 1:
			displayText = "Windowed";
			Screen.fullScreen = false;
			break;
		}
	}

	//a function that will be called when resolution is changed
	public void updateResolution() {
		switch (resolutionDrop.value) {
		case 0:
			resolutionText = "1920*1080";
			Screen.SetResolution (1920, 1080, fullScreen, 0);
			break;
		case 1:
			resolutionText = "1920*1200";
			Screen.SetResolution (1920, 1200, fullScreen, 0);
			break;
		case 2:
			resolutionText = "1600*1200";
			Screen.SetResolution (1600, 1200, fullScreen, 0);
			break;
		}
	}

	//a function that will be called when quality is changed
	public void updateQuality() {
		switch (qualityDrop.value) {
		case 0:
			qualityText = "Low";
			QualitySettings.SetQualityLevel (0);
			break;
		case 1:
			qualityText = "Medium";
			QualitySettings.SetQualityLevel (3);
			break;
		case 2:
			qualityText = "High";
			QualitySettings.SetQualityLevel (5);
			break;
		}
	}

	//a function that will be called when audio is changed
	public void updateAudio() {
		switch (audioDrop.value) {
		case 0:
			audioText = "Off";
			AudioListener.volume = 0;
			break;
		case 1:
			audioText = "Low";
			AudioListener.volume = 0.25F;
			break;
		case 2:
			audioText = "Medium";
			AudioListener.volume = 0.5F;
			break;
		case 3:
			audioText = "High";
			AudioListener.volume = 0.75F;
			break;
		case 4:
			audioText = "Full";
			AudioListener.volume = 1;
			break;
		}
	}

	//called after exiting to update the settings file
	public void updateFile () {
		Debug.Log ("updateFile called");
		//make the string which is 
		string s = "Display: " + displayText + "\n";
		s += "Resolution: " + resolutionText + "\n";
		s += "Quality: " + qualityText + "\n";
		s += "Audio: " + audioText;
		Debug.Log (s);
		File.WriteAllText ("settings.txt", s);
	}
}
