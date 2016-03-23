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
    private bool gettingData = false;

	public void AccountCheck()
    {
        passed = 'q';
        string name = namefield.text;
        string pass = passfield.text;
        warnings.text = "";
        StartCoroutine(Name_PassCheck(name, pass));
        StartCoroutine(confirm(name));
        

    }

    IEnumerator confirm(string name)
    {
        while (gettingData)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (passed == 't')
        {
            login.SetActive(false);
            main.SetActive(true);
            namefield.text = "";
            passfield.text = "";
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
                warnings.text = "problem with server";
                passed = 'x';
            }
        }
        gettingData = false;
    }

}
