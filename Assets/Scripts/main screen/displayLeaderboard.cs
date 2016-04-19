using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class displayLeaderboard : MonoBehaviour
{

    private bool updating;

    //parent object that holds prefabs created
    public GameObject parent;
    //prefab objects to create
    public GameObject prefab;
    //error
    public Text warning;
    //current account
    public accountInfo account;
    //holds all the prefabs previously created
    private List<GameObject> children = new List<GameObject>();

    public void getLeaderboard()
    {
        updating = false;
        Debug.Log("updating leaderboard");
        StartCoroutine(updateRankings());
        Debug.Log("Leaderboard updated");
        StartCoroutine(fetchLeaderboard());
    }

    //updates leaderboard in database
    private IEnumerator updateRankings()
    {
        updating = true;
        string post_url = "http://proj-309-38.cs.iastate.edu/php/updaterankings.php?";
        WWW u_check = new WWW(post_url);
        yield return u_check;
        updating = false;
    }

    private IEnumerator fetchLeaderboard()
    {
        //wait for leaderboards to update
        while (updating)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //connect to server and gather leaderboard
        //returned string is sorted by ranking
        string post_url = "http://proj-309-38.cs.iastate.edu/php/getleaderboard.php";
        WWW lb_check = new WWW(post_url);
        yield return lb_check;

        if (lb_check.error != null)
        {
            Debug.Log("problem loading friends list");
            warning.text = "problem connecting to server";
        }
        else {
            warning.text = "";
            //clear leaderboards that where currently loaded up
            foreach(GameObject obj in children)
            {
                GameObject.Destroy(obj);
            }

            //load in new leaderboards
            string[] leaderboard = lb_check.text.Split('\n');
            foreach (string entry in leaderboard)
            {
                if (!entry.Contains(","))
                {
                    break;
                }

                //parse string for all current information
                string[] curLine = entry.Split(',');
                
                //instantiat prefab that holds infromation
                GameObject prefabclone;
                prefabclone = Instantiate(prefab);
                prefabclone.transform.SetParent(parent.transform);
                children.Add(prefabclone);

                //get components of the prefab that store specific information
                Text rank = prefabclone.transform.Find("boardRank").GetComponent<Text>();
                Text userName = prefabclone.transform.Find("boardName").GetComponent<Text>();
                Text Wins = prefabclone.transform.Find("boardWon").GetComponent<Text>();
                Text Loses = prefabclone.transform.Find("boardLost").GetComponent<Text>();
                Text Ties = prefabclone.transform.Find("boardTied").GetComponent<Text>();
                Text HighScore = prefabclone.transform.Find("boardHighScore").GetComponent<Text>();

                //set information
                userName.text = curLine[0];
                rank.text = curLine[1];
                Wins.text = curLine[2];
                Loses.text = curLine[3];
                Ties.text = curLine[4];
                HighScore.text = curLine[5];

                if (userName.text.Equals(account.getName()))
                {
                    for(int i = 1; i < 6; i++)
                    {
                        prefabclone.transform.Find("divider" + i).GetComponent<Image>().color = Color.green;
                    }
                    prefabclone.GetComponent<Image>().color = Color.green;
                    userName.color = Color.green;
                    rank.color = Color.green;
                    Wins.color = Color.green;
                    Loses.color = Color.green;
                    Ties.color = Color.green;
                    HighScore.color = Color.green;
                }
            }
        }
    }
}
