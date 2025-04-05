using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayButton : SimpleButton
{
    public override void OnClick()
    {
        if(clickSound != null)
        {
            clickSound.Play();
        }
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        AddClickListener();
    }
}
