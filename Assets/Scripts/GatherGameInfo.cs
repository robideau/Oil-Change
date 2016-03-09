using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

/**
created by Ryan Young

*/

public class GatherGameInfo : MonoBehaviour {

    //information to gather from
    public Dropdown timeLimit;
    public Dropdown placeLimit;
    public InputField gameName;
    public Text keys;
    public accountInfo player;


    public GameObject prefab;
    GameObject prefabclone;
    public GameObject prefabParent;

    //this is part of the solution but is good for appearence sake for now
    public void gatherInfo_addToList()
    {

        //todo--
        //  add the invite only option
        //  extend for passwords
        //  once host names are figured out change the gameobject name to host's Game
        //todo--

        //make an instance of the prefab gameobject
        prefabclone =Instantiate(prefab);
        prefabclone.transform.SetParent(prefabParent.transform);

        playableGame reference = prefabclone.GetComponent<playableGame>();
        //set the BuildLimit for this playableGame
        reference.setBuildLimit(Int32.Parse(placeLimit.captionText.text));
        //set the play time for this playableGame in seconds
        string minTime = Regex.Match(timeLimit.captionText.text, @"\d+").Value;
        reference.setBuildTime(Int32.Parse(minTime)*60);
        //set the name for this game
        reference.setName(gameName.text);
        //break they keys text down into an array that will be deep copied into the playable game
        string keysToAdd = keys.text;
        char[] delim = { ',', ' ', '\n' };
        string[] keycollection = keysToAdd.Split(delim);
        reference.setKeys(keycollection);


        //update the prefabs text to clearly display the game information
        prefabclone.name = "game by " + player.getName() ;
        updateText updates = prefabclone.GetComponent<updateText>();
        updates.updateLarge(gameName.text,player.getName(),keycollection,0, placeLimit.captionText.text, timeLimit.captionText.text);

        //reset the create match screen details
        keys.text = "";
        gameName.text = "";
        
    }


}
