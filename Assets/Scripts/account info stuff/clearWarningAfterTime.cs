using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
*created by Ryan Young march 3
*
*wouldnt need this if the displaymodprofile would let me run update.
*just clears the Error Texts every 10 seconds
*/
public class clearWarningAfterTime : MonoBehaviour {

    private float frame;
    public Text pError;

	// Use this for initialization
	void Start () {
        frame = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > frame + 10)
        {
            pError.text = "";
            frame = Time.time;
        }

	}
}
