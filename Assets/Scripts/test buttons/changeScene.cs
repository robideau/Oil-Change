using UnityEngine;
using System.Collections;

public class changeScene : MonoBehaviour {


	public void switchScenes(string scene)
    {
        Application.LoadLevel(scene);
    }
}
