using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTowerButton : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] TMP_Text priceText;
    [SerializeField] Button button;
    [SerializeField] GameObject upgradesButton;
    bool unlocked = false;
    int price;
    void Start()
    {
        price = tower.unlockCost;
        priceText.text = price + "$";
        CheckLocked();
        UpdateButton();
    }
    public void UnlockTower()
    {
        if (!unlocked)
        {
            if (tower.towerType == TowerType.fire)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.FireTower, 1);
            }
            else if (tower.towerType == TowerType.ice)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.IceTower, 1);
            }
            else if (tower.towerType == TowerType.toxin)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.ToxinTower, 1);
            }
            else if (tower.towerType == TowerType.thunder)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.ThunderTower, 1);
            }
            unlocked = true;
            priceText.gameObject.SetActive(false);
            upgradesButton.SetActive(true);
            button.interactable = false;
            Shop.Instance.Coins -= price;
        } 
    }
    public void CheckLocked()
    {
        int u = 0;
        if (tower.towerType == TowerType.normal)
        {
            u = 1;
        }
        else if (tower.towerType == TowerType.fire)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.FireTower))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.FireTower);
        }
        else if (tower.towerType == TowerType.ice)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.IceTower))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.IceTower);
        }
        else if (tower.towerType == TowerType.toxin)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.ToxinTower))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.ToxinTower);
        }
        else if (tower.towerType == TowerType.thunder)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.ThunderTower))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.ThunderTower);
        }
        if (u == 0)
        {
            unlocked = false;
            priceText.gameObject.SetActive(true);
            upgradesButton.SetActive(false);
        }
        else
        {
            unlocked = true;
            priceText.gameObject.SetActive(false);
            upgradesButton.SetActive(true);
            button.interactable = false;
        }
    }
    
    
    
    public void UpgradeTower()
    {
        if (unlocked)
        {
            MenuUI.Instance.OpenTowerUpgrades();
            Shop.Instance.SelectTower(tower);
        }
    }
    public void UpdateButton()
    {
        if (!unlocked)
        {
            if (price <= Shop.Instance.Coins)
                button.interactable = true;
            else button.interactable = false;
        }

    }
}
