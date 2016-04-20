using UnityEngine;
using System.Collections;

public class musicControl : MonoBehaviour {

	public GameObject menuMusic;
	public GameObject buildMusic;
	public GameObject raceMusic;
	public GameObject currentFolder;
	private GameObject currentSong;


	// Use this for initialization
	void Start () {
		int menuSongCount = menuMusic.transform.childCount;

		currentFolder = menuMusic;

		currentSong = menuMusic.transform.GetChild(Random.Range(0, menuSongCount)).gameObject;

		currentSong.SetActive(true);

		Invoke("changeSong", currentSong.GetComponent<AudioSource>().clip.length);
	}

	private void changeSong ()
	{
		currentSong.SetActive(false);
		int folderSongCount = currentFolder.transform.childCount;
		currentSong = menuMusic.transform.GetChild(Random.Range(0, folderSongCount)).gameObject;
		currentSong.SetActive(true);
	}
}
