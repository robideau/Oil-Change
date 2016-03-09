using UnityEngine;
using System.Collections;
using System.IO;

public class FileSend : MonoBehaviour {
	//URL for where to write the file to
	public string url = "http://proj-309-38.cs.iastate.edu";
	public string fileName = "/Users/nickbramanti/hello.txt";
	// Use this for initialization
	void Start () {
		print("!!!");
		StartCoroutine (uploadFile());
	}

	IEnumerator uploadFile() {
		//create a new WWWForm
		WWWForm form = new WWWForm ();
		form.AddField ("action", "file upload");
		//add the text data as bytes
		byte[] fileData = File.ReadAllBytes(fileName);
		//add the text information and name, as well as tag it as a text file
		form.AddBinaryData ("file", fileData, "text file", "text");
		WWW w = new WWW (url, form);

		//wait for w to return
		yield return w;

		//check if there was an error while uploading and print it
		if (w.error != null) {
			print ("Error" + w.error);
		} else {
			if (w.uploadProgress == 1 && w.isDone) {
				yield return new WaitForSeconds(5);
				WWW w2 = new WWW (url + "textfile");
				yield return w2;
				if (w2.error != null) {
					print ("Error" + w2.error);
				} else {
					if (w2.text != null && w2.text != "") {
						print ("Finished Uploading File: text file");
					}
				}
			}
		}
	}
}
