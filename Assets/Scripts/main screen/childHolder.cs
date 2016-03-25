using UnityEngine;
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

	public childHolder()
    {

    }

    //adds a child to list of children being tracked
    public void addChild(GameObject child)
    {
        children.Add(child);
    }

    //remove all children from the game   (names sound bad but needed)
    public void destroyAllChildren()
    {
        foreach(GameObject child in children)
        {
            GameObject.Destroy(child);
        }
        children.Clear();
    }
}
