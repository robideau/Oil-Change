using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class friendsListScrollControll : MonoBehaviour {

    public Scrollbar scroll_controller;

    public GameObject scallablePart;

    public void scroll(bool left)
    {
        RectTransform s = (RectTransform) scallablePart.transform;
        float x = s.rect.width/50;
        x = (float) .1 * x;
        float mod = (float) 1.7-x;
        float val = scroll_controller.value;
        if (left)
        {
            val -= mod;
        }
        else
        {
            val += mod;
        }

        if(val > 1)
        {
            val = 1;
        }
        else if( val < 0)
        {
            val = 0;
        }

        scroll_controller.value = val;
        
    }
}
