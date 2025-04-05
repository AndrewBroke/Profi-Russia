using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorButton : SimpleButton
{
    public override void OnClick()
    {
        if(clickSound != null)
        {
            clickSound.Play();
        }
        SceneManager.LoadScene("LevelEditor");
    }

    private void Start()
    {
        AddClickListener();
    }
}
