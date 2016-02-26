using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class updateText : MonoBehaviour {

    public Text gameName;
    public Text hostName;
    public Text keyWords;
    public Text ping;


    public updateText()
    {

    }

    public void update(string gameN, string hostN, string[] keys, int ping)
    {
        gameName.text = gameN;
        hostName.text = hostN;
        this.ping.text = ping.ToString();
        keyWords.text = "";
        for(int i = 0; i < keys.Length; i++)
        {
            keyWords.text = keyWords.text + ", " + keys[i];
        }
    }
}
