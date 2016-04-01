using UnityEngine;
using System.Collections;

/**
Ryan Young
    last modified 3/31/16
*check if a game session is currently in place and returning from matchmaking. If this is the case this
* will load in player information again and bypass login to matchmaking screen. 
*/
public class StillPlayingReLoad : MonoBehaviour {

    public GameObject MatchMaking;
    public GameObject Login;
	
	// Update is called once per frame
    //check for SessionStillPlayingObject and managerDuplicated Object, if they exist destroy them, reload account information
    //and bypass login screen to matchmaking screen
	void Update () {
        bool stillPlaying = false;
        GameObject temp = GameObject.Find("SessionStillPlaying");
        if(temp != null)
        {
            GameObject.Destroy(temp);
            stillPlaying = true;
        }
        GameObject temp2 = GameObject.Find("managerDuplicated");
        if(temp2 != null)
        {
            accountInfo account = GameObject.Find("Script manager").GetComponent<accountInfo>();
            
            accountInfo reload = temp2.GetComponent<accountInfo>();

            Debug.Log("testing reloading: " + reload.getName());


            account.loadAccount(reload.getName());
            GameObject.Destroy(temp2);
        }
        if (stillPlaying)
        {
            MatchMaking.SetActive(true);
            Login.SetActive(false);
            stillPlaying = false;
        }
	}
}
