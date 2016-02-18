/* David Robideau
 * 2/18/2016
 * 
 * This script handles the game world boundaries. Attach to invisible walls/ceilings/floors.
 * Player cars must be tagged as "player". All players must have a PlayerCarController attached.
 *
 * Last update - 2/18/2016
 */
using UnityEngine;
using System.Collections;

public class WorldBoundaries : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			PlayerCarController playerCar = collision.gameObject.GetComponent<PlayerCarController> ();
			playerCar.fullReset ();
		}
	}

}
