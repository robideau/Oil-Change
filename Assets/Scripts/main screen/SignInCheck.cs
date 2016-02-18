using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignInCheck : MonoBehaviour {

    public InputField namefield;
    public InputField passfield;
    public GameObject login;
    public GameObject main;

	public void AccountCheck()
    {
        string name = namefield.text;
        string pass = passfield.text;

        if(UsernameCheck(name,pass))
        {
            login.SetActive(false);
            main.SetActive(true);
            namefield.text = "";
            passfield.text = "";
        }
        //test password hiding
        else if (pass.Equals("hack"))
        {
            login.SetActive(false);
            main.SetActive(true);
            namefield.text = "";
            passfield.text = "";
        }
        else
        {
            passfield.text = "";
        }

    }

    private bool UsernameCheck(string name, string pass)
    {
        //to use with data base to check if user name checks out then will check users password;

        //for now will just use guest sign in
        if (name.Equals("guest"))
        {
            return true;
        }
        return false;
    }

    private bool PasswordCheck(string pass)
    {
        //this will do the password checking part maybe.... might just do all in username Check
        return true;
    }
}
