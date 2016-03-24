/* David Robideau
 * 3/21/2016
 * 
 * This script replicates the player's opponent's track after build mode is completed.
 * Processes level data string and instantiates prefabs in correct locations.
 *
 * Last update - 3/21/2016
 */

using UnityEngine;
using System.Collections;

public class TrackReplicator : MonoBehaviour {

	private char delimiter = '\n';
	private string prefabsDirectory = "BuildObjects/Test";
	private GameObject replicatedObject = null;

	void Start() {
		//debug - test
		//replicateTrack ("ReferenceObject\n(0.0, 0.0, -20.0)\n(10, 0.05, 10)\n(0.0, 0.0, 0.0, 1.0)\n");
	}

	public void replicateTrack(string scannedTrackData) {
		string[] trackDataLines = scannedTrackData.Split (delimiter);
		for (int i = 0; i < trackDataLines.Length-1; i++) {
			if (i % 4 == 0) { //Object name
				GameObject toInstantiate = (GameObject) Resources.Load(prefabsDirectory + "/" + trackDataLines[i].Trim(), typeof(GameObject));
				replicatedObject = Instantiate (toInstantiate);
			} else if (i == 3 || i % 4 == 3) { //Rotation
				string[] rotationData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				//replicatedObject.transform.rotation = new Quaternion (float.Parse (rotationData [0]), float.Parse (rotationData [1]), float.Parse (rotationData [2]), float.Parse (rotationData [3]));
			} else if (i == 2 || i % 4 == 2) { //Scale (lossy)
				string[] scaleData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				replicatedObject.transform.localScale = new Vector3 (float.Parse (scaleData [0]), float.Parse (scaleData [1]), float.Parse (scaleData [2]));
			} else if (i == 1 || i % 4 == 1) { //Position
				string[] positionData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				print ("Position data for " + replicatedObject.name + ":" + positionData[0] + " " + positionData[1] + " " + positionData[2]);
				replicatedObject.transform.position = new Vector3 (float.Parse (positionData [0]), float.Parse (positionData [1]), float.Parse (positionData [2]));
			}
		}
	}
}
