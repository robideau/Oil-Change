/* David Robideau
 * 2/22/2016
 * 
 * This script stabilizes the player cameras to make gameplay appear smooth and easy to watch.
 *
 * Last update - 2/22/2016
 */

using UnityEngine;
using System.Collections;

public class CameraStabilizer : MonoBehaviour {

	public GameObject carTarget;
	public float cameraDamping;
	private Vector3 cameraOffset;

	void Start () {
		cameraOffset = this.transform.position - carTarget.transform.position;
	}

	void Update () {
		Vector3 finalPosition = carTarget.transform.position + cameraOffset;
		this.transform.position = finalPosition;
		Vector3 position = Vector3.Lerp (this.transform.position, finalPosition, Time.deltaTime * cameraDamping);
		this.transform.position = position;

		this.transform.LookAt (carTarget.transform.position);

		this.transform.eulerAngles = new Vector3 (0, carTarget.transform.eulerAngles.y, 0);
	}
}
