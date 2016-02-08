using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
    private int collected;
    public Text countText;
    public Text winText;

	void Start(){
		rb = GetComponent<Rigidbody> ();
        collected = 0;
        setText();
        winText.text = "";
    }

	void FixedUpdate (){
		
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal,0.0f,moveVertical);

		rb.AddForce (movement * speed);
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("pick up")){
            other.gameObject.SetActive(false);
            collected++;
            setText();
        }
    }

    void setText() {
        countText.text = "count: " + collected.ToString();
        if(collected == 5){
            winText.text = "at least you tried";
        }
    }
}
