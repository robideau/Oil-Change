using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour {

    public accountInfo player;
    public InputField friendToAdd;

    public void addNewFriend()
    {
        if (!friendToAdd.text.Equals(""))
        {
            StartCoroutine(checkFriendAndAdd(friendToAdd.text));
        }
    }

    public void updateLisDisplay()
    {

    }

    private IEnumerator checkFriendAndAdd(string friend)
    {
        //need to change the php script name here
        string post_url = "http://proj-309-38.cs.iastate.edu/php/login.php?" + "username=" + WWW.EscapeURL(player.getName()) + "&friend=" + friend;
        WWW f_check = new WWW(post_url);
        yield return f_check;
        if (f_check.error != null)
        {
            Debug.Log("problem loading server");
        }
        else if (f_check.Equals("none"))
        {
            Debug.Log("you have no friends");
        }
        else
        {
            player.addFriend(friend);
        }
    }
}
