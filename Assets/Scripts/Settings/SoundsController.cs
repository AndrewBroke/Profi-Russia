using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void ChangeVolume()
    {
        int k = PlayerPrefs.GetInt("Volume");
        if(k == 0)
        {
            audioMixer.SetFloat("MasterVolume", 20f);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", -80f);
        }
    }
}
