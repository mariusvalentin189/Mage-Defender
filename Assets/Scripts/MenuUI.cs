using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUI : Singleton<MenuUI>
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject selectLevelPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject shopMainPanel;
    [SerializeField] GameObject towersPanel;
    [SerializeField] GameObject upgradesPanel;
    [SerializeField] GameObject towerUpgradesPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] LoadingBar loadingPanel;
    [SerializeField] GameObject shopBackButton;
    [SerializeField] Animator anim;
    AudioManager am;
    void Awake()
    {
        am = AudioManager.Instance;
    }
    #region Buttons functions
    public void StartGame()
    {
        anim.SetTrigger("CloseMenu");
        anim.SetTrigger("OpenLevelSelect");
    }
    public void OpenShop()
    {
        anim.SetTrigger("CloseMenu");
        anim.SetTrigger("OpenShop");
    }
    public void OpenSettings()
    {
        anim.SetTrigger("CloseMenu");
        anim.SetTrigger("OpenSettings");
    }
    public void OpenTowerUpgrades()
    {
        anim.ResetTrigger("CloseTowerUpgrades");
        anim.SetTrigger("OpenTowerUpgrades");
    }
    public void OpenUpgrades()
    {
        anim.ResetTrigger("CloseUpgrades");
        anim.SetTrigger("OpenUpgrades");
        Shop.Instance.CheckUpgradeButtons();
    }
    public void OpenTowers()
    {
        anim.ResetTrigger("CloseTowers");
        anim.SetTrigger("OpenTowers");
    }
    public void BackToMenuFromSelectLevel()
    {
        anim.SetTrigger("CloseLevelSelect");
        anim.ResetTrigger("CloseMenu");
        anim.ResetTrigger("OpenLevelSelect");
    }
    public void BackToMenuFromShop()
    {
        anim.SetTrigger("CloseShop");
        anim.ResetTrigger("CloseMenu");
        anim.ResetTrigger("OpenShop");
    }
    public void BackToMenuFromSettings()
    {
        anim.SetTrigger("CloseSettings");
        anim.ResetTrigger("CloseMenu");
        anim.ResetTrigger("OpenSettings");
    }
    public void BackToShopFromTowerUpgrades()
    {
        anim.SetTrigger("CloseTowerUpgrades");
        anim.ResetTrigger("OpenTowerUpgrades");
    }
    public void BackToShopFromTowers()
    {
        anim.SetTrigger("CloseTowers");
        anim.ResetTrigger("OpenTowers");
    }
    public void BackToShopFromUpgrades()
    {
        anim.SetTrigger("CloseUpgrades");
        anim.ResetTrigger("OpenUpgrades");
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SoundVolume, SettingsManager.soundVolume);
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, SettingsManager.musicVolume);
        PlayerPrefs.SetInt(PlayerPrefsKeys.Width, SettingsManager.screenWidth);
        PlayerPrefs.SetInt(PlayerPrefsKeys.Height, SettingsManager.screenHeight);
        if(SettingsManager.fullScreen)
            PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 1);
        else PlayerPrefs.SetInt(PlayerPrefsKeys.FullScreen, 0);
        PlayButtonSound();
        Shop.Instance.LoadCoins();
        LevelsManager.Instance.LoadCompletedLevels();
        LevelsManager.Instance.DisableResetButton();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayButtonSound()
    {
        am.PlayButtonClickSound();
    }
    #endregion

    #region Animation events
    public void HandleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
    public void HandleSelectLevel()
    {
        selectLevelPanel.SetActive(!selectLevelPanel.activeSelf);
    }
    public void HandleUpgrades()
    {
        upgradesPanel.SetActive(!upgradesPanel.activeSelf);
        shopMainPanel.SetActive(!shopMainPanel.activeSelf);
    }
    public void HandleTowers()
    {
        towersPanel.SetActive(!towersPanel.activeSelf);
        shopMainPanel.SetActive(!shopMainPanel.activeSelf);
    }
    public void HandleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    public void HandleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
    public void HandleTowerUpgrades()
    {
        towerUpgradesPanel.SetActive(!towerUpgradesPanel.activeSelf);
        if (towerUpgradesPanel.activeSelf)
            shopBackButton.SetActive(false);
        else shopBackButton.SetActive(true);
    }
    #endregion


    public void LoadLevel(int id)
    {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Operation = SceneManager.LoadSceneAsync(id);
        loadingPanel.LevelName = "Level" + id;
    }
}
