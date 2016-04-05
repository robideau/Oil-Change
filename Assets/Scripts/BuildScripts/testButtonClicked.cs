using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testButtonClicked : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject carPrefab;
	public Text buttonText;
	public ObjectController objectController;
	public GameObject buildMenuUI;
	private bool inTestMode;

	void Start()
	{
		inTestMode = false;
	}



	// Use this for initialization
	public void TestMode () {
		if(inTestMode)
		{
			// We're in build mode
			buttonText.text = "Test Mode";
			carPrefab.SetActive(false);
			mainCamera.SetActive(true);
			buildMenuUI.SetActive(true);
			inTestMode = false;
		}
		else
		{
			// We're in test mode
			buttonText.text = "Build Mode";
			carPrefab.SetActive(true);
			carPrefab.GetComponent<PlayerCarController> ().hasFinished = false;
			mainCamera.SetActive(false);
			buildMenuUI.SetActive(false);
			inTestMode = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (carPrefab.GetComponent<PlayerCarController> ().hasFinished) {
			objectController.resetNewPiecesFlag ();
		}
	}

	public bool getTestMode()
	{
		return inTestMode;
	}
}
