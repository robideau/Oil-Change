using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class changeScene : MonoBehaviour {


	public void switchScenes(string scene)
    {
		SceneManager.LoadScene(scene);
    }
}
