using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/**
*created by Ryan Young march 8.
*
*this script is meant to talk with database and collect a list of playable games currently available
*it will then sort this list and update the display for the matchmaking screen
*/
public class managePlayableGameDisplay : MonoBehaviour {

    public GameObject parent;
    //the available playable games will need to be analyzed and loaded in the the games array
    private List<playableGame> games;

    public Dropdown sortOption;
    public Toggle reverse;

    private int sortType = 0;
    private int reverseMod = 1;

    //sort the playableGame list based on sort option
    //calls updateList and updateDisplay for most uptoDate information
    public void sort()
    {
        upDateList();
        checkSort();

        //decided to do a mergeSort for this sort.
		mergeSortRec(0, games.Count-1);


        upDateDisplay();

    }

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

    private void checkSort()
    {
        //check the sort option and set sortType accourdingly
        if (sortOption.captionText.text.Equals("by ping"))
        {
            sortType = 0;
        }
        else if (sortOption.captionText.text.Equals("by host name"))
        {
            sortType = 1;
        }
        else if (sortOption.captionText.text.Equals("by game name"))
        {
            sortType = 2;
        }
        else if (sortOption.captionText.text.Equals("by build time"))
        {
            sortType = 3;
        }
        else
        {
            sortType = 4;
        }

        //if the reverse toggle is on then the order of sorting will be reversed
        if (reverse.isOn)
        {
            reverseMod = -1;
        }
        else
        {
            reverseMod = 1;
        }

    }

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

    //update the list of playable games
    IEnumerator upDateList()
    {
        //still need to fetch all data for all playable games
        string url = "http://proj-309-38.cs.iastate.edu/php/login.php?" + "sortBy=hostUser" + "&reverse=FALSE" + "&searchUser=" + "";
        WWW g_list = new WWW(url);
        yield return g_list;

        //break the strings down for reading
        string[] each_session = g_list.text.Split("\n"[0]);
        for(int i = 0; i < each_session.Length; i++)
        {
            playableGame input = new playableGame();
            string[] session_breakdown = each_session[i].Split(";"[0]);
            input.setName(session_breakdown[0]);
            input.setHost(session_breakdown[1]);
            input.setBuildTime(Int32.Parse(session_breakdown[2]));
            input.setBuildLimit(Int32.Parse(session_breakdown[3]));
            char[] delim = { ',', ' ', '\n' };
            string[] keys = each_session[4].Split(delim);
            input.setKeys(keys);
            input.setPass(session_breakdown[5]);
        }

    }

    //clears and reads all playable games in the desired sorted order
    private void upDateDisplay()
    {
        //need to clear out all children for the parent.
        //need to creat children based on sorted playable games and add all of them to the list of 



    }
}
