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
	public float rayCastDist;

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
		// Place object
		if(Input.GetMouseButtonDown(0))
		{
			currentObject = null;
		}

		// If currentObject exists
		if(currentObject)
		{
			RaycastHit hit;
			if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, rayCastDist))
			{
				if(!currentObject.activeSelf)
					currentObject.SetActive(true);
				if(hit.collider.gameObject.tag == "Grid")
					currentObject.transform.position = hit.point;
			}
			else
			{
				if(currentObject.activeSelf)
					currentObject.SetActive(false);
			}
		}
	}

	// Called by prefab buttons in buildScreen.
	public void SetCurrentObject (GameObject button) {
		string prefabName = button.transform.GetChild(0).GetComponent<Text>().text;
		GameObject toInstantiate = (GameObject) Resources.Load(prefabsDirectory + "/" + prefabName, typeof(GameObject));
		if(currentObject != null) // Delete currentObject if object has not been placed and button is clicked
			Destroy(currentObject);
		currentObject = (GameObject) Instantiate(toInstantiate, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
	}
}
