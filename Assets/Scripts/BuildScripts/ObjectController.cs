/* John Kirchner
 * 2/26/2016
 * 
 * ObjectController manages prefab instantiation and placement in build mode.
 *
 * Last update - 4/4/2016
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public float rayCastDist;
	public int gridBlockSize;
	public GameObject worldGrid;
	public int buildLimit = 30;
	private int buildCount = 0;
	public Text buildLimitWarning;
	public Text buildLimitCounter;
	public ModularChat chat;
	public testButtonClicked tbscript;

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

		buildLimit = GameObject.Find ("SessionData").GetComponent<playableGame> ().getBuildLimit ();
		updateBuildCounterText ();
	}

	// Update is called once per frame
	void Update () {
		// Check if we're in test mode
		if(!tbscript.getTestMode())
		{
			// Rotate object
			if(Input.GetKeyDown("r"))
			{
				if(currentObject)
				{
					currentObject.transform.Rotate(0, 90, 0);
				}
			}
			// Place object
			if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				currentObject = null;
				newPiecePlaced = true;
				updateBuildCounterText ();
				chat.enableInput ();
			}
			// Delete object
			if((Input.GetMouseButtonDown(2) || Input.GetKey("delete")) && !EventSystem.current.IsPointerOverGameObject())
			{
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, rayCastDist))
				{
					string hitTag = hit.collider.gameObject.tag;
					if (hitTag == "BuildObject" || hitTag == "Finish" || hitTag == "Start") {
						Destroy(hit.collider.gameObject);
						buildCount--;
						updateBuildCounterText ();
					}
					else if(hitTag == "ParentedBuildObject")
					{
						Destroy(hit.collider.gameObject.transform.parent.parent.gameObject);
						buildCount--;
						updateBuildCounterText ();
					}
				}
			}
			// Move grid up a level
			if(Input.GetKeyDown("page up") || Input.GetKeyDown("u"))
			{
				worldGrid.transform.position = new Vector3(0.0f, worldGrid.transform.position.y + 5.0f, 0.0f);
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
															 Camera.main.transform.position.y + 5.0f, 
															 Camera.main.transform.position.z);
				// "Refresh" currentObject
				currentObject.SetActive(false);
				currentObject.SetActive(true);
			}
			// Move grid down a level
			if(Input.GetKeyDown("page down") || Input.GetKeyDown("j"))
			{
				worldGrid.transform.position = new Vector3(0.0f, worldGrid.transform.position.y - 5.0f, 0.0f);
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
															 Camera.main.transform.position.y - 5.0f, 
															 Camera.main.transform.position.z);
				if(currentObject)
				{
					// "Refresh" currentObject if it exists
					currentObject.SetActive(false);
					currentObject.SetActive(true);
				}
			}

			// Move currentObject with mouse pointer
			if(currentObject)
			{
				RaycastHit hit;
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
		else
		{
			if(currentObject)
			{
				if(currentObject.activeSelf)
					currentObject.SetActive(false);
			}
		}
	}

	// Called by prefab buttons in buildScreen.
	public void SetCurrentObject (GameObject button) {
		if (buildCount < buildLimit) {
			buildCount++;
			chat.disableInput ();
			string prefabName = button.transform.GetChild (0).GetComponent<Text> ().text;
			GameObject toInstantiate = (GameObject)Resources.Load (prefabsDirectory + "/" + prefabName, typeof(GameObject));
			if (currentObject != null) {// Delete currentObject if object has not been placed and button is clicked
				Destroy (currentObject);
				buildCount--;
			}
			currentObject = (GameObject)Instantiate (toInstantiate, hitPoint, Quaternion.identity);
		} else {
			StartCoroutine (displayBuildLimitWarning ());
		}
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
