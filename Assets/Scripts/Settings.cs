using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour
{
    [Header("Volume")]
    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;
    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    [Header("Controls")]
    Resolution[] resolutions;
    int selectedResolutionIndex;
    float soundVolume, musicVolume;
    bool fullScreen;
    int savedIndex;
    AudioManager audioManager;
    private void Awake()
    {
        SettingsManager.LoadSettings();
        audioManager = AudioManager.Instance;
        soundVolume = SettingsManager.soundVolume;
        musicVolume = SettingsManager.musicVolume;
        audioManager.SetSoundVolume(soundVolume);
        audioManager.SetMusicVolume(musicVolume);
        soundVolumeSlider.value = SettingsManager.soundVolume;
        musicVolumeSlider.value = SettingsManager.musicVolume;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int maxResolutionHz = 0;
        for (int i = 0; i < resolutions.Length / 2; i++)
        {
            if (resolutions[i].refreshRate > maxResolutionHz)
                maxResolutionHz = resolutions[i].refreshRate;
        }
        List<string> resolutionsList = new List<string>();
        int currentResolutionIndex = 0;
        for(int i=0;i<resolutions.Length;i++)
        {
            if (resolutions[i].width == SettingsManager.screenWidth && resolutions[i].height == SettingsManager.screenHeight)
                currentResolutionIndex = i;
            if (resolutions[i].refreshRate == maxResolutionHz)
            {
                string res = resolutions[i].width + " x " + resolutions[i].height;
                resolutionsList.Add(res);
            }
        }
        resolutionDropdown.AddOptions(resolutionsList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullScreenToggle.isOn = SettingsManager.fullScreen;
        savedIndex = currentResolutionIndex;
    }
    public void SetSoundVolume(float value)
    {
        soundVolume = value;
        audioManager.SetSoundVolume(soundVolume);
        SettingsManager.SetSoundVolume(soundVolume);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioManager.SetMusicVolume(musicVolume);
        SettingsManager.SetMusicVolume(musicVolume);
    }
    public void SetFullScreen(bool value)
    {
        fullScreen = value;
        SettingsManager.SetFullScreen(fullScreen);
    }
    public void SetResolution(int index)
    {
        selectedResolutionIndex = index;
        SettingsManager.SetResolution(selectedResolutionIndex);
    }
}
