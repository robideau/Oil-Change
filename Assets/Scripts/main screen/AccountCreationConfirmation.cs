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
        if (confirmName(name) && passConfirm(p, pc))
        {

        }

    }

    private bool confirmName(string name)
    {
        return false;
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
            if(pass[i] > 'a' || pass[i] < 'z')
            {
                lowerCheck = true;
            }
            else if(pass[i] > 'A' || pass[i] < 'Z')
            {
                upperCheck = true;
            }
            else if (System.Char.IsDigit(pass[i]))
            {
                numberCheck = true;
            }
        }
        if(upperCheck && lowerCheck && numberCheck)
        {
            return true;
        }

        string err = "pass missing: ";
        if (!upperCheck)
        {
            err += "an upper case letter, "
        }
        if (!lowerCheck)
        {
            err += "a lower case letter, "
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
