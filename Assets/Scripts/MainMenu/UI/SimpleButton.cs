using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour, IClickable
{
    Button button;
    public AudioSource clickSound;
    public virtual void OnClick()
    {
        if(clickSound != null)
        {
            clickSound.Play();
        }
    }

    public void AddClickListener()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
}
