using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class dropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public RectTransform contains;
    public bool Pulled;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Pulled = true;
    }

    public static explicit operator dropdown(GameObject v)
    {
        throw new NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pulled = false;
    }


    // Use this for initialization
    void Start () {
        contains = transform.FindChild("container").GetComponent<RectTransform>();
        Pulled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Pulled)
        {
            Vector3 scale = contains.localScale;
            scale.y = Mathf.Lerp(scale.y, 1, Time.deltaTime * 12);
            contains.localScale = scale;
        }
        else
        {
            Vector3 scale = contains.localScale;
            scale.y = Mathf.Lerp(scale.y, 0, Time.deltaTime * 12);
            contains.localScale = scale;
        }
	}
}
