using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testButtonClicked : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject carPrefab;
	public Text buttonText;
	public ObjectController objectController;
	private bool inTestMode;

	void Start()
	{
		inTestMode = false;
	}



	// Use this for initialization
	public void TestMode () {
		if(inTestMode)
		{
			buttonText.text = "Test Mode";
			carPrefab.SetActive(false);
			mainCamera.SetActive(true);
			inTestMode = false;
		}
		else
		{
			buttonText.text = "Build Mode";
			carPrefab.SetActive(true);
			carPrefab.GetComponent<PlayerCarController> ().hasFinished = false;
			mainCamera.SetActive(false);
			inTestMode = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (carPrefab.GetComponent<PlayerCarController> ().hasFinished) {
			objectController.resetNewPiecesFlag ();
		}
	}
}
