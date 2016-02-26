using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class modifyText : MonoBehaviour {

    public Text modify;
    int line = 0;
    int trackedLength = 0;
    public Text error;

    public void clear()
    {
        modify.text = "";
        trackedLength = 0;
        error.text = "";
        line = 0;
    }

    public void extend(string extend)
    {
        if(modify.text.Length < 26)
        {
            line = 0;
        }

        if ((trackedLength + extend.Length) > 26)
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

    public void extend(Dropdown pullFrom)
    {
        if (modify.text.Length < 26)
        {
            line = 0;
        }

        if ((trackedLength + pullFrom.captionText.text.Length) > 26)
        {
            modify.text += "\n";
            trackedLength = 0;
            line++;
        }
        if(line == 3)
        {
            error.text = "maximum keys added";
            return;
        }

        if(trackedLength != 0) { 
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
