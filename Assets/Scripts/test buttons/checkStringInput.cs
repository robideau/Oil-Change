using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class checkStringInput : MonoBehaviour {

    public GameObject currentTemplate;
    public InputField currentField;

    public void checkString(GameObject nextTemplate)
    {
        string temp = currentField.text;
        if(temp == "back")
        {
            currentTemplate.SetActive(false);
            nextTemplate.SetActive(true);
        }
    }

	
}
