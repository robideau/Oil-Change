using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignInCheck : MonoBehaviour {

    public InputField namefield;
    public InputField passfield;
    public GameObject login;
    public GameObject main;
    public Text warnings;
    public accountInfo curAccount;
    private char passed = 'q';

	public void AccountCheck()
    {
        string name = namefield.text;
        string pass = passfield.text;
        warnings.text = "";
        StartCoroutine(Name_PassCheck(name, pass));
        if (passed == 't')
        {
            login.SetActive(false);
            main.SetActive(true);
            namefield.text = "";
            passfield.text = "";
        }
        else if(passed == 'f')
        {
            passfield.text = "";
        }
        else if(passed == 'u')
        {
            passfield.text = "";
            namefield.text = "";
        }

    }

    IEnumerator Name_PassCheck(string name, string pass)
    {
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
                curAccount.loadAccount(name);
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
                warnings.text = check;
                passed = 'q';
            }
        }
    }

}
