using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
*renamde modify text to this Ryan Young
*this class is used to add to a list based on words slected on a dropdown
*
*used as of now in the matchmaking screen for game rules and for keywords.
*
*/
public class selectionAdd_ClearList : MonoBehaviour {

    public Text modify;
    int line = 0;
    int trackedLength = 0;
    public Text error;

    /**
    *sets the text in modify to empty... resets line count and tracked length
    */
    public void clear()
    {
        modify.text = "";
        trackedLength = 0;
        error.text = "";
        line = 0;
    }

    /**
    *adds the given input string to the end of the current text
    *returns to next line if line limit reached
    *
    */
    public void extend(string extend)
    {
        if (modify.text.Length < 20)
        {
            line = 0;
        }

        if ((trackedLength + extend.Length) > 20)
        {
            modify.text += "\n";
            trackedLength = 0;
            line++;
        }
        if (line == 3)
        {
            error.text = "maximum keys added";
            return;
        }

        if (trackedLength != 0)
        {
            modify.text = modify.text + "  " + extend;
            trackedLength += extend.Length;
        }
        else
        {
            modify.text += extend;
            trackedLength += extend.Length;
        }
    }

    /**
    *adds the text from the Dropdown to the end of modify account for line length and line number
    */
    public void extend(Dropdown pullFrom)
    {
        if (modify.text.Length < 20)
        {
            line = 0;
        }

        if ((trackedLength + pullFrom.captionText.text.Length) > 20)
        {
            modify.text += "\n";
            trackedLength = 0;
            line++;
        }
        if (line == 3)
        {
            error.text = "maximum keys added";
            return;
        }

        if (trackedLength != 0)
        {
            modify.text = modify.text + "  " + pullFrom.captionText.text;
            trackedLength += pullFrom.captionText.text.Length;
        }
        else
        {
            modify.text += pullFrom.captionText.text;
            trackedLength += pullFrom.captionText.text.Length;
        }
    }
}
