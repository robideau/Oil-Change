using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/**
*created by Ryan Young march 8.
*
*this script is meant to talk with database and collect a list of playable games currently available
*it will then sort this list and update the display for the matchmaking screen
*/
public class managePlayableGameDisplay : MonoBehaviour {

    public GameObject parent;
    //the available playable games will need to be analyzed and loaded in the the games array
    private playableGame[] games;

    public Dropdown sortOption;
    public Toggle reverse;

    public int sortType = 0;
    public int reverseMod = 1;

    //sort the playableGame list based on sort option
    //calls updateList and updateDisplay for most uptoDate information
    public void sort()
    {
        upDateList();
        checkSort();

        //decided to do a mergeSort for this sort.
        mergeSortRec(0, games.Length-1);


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
    private void upDateList()
    {
        //still need to fetch all data for all playable games



    }

    //clears and reads all playable games in the desired sorted order
    private void upDateDisplay()
    {
        //need to clear out all children for the parent.
        //need to creat children based on sorted playable games and add all of them to the list of 



    }
}
