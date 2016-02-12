using UnityEngine;
using System.Collections;

public class GoForward : MonoBehaviour {

    GameObject current;

    // Use this for initialization
    void Start () {
        current = gameObject;
	}

    public void goForward(GameObject forward)
    {
        current.SetActive(false);
        forward.SetActive(true);
    }

}
