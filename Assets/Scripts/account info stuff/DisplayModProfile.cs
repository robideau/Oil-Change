using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
*created by Ryan Young march 3
*/
public class DisplayModProfile : MonoBehaviour {

    /**
    *user name displayer
    */
    public Text userNameDisplay;

    /**
    *played games displayer
    */
    public Text playedGames;

    /**
    *player rank displayer
    */
    public Text Rank;

    /**
    *accountInfo object for current Game Session
    */
    public accountInfo manager;

    /**
    *warning about passwords
    */
    public Text pWarning;

    /**
    *password inputfields
    */
    public InputField password;
    public InputField confirmPass;



    //need a scrollable friends list
    //need a scrollable accolade list

    /**
    * when the player enters the account screen all current information will be loaded into the screen
    */
    public void loadInText()
    {

        userNameDisplay.text = manager.getName();
        stats playerStats = manager.getStats();
        playedGames.text = "" + playerStats.getPlayed();
        Rank.text = "" + playerStats.getRank();

    }

    /**
    *if the player is on the account screen this will update the account info
    */
    public void upDateAccount()
    {
        //load in the passwords and email being dealt with
        string p1 = password.text;
        string p2 = confirmPass.text;

        //use new  Account Creation confirmation to confirm the password and email.
        AccountCreationConfirmation check = new AccountCreationConfirmation();
        //pass in the warning texts to the pcc check for updating if bad 
        check.PasswordWarning = pWarning;

        bool pcheck =false;
        if(!(p1.Equals("") && p2.Equals("")))
        {
            pcheck = check.passConfirm(p1, p2);
        }
        

        //good password givee
        if (pcheck)
        {
            pWarning.text = "Password Changed";
            //need to forward new password to database
            
        }

        password.text = "";
        confirmPass.text = "";
        
    }
}
