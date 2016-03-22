﻿/* David Robideau
 * 3/9/2016
 * 
 * This script handles the processing and storage of build mode data.
 * Returns track data as a string -
 * 		Each object has 4 lines of data:
 * 				1: name
 * 				2: position
 * 				3: scale
 * 				4: rotation
 *
 * Last update - 3/21/2016
 */

using UnityEngine;
using System.Collections;

public class TrackScanner : MonoBehaviour {

	//Output filename
	public string outputFile;

	//Number of spaces in the grid - one space = one block
	public int gridXSpaces;
	public int gridYSpaces;
	public int gridZSpaces;
	private float startX;
	private float startZ;

	//Lowest possible value for y, height of each y space
	public int baseY;
	public int yIncrements;

	//References to world grid and controllers
	public GameObject worldGrid;
	private BoxCollider gridCollider;
	public ObjectController objectController;
	private float gridBlockSize;

	//Whether or not trackscanner is currently scanning
	private bool scanning = false;

	//Scan delay - small delay to be inserted after each scan iteration to allow collisions to register
	public float scanDelay;

	//This string holds the scanned level data to be sent to opponent
	private string scannedLevelData = "";
	private char delimiter = '\n';

	void Start () {
		gridCollider = worldGrid.GetComponent<BoxCollider> ();
		gridBlockSize = objectController.gridBlockSize;
		//Set TrackScanner position to lowest X and Z position corner of world grid at base Y value
		//Add size of TrackScanner collider to compensate for overlapping colliders on build pieces
		startX = (worldGrid.transform.position.x - (gridCollider.size.x / 2.0f)) + GetComponent<BoxCollider>().size.x;
		startZ = (worldGrid.transform.position.y - (gridCollider.size.z / 2.0f)) + GetComponent<BoxCollider>().size.z;
		transform.position = new Vector3 (startX, baseY, startZ);
	}

	void Update() {
		
	}

	//Can be called by other scripts
	/*
	 *  This function scans and processes the current track and saves the data to a text file, which is then sent to
	 *  another player and rebuilt in race mode. 
	 *  TrackScanner has a box collider that detects build objects. When a collision is detected, the appropriate object
	 *  is recorded by OnTriggerEnter.
	 */
	public IEnumerator ProcessTrack() {
		scanning = true; //begin scanning - record trigger collisions

		//Iterate through all grid spaces
		for (int i = 0; i < gridYSpaces; i++) {
			transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
			for (int j = 0; j < gridZSpaces; j++) {
				transform.position = new Vector3(startX, transform.position.y, transform.position.z);
				for (int k = 0; k < gridXSpaces; k++) {
					transform.position = new Vector3 (startX + gridBlockSize * k, transform.position.y, transform.position.z);
					yield return new WaitForSeconds (scanDelay);
				}
				transform.position = new Vector3 (transform.position.x, transform.position.y, startZ + gridBlockSize * j);
				yield return new WaitForSeconds (scanDelay);
			}
			transform.position = new Vector3 (transform.position.x, transform.position.y + yIncrements * (i+1), transform.position.z);
			yield return new WaitForSeconds (scanDelay);
		}
			
		scanning = false;
		print ("Scan complete.");
		yield return null;
	}

	//A (much) quicker version of ProcessTrack() - to be used as default unless we start running into issues.
	public IEnumerator QuickProcessTrack() {
		scanning = true;
		GetComponent<BoxCollider> ().size = new Vector3 (gridXSpaces * gridBlockSize, 1, gridZSpaces * gridBlockSize);
		transform.position = new Vector3 (0, baseY - 1, 0);
		while (transform.position.y < yIncrements * gridYSpaces) {
			transform.position = new Vector3 (0, transform.position.y + 1, 0);
			yield return new WaitForSeconds (scanDelay);
		}
		scanning = false;
		print ("Quick scan complete.");
		yield return null;
	}

	void OnTriggerEnter(Collider other) {
		//If other object is a placed track piece
		if (scanning) {
			if (other.gameObject.tag == "BuildObject" || other.gameObject.tag == "Finish" || other.gameObject.tag == "Start") {
				scannedLevelData += other.gameObject.name + "\n";
				scannedLevelData += other.transform.position + "\n";
				scannedLevelData += "(" + other.transform.lossyScale.x + ", " + other.transform.lossyScale.y + ", " + other.transform.lossyScale.z + ")" + "\n";
				scannedLevelData += other.transform.rotation + "\n";
			}
		}
	}

	//Clean object names in scanned level data
	//Use this function to remove (#) tags on the end of prefab instances
	//Trims strings at first space - VERY IMPORTANT: DO NOT USE SPACES IN PREFAB NAMES
	private void cleanObjectNames() {
		string[] stringLines = scannedLevelData.Split (delimiter);
		scannedLevelData = "";
		for (int i = 0; i < stringLines.Length; i++) {
			if (i % 4 == 0) {
				string[] tokens = stringLines [i].Split (' ');
				scannedLevelData += (tokens [0] + "\n");
			} else {
				scannedLevelData += (stringLines [i] + "\n");
			}
		}
	}

	public bool IsScanning() {
		return scanning;
	}

	public void scanLevelData() {
		StartCoroutine (QuickProcessTrack ());
	}

	public string getScannedLevelData() {
		return scannedLevelData;
	}
		
}