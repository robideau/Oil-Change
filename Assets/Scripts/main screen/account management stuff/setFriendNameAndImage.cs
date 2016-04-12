using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/**
*created by Ryan Young
    last modified 4/11/16
*this is used to update individual prefabs for a friend in the profile screen
*/
public class setFriendNameAndImage : MonoBehaviour {

    public Text Name;
    public Image Picture;

    /**
    *set the string of the given text to the given name
    */
    public void setName(string name)
    {
        Name.text = name;
    }

    //not implemented as of now but would load in one of preset picture to player picture
    public void setPicture()
    {
        throw new NotImplementedException();
    }
}
