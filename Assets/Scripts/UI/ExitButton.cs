using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : SimpleButton
{

    Button button;
    public override void OnClick()
    {
        print(Application.isEditor);
        if (Application.isEditor)
        {
            Debug.Log("Exit button clicked! Break");
            Debug.Break(); // Pauses the game in the Unity Editor
        }
        else
        {
            Debug.Log("Exit button clicked! Quit");
            Application.Quit(); // Exits the application
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
