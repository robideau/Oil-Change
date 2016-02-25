/* David Robideau
 * 2/25/2016
 * 
 * This script procedurally generates the build menu based on premade build objects.
 * Also handles page turning logic.
 *
 * Last update - 2/25/2016
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuildMenu : MonoBehaviour {

	//MENU GENERATION OPTIONS
	public bool buildMenuFromTest; //Toggle to build from "Test" prefab folder instead of "Final"
	public int itemsPerPage; //Default 6 - cannot be changed yet

	//UI REFERENCES
	public GameObject buildObjectButtons;
	public Button nextPageButton;
	public Button prevPageButton;
	public Text pageIndicator;

	//PRIVATE PREFAB REFERENCES
	private string prefabsDirectory;
	private static List <GameObject> prefabList;

	//OTHER VARIABLES
	private int currentPage;
	private int numPages;
	private int finalPageObjectCount;

	void Start () {
		
		prefabList = new List<GameObject> (); //Instantiate prefab list

		if (buildMenuFromTest) { //Determine prefab resource location
			prefabsDirectory = "BuildObjects/Test";
		} else {
			prefabsDirectory = "BuildObjects/Final";
		}

		populatePrefabs (prefabsDirectory); //Populate prefab list
	
		currentPage = 1;
		numPages = prefabList.Count / itemsPerPage + 1;
		finalPageObjectCount = prefabList.Count % itemsPerPage;
		pageIndicator.text = "1/" + numPages;

		generateMenuItems (1);
	}

	void Update () {
		
	}

	//Assign prefabs to menu items. Currently only assigns names to text fields.
	private void generateMenuItems(int pageNumber) {
		List <GameObject> currentPageObjects;
		if (pageNumber == numPages) { //Final page - may have different button count
			currentPageObjects = prefabList.GetRange ((pageNumber - 1) * itemsPerPage, finalPageObjectCount);
			for (int i = finalPageObjectCount; i < itemsPerPage; i++) { //Deactivate unnecessary buttons
				buildObjectButtons.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		else { //Normal page - use default button count
			currentPageObjects = prefabList.GetRange ((pageNumber - 1) * itemsPerPage, itemsPerPage); //Find sublist of prefabs depending on page #
			for (int i = finalPageObjectCount; i < itemsPerPage; i++) { //Reactivate all buttons
				buildObjectButtons.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		for (int i = 0; i < currentPageObjects.Count; i++) {
			Text currentButtonText = buildObjectButtons.transform.GetChild (i).GetChild(0).GetComponent<Text>();
			currentButtonText.text = currentPageObjects[i].name;
		}
	}

	//Populate prefab list - to be called immediately, before generating menu
	private void populatePrefabs(string directory) {
		Object[] prefabSubList = Resources.LoadAll (directory, typeof(GameObject));
		foreach (GameObject subListObject in prefabSubList) {
			GameObject listObject = (GameObject)subListObject;
			prefabList.Add (listObject);
		}
	}

	public void nextPage() {
		if (currentPage < numPages) {
			currentPage++;
			generateMenuItems (currentPage);
			pageIndicator.text = currentPage + "/" + numPages;
		}
	}

	public void prevPage() {
		if (currentPage > 1) {
			currentPage--;
			generateMenuItems (currentPage);
			pageIndicator.text = currentPage + "/" + numPages;
		}
	}
}
