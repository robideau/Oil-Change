/* David Robideau
 * 2/25/2016
 * 
 * This script procedurally generates the build menu based on premade build objects.
 * Also handles page turning logic.
 *
 * Last update - 3/9/2016
 */

using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuildMenu : MonoBehaviour {

	//MENU GENERATION OPTIONS
	public bool buildMenuFromTest; //Toggle to build from "Test" prefab folder instead of "Final"
	public int itemsPerPage; //Default 6 - cannot be changed yet
	public bool buttonTextOn;
	public Texture2D defaultTexture;

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

		//Only pull prefab icons in editor - can not be used in-game.
		if (Application.isEditor) {
			generateIcons ();
		}

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
			//Generate button text
			Text currentButtonText = buildObjectButtons.transform.GetChild (i).GetChild (0).GetComponent<Text> ();
			currentButtonText.text = currentPageObjects [i].name;
			if (!buttonTextOn) {
				currentButtonText.enabled = false;
			}
			//Generate button image
			Sprite currentButtonSprite; 
			byte[] imageData;
			if (Application.isEditor) {
				imageData = File.ReadAllBytes(Application.dataPath + "/StreamingAssets/Icons/" + currentPageObjects[i].name + ".png");
			} else {
				imageData = File.ReadAllBytes(Application.streamingAssetsPath + "/Icons/" + currentPageObjects[i].name + ".png");
			}
			Texture2D currentButtonTexture = new Texture2D(1, 1);
			currentButtonTexture.LoadImage (imageData);
			currentButtonSprite = Sprite.Create (currentButtonTexture, 
				new Rect (0, 0, currentButtonTexture.width, currentButtonTexture.height),
				new Vector2 (0.5f, 0.5f),
				50);
			buildObjectButtons.transform.GetChild (i).GetComponent<Image> ().overrideSprite = currentButtonSprite;
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

	//Pull prefab icons and apply to buttons - can only be run in the editor, but must be run once before build.
	private void generateIcons() {
		#if UNITY_EDITOR
		for (int i = 0; i < prefabList.Count; i++) {
			GameObject currentPrefab = prefabList [i];
			Texture2D currentPrefabIconTexture = null;
			currentPrefabIconTexture = AssetPreview.GetAssetPreview (currentPrefab); //Generate texture
			byte[] textureData = null;
			if(currentPrefabIconTexture == null)
				textureData = defaultTexture.EncodeToPNG();
			else
				textureData = currentPrefabIconTexture.EncodeToPNG (); //Generate image from texture
			File.WriteAllBytes (Application.dataPath + "//StreamingAssets//Icons//" + currentPrefab.name + ".png", textureData); //Save image to assets
		}
		#endif
	}

}
