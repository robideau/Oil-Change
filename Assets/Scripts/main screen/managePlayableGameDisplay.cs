using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

/**
*created by Ryan Young .
*       last updated 3/25/16
*this script is meant to talk with database and collect a list of playable games currently available
*it will then sort this list and update the display for the matchmaking screen
*/
public class managePlayableGameDisplay : MonoBehaviour {

    //parent to hold playable game prefabs
    public GameObject parent;
    public GameObject prefab;

    public childHolder managePrefabChildren;
    
    //the available playable games will need to be analyzed and loaded in the the games array
    private List<playableGame> games = new List<playableGame>();

    //options for sorting and toggle reverse sort mode
    public Dropdown sortOption;
    public Toggle reverse;

    //warning text box incase problem with data base
    public Text warning;

    //string and sort type indicator
    private string reverseMod = "";
    private string sortType = "";
    //determine if data is still being fetched from the database
    private bool gettingData = false;

    //sort the playableGame list based on sort option
    //calls updateList and updateDisplay for most uptoDate information
    public void upDateGames()
    {

        warning.text = "";
        upDateList();
        checkSort();

        //              keeping this in case I need a merge sort later on but don't currently need this since database does sorting
        //decided to do a mergeSort for this sort.
        //mergeSortRec(0, games.Count-1);               

        StartCoroutine(upDateList());
        StartCoroutine(upDateDisplay());

    }
            
    /*                          obsolete for now keeping if needed later on
    //recursively call quick sort paritioning all elements into correct places
    private void mergeSortRec(int start, int end)
    {

        if (start > end)
        {
            int mid = (start + end) / 2;
            mergeSortRec(start, mid);
            mergeSortRec(mid + 1, end);

            merge(start, mid + 1, end);
        }
    }
    */

    /*                              obsolete af of now keeping if needed later on
    //partition all elements in array around the pivot
    private void merge(int start,int mid, int end)
    {
        playableGame[] sorted = new playableGame[end-start+1];

        int startEnd = mid - 1;
        int i = start;
        int j = mid;
        int pos = 0;

        while (i <= startEnd && j <= end)
        {
            if(compare(games[i],games[j]) <= 0){
                sorted[pos] = games[i];
                i++;
                pos++;
            }
            else
            {
                sorted[pos] = games[j];
                j++;
                pos++;
            }
        }

        while(i <= startEnd)
        {
            sorted[pos] = games[i];
            i++;
            pos++;
        }

        while(j <= end)
        {
            sorted[pos] = games[j];
            j++;
            pos++;
        }

        for(int k = 0; k < sorted.Length; k++)
        {
            games[start] = sorted[k];
            start++;
        }

    }
    */

    private void checkSort()
    {
        //check the sort option and set sortType accourdingly
        //if (sortOption.captionText.text.Equals("by ping"))
        //{
        //    sortType = 0;
        //}
        if (sortOption.captionText.text.Equals("by host name"))
        {
            sortType = "hostUser";
        }
        else if (sortOption.captionText.text.Equals("by game name"))
        {
            sortType = "sessionName";
        }
        else if (sortOption.captionText.text.Equals("by build time"))
        {
            sortType = "buildTime";
        }
        else
        {
            sortType = "buildLimit";
        }

        //if the reverse toggle is on then the order of sorting will be reversed
        if (reverse.isOn)
        {
            reverseMod = "TRUE";
        }
        else
        {
            reverseMod = "FALSE";
        }

    }

    /*                  as of now obsolete keeping incase needed later
    //compare two playable game objects 
    private int compare(playableGame a, playableGame b)
    {
        //compare by ping
        if(sortType == 0)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! PING is not checkable right now so deafulted to host name comparison
            //this needs to be changed!!!!
            return a.getHost().CompareTo(b.getHost()) * reverseMod;
        }
        //compare by Host Name
        else if (sortType == 1)
        {
            return a.getHost().CompareTo(b.getHost())*reverseMod;
        }
        //compare by game name
        else if (sortType == 2)
        {
            return a.getName().CompareTo(b.getName())*reverseMod;
        }
        //compare by build time
        else if (sortType == 3)
        {
            return a.getBuildTime().CompareTo(b.getBuildTime())*reverseMod;
        }
        //compare by build limit
        else
        {
            return a.getBuildLimit().CompareTo(b.getBuildLimit())*reverseMod;
        }

    }
    */

    //update the list of playable games
    IEnumerator upDateList()
    {

        games.Clear();

        gettingData = true;
        //still need to fetch all data for all playable games
        string url = "http://proj-309-38.cs.iastate.edu/php/getmatches.php?" + "sortBy=" + sortType + "&reverse=" + reverseMod + "&searchUser=" + "";
        WWW g_list = new WWW(url);
        yield return g_list;
        Debug.Log(g_list.text);
        if (g_list.text.Contains(";")){
            //break the strings down for reading
            string[] each_session = g_list.text.Split("\n"[0]);
            //fore each session break the string down furthur and all game details to a new playable game which is added to the list in the order
            //specified
            for (int i = 0; i < each_session.Length; i++)
            {
                if (each_session[i].Length == 0)
                {
                    continue;
                }
                playableGame input = new playableGame();
                string sessionModed = each_session[i].Replace(' ', ',');
                string[] session_breakdown = sessionModed.Split(";"[0]);

                Debug.Log(session_breakdown[0]);
                input.setName(session_breakdown[0].Trim(","[0]));

                Debug.Log(session_breakdown[1]);
                input.setHost(session_breakdown[1].Trim(","[0]));

                Debug.Log(session_breakdown[2]);
                input.setBuildTime(Int32.Parse(session_breakdown[2].Trim(","[0])));

                Debug.Log(session_breakdown[3]);
                input.setBuildLimit(Int32.Parse(session_breakdown[3].Trim(","[0])));

                Debug.Log(session_breakdown[4]);
                char[] delim2 = { ',', '\t' };
                string[] keys = session_breakdown[4].Split(delim2);
                input.setKeys(keys);

                Debug.Log(session_breakdown[5]);
                input.setPass(session_breakdown[5]);
                games.Add(input);
            }
        }
        else
        {
            warning.text = "database problem:\n" + g_list.text;
        }

        gettingData = false;
    }

    //clears and reads all playable games in the desired sorted order
    IEnumerator upDateDisplay()
    {
        //wait for data to be gathered from database
        while (gettingData)
        {
            yield return new WaitForSeconds(0.1f);
        }


        //destroy all lists playable game currently displayed
        managePrefabChildren.destroyAllChildren();

        //for every playable game from databased intantiate the prefab and add it to the list.
        //update each prefabs playable game this will be based on sort and up to date games
        foreach (playableGame game in games)
        {
            //make playable game object
            GameObject prefabclone;
            prefabclone = Instantiate(prefab);
            prefabclone.transform.SetParent(parent.transform);
            
            //establish new playable game for this prefab
            playableGame reference = prefabclone.GetComponent<playableGame>();
            reference.setBuildLimit(game.getBuildLimit());
            reference.setBuildTime(game.getBuildTime());
            reference.setName(game.getName());
            reference.setKeys(game.getKeyWords());
            reference.setHost (game.getHost());
            reference.setPass(game.getPass());
            
            //set details about prefab and change prefab display
            prefabclone.name = "game by " + reference.getHost();
            updateText updates = prefabclone.GetComponent<updateText>();

            updates.updateLarge(reference.getName(), reference.getHost(), reference.getKeyWords(), reference.getBuildLimit().ToString(), reference.getBuildTime().ToString());
            //check password paramaters for prefab
            if (game.getPass().Equals(""))
            {
                prefabclone.GetComponentInChildren<InputField>().interactable = false;
            }
            else
            {
                prefabclone.GetComponentInChildren<InputField>().interactable = true;
            }
            //add new game prefab to prefab manager
            managePrefabChildren.addChild(prefabclone);
        }
    }
}
