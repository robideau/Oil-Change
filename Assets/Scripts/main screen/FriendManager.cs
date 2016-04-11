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
    private List<Text> prefabs = new List<Text>();

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
        foreach(Text prefab in prefabs)
        {
            GameObject.Destroy(prefab.gameObject);
        }
        prefabs = new List<Text>();
        //load in display holding all friends
        foreach(string friend in friends)
        {
            Debug.Log(friend + " added to list");
            Text prefabclone;
            prefabclone = Instantiate(prefabFriendMatchMaking);
            prefabclone.text = friend;
            prefabs.Add(prefabclone);
            prefabclone.transform.SetParent(friendHolderMatchMaking.transform);
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
