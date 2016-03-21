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
		replicateTrack ("ReferenceObject\n(0.0, 0.0, -20.0)\n(10, 0.05, 10)\n(0.0, 0.0, 0.0, 1.0)\n");
	}

	public void replicateTrack(string scannedTrackData) {
		string[] trackDataLines = scannedTrackData.Split (delimiter);
		for (int i = 0; i < trackDataLines.Length-1; i++) {
			if (i % 4 == 0) { //Object name
				GameObject toInstantiate = (GameObject) Resources.Load(prefabsDirectory + "/" + trackDataLines[i].Trim(), typeof(GameObject));
				replicatedObject = Instantiate (toInstantiate);
			} else if (i % 3 == 0) { //Rotation
				string[] rotationData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				replicatedObject.transform.rotation = new Quaternion (float.Parse (rotationData [0]), float.Parse (rotationData [1]), float.Parse (rotationData [2]), float.Parse (rotationData [3]));
			} else if (i % 2 == 0) { //Scale (lossy)
				string[] scaleData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				replicatedObject.transform.localScale = new Vector3 (float.Parse (scaleData [0]), float.Parse (scaleData [1]), float.Parse (scaleData [2]));
			} else { //Position
				string[] positionData = (trackDataLines[i].Substring(1, trackDataLines[i].Length-2)).Split(',');
				replicatedObject.transform.position = new Vector3 (float.Parse (positionData [0]), float.Parse (positionData [1]), float.Parse (positionData [2]));
			}
		}
	}
}
