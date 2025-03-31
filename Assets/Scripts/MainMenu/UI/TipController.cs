using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipController : MonoBehaviour
{
    [SerializeField] private Text tipText;
    private string message;
    public void ChangeTipText(string text)
    {
        message = text;
        StartCoroutine("Timer");
    }
    public void StopTimer()
    {
        StopCoroutine("Timer");
        tipText.text = "";
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        tipText.text = message;

    }
}
