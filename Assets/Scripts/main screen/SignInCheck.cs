using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
created by Ryan Young
    last modified 3/22/16
    used to check sign in information with data base and confirm user is valid and has correct password
*/
public class SignInCheck : MonoBehaviour {

    //user name
    public InputField namefield;
    //user password
    public InputField passfield;
    //used for screen transition from login to main menu
    public GameObject login;
    public GameObject main;
    //warnings indicate problems with loging in
    public Text warnings;
    //account information for current game session update on successful login
    public accountInfo curAccount;

    //indicator that login information is correct
    private char passed = 'q';
    //lock to wait for data gathering
    private bool gettingData = false;

    //call this to check all information
	public void AccountCheck()
    {
        //passed deafults
        passed = 'q';
        //get name and password strings
        string name = namefield.text;
        string pass = passfield.text;
        //clear warning
        warnings.text = "";
        //check information with database
        StartCoroutine(Name_PassCheck(name, pass));
        //confirm database successful gathering and transition to main menu
        StartCoroutine(confirm(name));
        

    }

    //waits for use check and preforms transitions
    IEnumerator confirm(string name)
    {
        while (gettingData)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //check status of database check and update input fields based on results
        if (passed == 't')
        {
            login.SetActive(false);
            main.SetActive(true);
            namefield.text = "";
            passfield.text = "";
            //loads all account information from database into current account for session
            curAccount.loadAccount(name);
        }
        else if (passed == 'f')
        {
            passfield.text = "";
        }
        else if (passed == 'u')
        {
            passfield.text = "";
            namefield.text = "";
        }
    }

    //send username and passowrd to database and confirm information is correct
    IEnumerator Name_PassCheck(string name, string pass)
    {

        gettingData = true;
        string post_url = "http://proj-309-38.cs.iastate.edu/php/login.php?" + "username=" + WWW.EscapeURL(name) + "&password=" + pass;
        WWW pn_check = new WWW(post_url);
        yield return pn_check;
        if (pn_check.error != null)
        {
            warnings.text = "problem connecting to server";
        }
        else
        {
            string check = pn_check.text;
            if (check.Equals("t"))
            {
                //curAccount.loadAccount(name);
                passed = 't';
            }
            else if (check.Equals("f"))
            {
                warnings.text = "incorrect password";
                passed = 'f';
            }
            else if (check.Equals("nouser"))
            {
                warnings.text = "bad user name";
                passed = 'u';
            }
            else
            {
                warnings.text = "problem with server";
                passed = 'x';
            }
        }
        gettingData = false;
    }

}
