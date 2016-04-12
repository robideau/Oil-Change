using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FriendManager : MonoBehaviour {

    //current player
    public accountInfo player;
    //input field where a desired friend to add is entered
    public InputField friendToAdd;

    //warning for when a friend entered does not exist or problem with server
    public Text warning;

    //for matchmaking screen used to display friends list
    public Text prefabFriendMatchMaking;
    public GameObject friendHolderMatchMaking;

    //list of friends in matchmaking used for updating display
    private List<Text> MatchMakingPrefabs = new List<Text>();

    //for acount info screen used to display friends list
    public GameObject prefabFriendAcountInfo;
    public GameObject friendHolderAccountInfo;

    //list of friends in account info for updating display
    private List<GameObject> AccountInfoPrefabs = new List<GameObject>();

    /**
    *used to add a new friend to players friends list and update display on successful add
    */
    public void addNewFriend()
    {
        warning.text = "";
        if (!friendToAdd.text.Equals(""))
        {
            if (friendToAdd.text.Equals(player.getName()))
            { 
                warning.text = "can't add yourself";
            }
            else if (!player.hasFriend(friendToAdd.text))
            {
                StartCoroutine(checkFriendAndAdd(friendToAdd.text));
            }
            else
            {
                warning.text = "already have friend";
            }
        }

        friendToAdd.text = "";
    }

    /**
    *update the friends list display in matchmaking
    */
    public void updateMatchMakingListDisplay()
    {
        //get uptodate friends list
        string[] friends = player.getFriends();
        //clear display
        foreach(Text prefab in MatchMakingPrefabs)
        {
            GameObject.Destroy(prefab.gameObject);
        }
        MatchMakingPrefabs = new List<Text>();
        //load in display holding all friends
        foreach(string friend in friends)
        {
            Debug.Log(friend + " added to list");
            Text prefabclone;
            prefabclone = Instantiate(prefabFriendMatchMaking);
            prefabclone.text = friend;
            MatchMakingPrefabs.Add(prefabclone);
            prefabclone.transform.SetParent(friendHolderMatchMaking.transform);
            prefabclone.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void updatePlayerInfoListDisplay()
    {
        //get uptodate friends list
        string[] friends = player.getFriends();
        //clear display
        foreach (GameObject prefab in AccountInfoPrefabs)
        {
            GameObject.Destroy(prefab.gameObject);
        }
        AccountInfoPrefabs = new List<GameObject>();
        //load in display holding all friends
        foreach (string friend in friends)
        {
            Debug.Log(friend + " added to list AI");
            GameObject prefabclone;
            prefabclone = Instantiate(prefabFriendAcountInfo);
            prefabclone.GetComponent<setFriendNameAndImage>().setName(friend);
            AccountInfoPrefabs.Add(prefabclone);
            prefabclone.transform.SetParent(friendHolderAccountInfo.transform);
            prefabclone.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /**
    *check if the desired friend exists and add the to players friends list on success
    */
    private IEnumerator checkFriendAndAdd(string friend)
    {
        //need to change the php script name here
        string post_url = "http://proj-309-38.cs.iastate.edu/php/addfriend.php?" + "username=" + WWW.EscapeURL(player.getName()) + "&friend=" + friend;
        WWW f_check = new WWW(post_url);
        yield return f_check;
        if (f_check.error != null)
        {
            Debug.Log("problem loading server");
        }
        else if (f_check.text.Equals("frienddoesnotexist"))
        {
            Debug.Log("friend does not exist");
            warning.text = "friend does not exists";
        }
        else
        {
            Debug.Log("friend added: " + friend);
            player.addFriend(friend);
            updateMatchMakingListDisplay();
        }
        friendToAdd.text = "";
    }
}
