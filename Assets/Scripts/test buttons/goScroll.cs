using UnityEngine;
using System.Collections;

public class goScroll : MonoBehaviour {

    public GameObject thisScreen;
    public GameObject newScreen;

    public void goToScroll()
    {
        thisScreen.SetActive(false);
        newScreen.SetActive(true);

    }
}
