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

			// Vertical Tilt relative to camera direction
			transform.Rotate(-mouseVertical*vRotate, 0.0f, 0.0f);
			// Horizontal Rotate relative to world
			transform.Rotate(0.0f, mouseHorizontal*hRotate, 0.0f, Space.World);
		}
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		transform.Translate(moveHorizontal*speed*0.2f , 0.0f, moveVertical*speed*0.2f);
	}
}
