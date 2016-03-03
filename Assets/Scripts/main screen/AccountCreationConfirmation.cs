using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
*created by Ryan Young, last modified March 2
*/
public class AccountCreationConfirmation : MonoBehaviour {

    public InputField nameField;
    public InputField emailField;
    public InputField passField;
    public InputField passConfirmField;

    public GameObject accountScreen;
    public GameObject mainScreen;

    public Text PasswordWarning;
    public Text userExistWarning;
    public Text emailWarning;

    public void checkCofirmNew()
    {
        string name = nameField.text;
        string email = emailField.text;
        string p = passField.text;
        string pc = passConfirmField.text;
        bool econ = confirmEmail(email);
        bool ncon = confirmName(name);
        bool pcon = passConfirm(p, pc);
        if (ncon && pcon && econ)
        {

            //want to find a way to modify this component without setter methods
            //if needed setter methods will be done but thats a bit annoying...
            //GameObject cur = this.gameObject;
            //accountInfo curAccount = cur.GetComponent<accountInfo>();
            

            accountScreen.SetActive(false);
            mainScreen.SetActive(true);
        }

    }

    /**
    *varifys that the given name is acceptable and not already in use
    */
    private bool confirmName(string name)
    {

        //will needed to do data base stuff with this but for now will do basic checks


        if(name.Length <= 0)
        {
            userExistWarning.text = "no user name entered";
            return false;
        }
        for (int i = 0; i < name.Length; i++)
        {
            if(name[i] == ' ')
            {
                userExistWarning.text = "no white spaces allowed in the username";
                return false;
            }
        }

        userExistWarning.text = "";
        return true;
    }

    /**
    *makes sure that the password is acceptable and both passwords match
    */
    private bool passConfirm(string pass, string passCon)
    {

        bool upperCheck = false;
        bool numberCheck = false;
        bool lowerCheck = false;

        if (!pass.Equals(passCon))
        {
            PasswordWarning.text = "passwords do not match";
            return false;
        }
        else if(pass.Length < 8)
        {
            PasswordWarning.text = "passwords must be at least 8 characters long";
            return false;
        }

        for(int i = 0; i < pass.Length; i++)
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

    /**
    *checks email to see if it is of standard form... word@word
    */
    private bool confirmEmail(string email)
    {
        int split = -1;
        for(int i = 0; i < email.Length; i++)
        {
            if(email[i] == '@'){
                split = i;
                break;
            }
        }

        //if split != -1 then @ was found and if its index is not at the ends of the email
        //string then the email is assumed to be formatted fine.
        if(split <= 0 || split == email.Length-1)
        {
            emailWarning.text = "bad email given";
            return false;
        }

        emailWarning.text = "";
        return true;
    }
}
