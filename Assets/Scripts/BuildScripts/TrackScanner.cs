/* David Robideau
 * 3/9/2016
 * 
 * This script handles the processing and storage of build mode data
 *
 * Last update - 3/9/2016
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

	void Start () {
		gridCollider = worldGrid.GetComponent<BoxCollider> ();
		gridBlockSize = objectController.gridBlockSize;
		//Set TrackScanner position to lowest X and Z position corner of world grid at base Y value
		//Add size of TrackScanner collider to compensate for overlapping colliders on build pieces
		startX = (worldGrid.transform.position.x - (gridCollider.size.x / 2.0f)) + GetComponent<BoxCollider>().size.x;
		startZ = (worldGrid.transform.position.y - (gridCollider.size.z / 2.0f)) + GetComponent<BoxCollider>().size.z;
		transform.position = new Vector3 (startX, baseY + GetComponent<BoxCollider>().size.y, startZ);
	}

	void Update() {
		if (Input.GetKeyDown ("q")) {
			ProcessTrack ();
		}
	}

	//Can be called by other scripts
	/*
	 *  This function scans and processes the current track and saves the data to a text file, which is then sent to
	 *  another player and rebuilt in race mode. 
	 *  TrackScanner has a box collider that detects build objects. When a collision is detected, the appropriate object
	 *  is recorded by OnTriggerEnter.
	 */
	public void ProcessTrack() {
		scanning = true; //begin scanning - record trigger collisions

		//Iterate through all grid spaces
		for (int i = 0; i < gridYSpaces; i++) {
			transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
			for (int j = 0; j < gridZSpaces; j++) {
				transform.position = new Vector3(startX, transform.position.y, transform.position.z);
				for (int k = 0; k < gridXSpaces; k++) {
					transform.position = new Vector3(startX + gridBlockSize * k, transform.position.y, transform.position.z);
					print (transform.position);
				}
				transform.position = new Vector3(transform.position.x, transform.position.y, startZ + gridBlockSize * j);
			}
			transform.position = new Vector3(transform.position.x, transform.position.y + yIncrements * i, transform.position.z);
		}
			
		scanning = false;
	}

	void OnTriggerEnter(Collider other) {
		//If other object is a placed track piece
		if (scanning) {
			if (other.gameObject.tag == "BuildObject" || other.gameObject.tag == "Finish" || other.gameObject.tag == "Start") {
				
			}
		}
	}

	public bool IsScanning() {
		return scanning;
	}
		
}
