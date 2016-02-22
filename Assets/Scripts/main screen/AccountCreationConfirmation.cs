using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            accountScreen.SetActive(false);
            mainScreen.SetActive(true);
        }

    }

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
}
