using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : SimpleButton
{
    public override void OnClick()
    {
        if(clickSound != null)
        {
            clickSound.Play();
        }
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
        #else
            Application.Quit();
        #endif
    }

    private void Start()
    {
        AddClickListener();
    }
}
