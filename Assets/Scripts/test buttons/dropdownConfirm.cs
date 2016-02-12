using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dropdownConfirm : MonoBehaviour {

    private Text words;
    public Dropdown drop;
    public GameObject main;

    public void ChangeTheScene(GameObject current) {

        words = drop.captionText;
        switch (words.text)
        {
            case "effect 1":
                break;
            case "effect 2":
                break;
            case "effect 3":
                current.SetActive(false);
                main.SetActive(true);
                break;
            default:
                break;
        }
        


	}
}
