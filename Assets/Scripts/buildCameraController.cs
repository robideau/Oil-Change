using UnityEngine;
using System.Collections;

public class buildCameraController : MonoBehaviour {

	public float speed;
	public float vRotate;
	public float hRotate;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called after all Update() functions called.
	// Good for moving cameras
	void LateUpdate()
	{
		//Right mouse button held down
		if(Input.GetMouseButton(1))
		{
			float mouseHorizontal = Input.GetAxis("Mouse X");
			float mouseVertical = Input.GetAxis("Mouse Y");

			transform.Rotate(-mouseVertical*vRotate, mouseHorizontal*hRotate, 0.0f);
		}
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		transform.Translate(moveHorizontal*speed , 0.0f, moveVertical*speed);
	}
}
