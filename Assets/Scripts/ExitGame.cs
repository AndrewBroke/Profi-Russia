using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
        #else
            Application.Quit();
        #endif
    }
}
