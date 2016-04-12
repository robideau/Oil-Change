using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/**
*create by Ryan Young
* last modified 3/25/16
 * used to store a reference to all playable games in matchmaking, server the purpose of reseeting the list by destroying all children currently
 * in the list
*/
public class childHolder : MonoBehaviour {

    //list of all children
    private List<GameObject> children = new List<GameObject>();
    int childCount = 0;

	public childHolder()
    {

    }

    //adds a child to list of children being tracked
    public void addChild(GameObject child)
    {
        children.Add(child);
        childCount++;
    }

    //remove all children from the game   (names sound bad but needed)
    public void destroyAllChildren()
    {
        foreach(GameObject child in children)
        {
            GameObject.Destroy(child);
        }
        children.Clear();
        childCount = 0;
    }

    public List<GameObject> getChildList()
    {
        return children;
    }

    public GameObject getChild(int i)
    {
        return children[i];
    }

    public bool checkChildField(int i)
    {
        Debug.Log("checking iField for child: " + children[i].name);
        InputField f = children[i].GetComponent<InputField>();
        if(f == null)
        {
            return false;
        }
        else
        {
            return children[i].GetComponent<InputField>().interactable;
        }
    }

    public int getCount()
    {
        return childCount;
    }
}
