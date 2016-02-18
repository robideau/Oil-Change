using UnityEngine;
using System.Collections;

public class testButtonClicked : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject carPrefab;
	public Vector3 startingPos;
	public Quaternion startingRot;

	void Start()
	{
		startingPos = new Vector3(0, 2, -5);
		startingRot = Quaternion.identity;
	}

	// Use this for initialization
	public void TestMode () {
		carPrefab.transform.position = startingPos;
		carPrefab.transform.rotation = startingRot;
		carPrefab.SetActive(true);
		mainCamera.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape"))
		{
			carPrefab.SetActive(false);
			mainCamera.SetActive(true);
		}
	}
}
