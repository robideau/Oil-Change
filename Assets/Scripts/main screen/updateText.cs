using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
*created by Ryan Young
*
*for the playable game prefabs this will update the prefab with the given information
*
*/
public class updateText : MonoBehaviour {

    public Text gameName;
    public Text hostName;
    public Text keyWords;
    public Text ping;

    public Text buildTime;
    public Text buildLimit;


    public updateText()
    {

    }

    //update for regular playable game 
    
    //for not opted agains this since I wanted the functionality first.
    //later if time will bring this back and make expandable options
   public void updateSimple(string gameN, string hostN, string[] keys, int ping)
    {
        gameName.text = gameN;
        hostName.text = hostN;
        this.ping.text = ping.ToString();
        keyWords.text = "";
        for(int i = 0; i < keys.Length; i++)
        {
            if(i == 0)
            {
                keyWords.text = keyWords.text + keys[i];
            }
            else
            {
                keyWords.text = keyWords.text + " " + keys[i];
            }
            
        }
    }
    
    //updates and extended playabale game
    public void updateLarge(string gameN, string hostN, string[] keys, int ping, string limitBuild, string limitTime)
    {
        updateSimple(gameN, hostN, keys, ping);

        buildTime.text = "   time: " + limitTime;
        buildLimit.text = "   items: " + limitBuild;
    }
}
