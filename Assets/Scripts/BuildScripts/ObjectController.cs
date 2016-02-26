/* John Kirchner
 * 2/26/2016
 * 
 * ObjectController manages prefab instantiation and placement in build mode.
 *
 * Last update - 2/26/2016
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public Camera mainCamera;

	private GameObject currentObject;
	private bool buildMenuTestDir;
	private string prefabsDirectory;

	void Start () {
		currentObject = null;
		buildMenuTestDir = gameObject.GetComponent<BuildMenu>().buildMenuFromTest; //Grab build prefabs directory

		if (buildMenuTestDir) { //Determine prefab resource location
			prefabsDirectory = "BuildObjects/Test";
		} else {
			prefabsDirectory = "BuildObjects/Final";
		}
	}

	// Update is called once per frame
	void Update () {

	}

	// Called by prefab buttons in buildScreen.
	public void SetCurrentObject (GameObject button) {
		string prefabName = button.transform.GetChild(0).GetComponent<Text>().text;
		currentObject = (GameObject) Resources.Load(prefabsDirectory + "/" + prefabName, typeof(GameObject));
		currentObject.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
		Instantiate(currentObject, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
	}
}
