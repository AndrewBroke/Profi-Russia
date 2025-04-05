using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDd;
    [SerializeField] private Toggle screenModeToggle;
    [SerializeField] private Toggle musicAndSoundsToggle;
    [SerializeField] private Dropdown languageDd;

    [SerializeField] private CurrentSetitngs currentSettings;
    [SerializeField] private BasicSettigns basicSettings;

    [SerializeField] private SoundsController soundsController;

    public void Awake()
    {
        LoadSettings();
    }
    public void ChangeSettings()
    {
        // Save resulution
        switch (resolutionDd.value)
        {
            case 0:
                Screen.SetResolution(1280, 720, screenModeToggle.isOn);
                break;
            case 1:
                Screen.SetResolution(1366, 768, screenModeToggle.isOn);
                break;
            case 2:
                Screen.SetResolution(1600, 900, screenModeToggle.isOn);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, screenModeToggle.isOn);
                break;
        }

        if (musicAndSoundsToggle.isOn)
        {
            PlayerPrefs.SetInt("Volume", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Volume", 1);
        }

        soundsController.ChangeVolume();
    }
    public void SaveSettings()
    {
        ChangeSettings();
        currentSettings.resolution = resolutionDd.value;
        currentSettings.screenMode = screenModeToggle.isOn;
        currentSettings.soundsAndMusic = musicAndSoundsToggle.isOn;
        currentSettings.language = languageDd.value;
        print("Сохранено");
    }
    public void LoadSettings()
    {
        resolutionDd.value = currentSettings.resolution;
        screenModeToggle.isOn = currentSettings.screenMode;
        musicAndSoundsToggle.isOn = currentSettings.soundsAndMusic;
        languageDd.value = currentSettings.language;
        ChangeSettings();
    }
    public void ResetSettings()
    {
        resolutionDd.value = basicSettings.resolution;
        screenModeToggle.isOn = basicSettings.screenMode;
        musicAndSoundsToggle.isOn = basicSettings.soundsAndMusic;
        languageDd.value = basicSettings.language;
        ChangeSettings();
    }
}
