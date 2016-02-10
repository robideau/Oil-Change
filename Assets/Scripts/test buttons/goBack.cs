using UnityEngine;
using System.Collections;

public class goBack : MonoBehaviour
{

    private GameObject thisScreen;
    public GameObject newScreen;

    public void goToMain(GameObject current)
    {
        thisScreen = current;
        thisScreen.SetActive(false);
        newScreen.SetActive(true);

    }
}
