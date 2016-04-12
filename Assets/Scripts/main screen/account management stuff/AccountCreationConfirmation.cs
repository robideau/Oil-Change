using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
*created by Ryan Young, last modified March 2
*/
public class AccountCreationConfirmation : MonoBehaviour {

    public InputField nameField;
    public InputField passField;
    public InputField passConfirmField;

    public GameObject accountScreen;
    public GameObject mainScreen;

    public Text PasswordWarning;
    public Text userExistWarning;

    public void checkCofirmNew()
    {
        string name = nameField.text;
        string p = passField.text;
        string pc = passConfirmField.text;
        bool ncon = confirmName(name);
        bool pcon = passConfirm(p, pc);
        if (ncon && pcon)
        {
            StartCoroutine(newAccount(name, p));

        }

    }

   /**
   *will signal database to allocate new memory for a new account
   */
    IEnumerator newAccount(string name, string pass)
    {
        string post_url = "http://proj-309-38.cs.iastate.edu/php/createaccount.php?" + "username=" + WWW.EscapeURL(name) + "&password=" + pass; 
        WWW pStat_check = new WWW(post_url);
        yield return pStat_check;
        string confirmName = pStat_check.text;

        //account created
        if (confirmName.Equals("success"))
        {
            //set the games sessions Account
            GameObject cur = this.gameObject;
            accountInfo curAccount = cur.GetComponent<accountInfo>();
            curAccount.newAccount(name, pass);

            //account created successfully transistion to next screen
            accountScreen.SetActive(false);
            mainScreen.SetActive(true);
        }
        //username in use
        else if(confirmName.Equals("userexists"))
        {
            userExistWarning.text = "user name already in use";
        }
        //server problem
        else
        {
            userExistWarning.text = "problem creating account";
        }
    }


    /**
    *varifys that the given name is acceptable and not already in use
    */
    public bool confirmName(string name)
    {
        if(name.Length <= 0)
        {
            userExistWarning.text = "no user name entered";
            return false;
        }
        if (name.Contains(" ") || name.Contains("&") || name.Contains(",") || name.Contains(";") || name.Contains("\t") || name.Contains("\n"))
        {
            userExistWarning.text = "game name cannot contain characters: space, '&', ',', ';', tab, or return";
            return false;
        }
        

        userExistWarning.text = "";
        return true;
    }

    /**
    *makes sure that the password is acceptable and both passwords match
    */
    public bool passConfirm(string pass, string passCon)
    {

        bool upperCheck = false;
        bool numberCheck = false;
        bool lowerCheck = false;

        if (!pass.Equals(passCon))
        {
            PasswordWarning.text = "passwords do not match";
            return false;
        }
        else if(pass.Length < 8 || pass.Length > 20)
        {
            PasswordWarning.text = "passwords must be at least 8 characters long and no more than 20 characters long";
            return false;
        }

        if (pass.Contains(" ") || pass.Contains("&") || pass.Contains(",") || pass.Contains(";") || pass.Contains("\t") || pass.Contains("\n"))
        {
            PasswordWarning.text = "password cannot contain characters: space, '&', ',', ';', tab, or return";
            return false;
        }

        for (int i = 0; i < pass.Length; i++)
        {
            if (System.Char.IsLetter(pass[i]) && System.Char.IsLower(pass[i]))
            {
                lowerCheck = true;
            }
            else if(System.Char.IsLetter(pass[i]) && !System.Char.IsLower(pass[i]))
            {
                upperCheck = true;
            }
            else if (System.Char.IsNumber(pass[i]))
            {
                numberCheck = true;
            }
            
            if(pass[i] == ' ')
            {
                PasswordWarning.text = "no white spaces allowed in password";
                return false;
            }
        }
        if(upperCheck && lowerCheck && numberCheck)
        {
            PasswordWarning.text = "";
            return true;

        }

        string err = "pass missing: ";
        if (!upperCheck)
        {
            err += "an upper case letter, ";
        }
        if (!lowerCheck)
        {
            err += "a lower case letter, ";
        }
        if (!numberCheck)
        {
            err += "a number,";
        }
        err = err.Substring(0, err.Length - 1);
        err += ".";

        PasswordWarning.text = err;
        return false;
    }

}
