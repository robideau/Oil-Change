using UnityEngine;
using System.Collections;

public class musicControl : MonoBehaviour {

	private GameObject currentSong;
	private bool isPlaying;
	private static musicControl _this;

	void Awake()
	{
		if(!_this)
			_this = this;
		else
			Destroy(this.gameObject);

		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		int menuSongCount = transform.childCount;

		currentSong = transform.GetChild(Random.Range(0, menuSongCount)).gameObject;

		currentSong.SetActive(true);

		Invoke("changeSong", currentSong.GetComponent<AudioSource>().clip.length);
	}

	private void changeSong ()
	{
		currentSong.SetActive(false);
		int folderSongCount = transform.childCount;
		currentSong = transform.GetChild(Random.Range(0, folderSongCount)).gameObject;
		currentSong.SetActive(true);
	}
}
