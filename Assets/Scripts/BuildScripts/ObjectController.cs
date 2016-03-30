﻿/* John Kirchner
 * 2/26/2016
 * 
 * ObjectController manages prefab instantiation and placement in build mode.
 *
 * Last update - 3/30/2016
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public float rayCastDist;
	public int gridBlockSize;
	public int buildLimit = 30;
	private int buildCount = 0;
	public Text buildLimitWarning;
	public Text buildLimitCounter;
	public ModularChat chat;

	private GameObject currentObject;
	private bool buildMenuTestDir;
	private string prefabsDirectory;
	private Vector3 hitPoint;

	private bool newPiecePlaced = false;

	void Start () {
		//buildLimit = GameObject.Find ("SessionData").GetComponent<playableGame> ().getBuildLimit();
		currentObject = null;
		buildMenuTestDir = gameObject.GetComponent<BuildMenu>().buildMenuFromTest; //Grab build prefabs directory

		if (buildMenuTestDir) { //Determine prefab resource location
			prefabsDirectory = "BuildObjects/Test";
		} else {
			prefabsDirectory = "BuildObjects/Final";
		}

		updateBuildCounterText ();
	}

	// Update is called once per frame
	void Update () {
		// Place object
		if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (buildCount < buildLimit) {
				currentObject = null;
				newPiecePlaced = true;
				buildCount++;
				updateBuildCounterText ();
			} else {
				Destroy (currentObject);
				StartCoroutine (displayBuildLimitWarning ());
			}
			chat.enableInput ();
		}

		// If currentObject exists
		if(currentObject)
		{
			RaycastHit hit;
			if(Camera.current != null)
			{
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, rayCastDist))
				{
					if(!currentObject.activeSelf)
						currentObject.SetActive(true);
					if (hit.collider.gameObject.tag == "Grid") {
						hitPoint = hit.point;
						hitPoint.x = Mathf.Round(hit.point.x / gridBlockSize) * gridBlockSize; //Snap X
						hitPoint.z = Mathf.Round(hit.point.z / gridBlockSize) * gridBlockSize; //Snap Z
						currentObject.transform.position = hitPoint; //Snap to grid
					}
				}
				else
				{
					if(currentObject.activeSelf)
						currentObject.SetActive(false);
				}
			}
		}
	}

	// Called by prefab buttons in buildScreen.
	public void SetCurrentObject (GameObject button) {
		chat.disableInput ();
		string prefabName = button.transform.GetChild(0).GetComponent<Text>().text;
		GameObject toInstantiate = (GameObject) Resources.Load(prefabsDirectory + "/" + prefabName, typeof(GameObject));
		if(currentObject != null) // Delete currentObject if object has not been placed and button is clicked
			Destroy(currentObject);
		currentObject = (GameObject) Instantiate(toInstantiate, hitPoint, Quaternion.identity);
	}

	public bool isNewPiecesPlaced() {
		return newPiecePlaced;
	}

	public void resetNewPiecesFlag() {
		newPiecePlaced = false;
	}

	//Flash build limit warning
	public IEnumerator displayBuildLimitWarning() {
		buildLimitWarning.gameObject.SetActive (true);
		yield return new WaitForSeconds (3);
		buildLimitWarning.gameObject.SetActive (false);
	}

	public void updateBuildCounterText() {
		buildLimitCounter.text = "Pieces used: " + buildCount + "/" + buildLimit;
	}
}
