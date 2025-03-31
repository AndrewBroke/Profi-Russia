using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipHandler : MonoBehaviour
{
    public Text tipText;
    string message;

    public void ShowToolTip(string newMessage)
    {
        print("Here");
        message = newMessage;
        StartCoroutine("ShowToolTipDelay");
    }

    public IEnumerator ShowToolTipDelay()
    {
        yield return new WaitForSeconds(2f);
        tipText.text = message;
    }

    public void HideToolTip()
    {
        StopAllCoroutines();
        StopCoroutine("ShowToolTipDelay");
        tipText.text = "";
    }
}
