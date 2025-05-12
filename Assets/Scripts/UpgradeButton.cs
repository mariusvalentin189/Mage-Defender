using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] TMP_Text valueText;
    [SerializeField] TMP_Text buyText;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] int[] unlockCost;
    [SerializeField] Button button;
    [SerializeField] float value;
    [SerializeField] GameObject infoPanel;
    [SerializeField] TMP_Text infoText;
    int currentUpgrade = 0;
    float currentValue;
    string textValue;
    void Awake()
    {
        CheckUnlockedUpgrades();
        CheckCost();
        UpdateInfoText();
        HideInfoPanel();
    }
    public void CheckUnlockedUpgrades()
    {
        if (upgradeType == UpgradeType.startingCoins)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.startingCurrencyUpgrade))
            {
                currentUpgrade = PlayerPrefs.GetInt(PlayerPrefsKeys.startingCurrencyUpgrade);
                currentValue = PlayerPrefs.GetInt(PlayerPrefsKeys.startingCurrency);
            }
            else
            {
                currentValue = 100;
                currentUpgrade = 0;
            }
            textValue = "Starting coins: ";
        }
        else if (upgradeType == UpgradeType.startingLives)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.startingLivesUpgrade))
            {
                currentUpgrade = PlayerPrefs.GetInt(PlayerPrefsKeys.startingLivesUpgrade);
                currentValue = PlayerPrefs.GetInt(PlayerPrefsKeys.startingLives);
            }
            else
            {
                currentValue = 10;
                currentUpgrade = 0;
            }
            textValue = "Starting lives: ";
        }
        else if (upgradeType == UpgradeType.towersBuildTime)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.towersBuildTimeUpgrade))
            {
                currentUpgrade = PlayerPrefs.GetInt(PlayerPrefsKeys.towersBuildTimeUpgrade);
                currentValue = PlayerPrefs.GetFloat(PlayerPrefsKeys.towersBuildTime);
            }
            else
            {
                currentValue = 3;
                currentUpgrade = 0;
            }
            textValue = "Towers build time: ";
        }
        UpdateText();
        for (int i = 0; i < currentUpgrade; i++)
            upgradeIcons[i].SetActive(true);
    }
    public void UnlockUpgrade()
    {
        if (currentUpgrade < upgradeIcons.Length)
        {
            upgradeIcons[currentUpgrade].SetActive(true);
            Shop.Instance.Coins -= unlockCost[currentUpgrade];
            currentValue += value;
            currentValue = Mathf.Round(currentValue * 10) * 0.1f;
            currentUpgrade++;
            switch (upgradeType)
            {
                case UpgradeType.startingCoins:
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.startingCurrencyUpgrade, currentUpgrade);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.startingCurrency, (int)currentValue);
                        break;
                    }
                case UpgradeType.startingLives:
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.startingLivesUpgrade, currentUpgrade);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.startingLives, (int)currentValue);
                        break;
                    }
                case UpgradeType.towersBuildTime:
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.towersBuildTimeUpgrade, currentUpgrade);
                        PlayerPrefs.SetFloat(PlayerPrefsKeys.towersBuildTime, currentValue);
                        break;
                    }
            }
            UpdateText();
            UpdateInfoText();
            Shop.Instance.CheckUpgradeButtons();
            MenuUI.Instance.PlayButtonSound();
        }
    }
    void UpdateText()
    {
        valueText.text = textValue + currentValue;
        switch (upgradeType)
        {
            case UpgradeType.towersBuildTime:
                {
                    valueText.text += "s";
                    break;
                }
        }
        if (currentUpgrade < upgradeIcons.Length)
            buyText.text = "Buy " + unlockCost[currentUpgrade] + "$";
        else buyText.text = "Maxed";
    }
    public void CheckCost()
    {
        if (currentUpgrade < upgradeIcons.Length)
        {
            if (unlockCost[currentUpgrade] <= Shop.Instance.Coins)
                button.interactable = true;
            else
            {
                button.interactable = false; 
                if (infoPanel.activeSelf)
                    HideInfoPanel();
            }
        }
        else
        {
            button.interactable = false;
            if (infoPanel.activeSelf)
                HideInfoPanel();
        }
    }
    void ShowInfoPanel()
    {
        infoPanel.SetActive(true);
        infoPanel.transform.SetParent(Shop.Instance.transform);
    }
    void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }
    void UpdateInfoText()
    {
        if (currentUpgrade < upgradeIcons.Length)
        {
            if (upgradeType == UpgradeType.towersBuildTime)
            {
                infoText.text = currentValue + "s -> " + (currentValue + value) + "s";
            }
            else infoText.text = currentValue + " -> " + (currentValue + value);
        }
        else infoText.text = "Fully upgraded";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
            ShowInfoPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
            HideInfoPanel();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
        {
            UnlockUpgrade();
        }
    }
}
public enum UpgradeType
{
    startingCoins,
    startingLives,
    towersBuildTime
}