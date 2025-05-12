using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : Singleton<Shop>
{
    [SerializeField] ShopTowerButton[] shopButtons;
    [SerializeField] TMP_Text[] coinsText;
    [SerializeField] TowerUpgradeButton normalUpgradePrefab, fireUpgradePrefab, iceUpgradePrefab, toxinUpgradePrefab, thunderUpgradePrefab;
    [SerializeField] UpgradeButton[] upgradeButtons;
    [SerializeField] Transform upgradesPanel;
    [SerializeField] int startingCoins;
    public Tower SelectedTower { get; set; }
    int coins;
    List<TowerUpgradeButton> towerUpgradeButtons = new List<TowerUpgradeButton>();
    public int Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            foreach(TMP_Text coinsT in coinsText)
                coinsT.text = coins + "$";
            PlayerPrefs.SetInt(PlayerPrefsKeys.Coins, coins);
            CheckShopButtons();
        }
    }
    private void Awake()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Coins))
            Coins = PlayerPrefs.GetInt(PlayerPrefsKeys.Coins);
        else Coins = startingCoins;
    }
    public void SelectTower(Tower tower)
    {
        SelectedTower = tower;
        UpdateUpgrades();
    }
    public void UpdateUpgrades()
    {
        if(upgradesPanel.childCount>0)
        {
            for (int i = 0; i < upgradesPanel.childCount; i++)
            {
                upgradesPanel.GetChild(i).GetComponent<TowerUpgradeButton>().ResetParent();
                Destroy(upgradesPanel.GetChild(i).gameObject);
            }
            towerUpgradeButtons.Clear();
        }
        if(SelectedTower)
        {
            if (SelectedTower.towerType == TowerType.normal)
            {
                NormalTower tower = (NormalTower)SelectedTower;
                for (int i = 0; i < tower.upgrades.Length; i++)
                {
                    TowerUpgradeButton go = Instantiate(normalUpgradePrefab, upgradesPanel);
                    string upgradeText = "Upgrade I";
                    for (int j = 0; j < i; j++)
                    {
                        if (j == 2)
                        {
                            upgradeText = "Upgrade IV";
                        }
                        else
                        {
                            upgradeText += "I";
                        }
                    }
                    go.Initialize(tower.upgrades[i].unlockCost, i, upgradeText);
                    towerUpgradeButtons.Add(go);
                }
            }
            else if (SelectedTower.towerType == TowerType.fire)
            {
                FireTower tower = (FireTower)SelectedTower;
                for (int i = 0; i < tower.upgrades.Length; i++)
                {
                    TowerUpgradeButton go = Instantiate(fireUpgradePrefab, upgradesPanel);
                    string upgradeText = "Upgrade I";
                    for (int j = 0; j < i; j++)
                    {
                        if (j == 2)
                        {
                            upgradeText = "Upgrade IV";
                        }
                        else
                        {
                            upgradeText += "I";
                        }
                    }
                    go.Initialize(tower.upgrades[i].unlockCost, i, upgradeText);
                    towerUpgradeButtons.Add(go);
                }
            }
            else if(SelectedTower.towerType==TowerType.ice)
            {
                IceTower tower = (IceTower)SelectedTower;
                for (int i = 0; i < tower.upgrades.Length; i++)
                {
                    TowerUpgradeButton go = Instantiate(iceUpgradePrefab, upgradesPanel);
                    string upgradeText = "Upgrade I";
                    for (int j = 0; j < i; j++)
                    {
                        if (j == 2)
                        {
                            upgradeText = "Upgrade IV";
                        }
                        else
                        {
                            upgradeText += "I";
                        }
                    }
                    go.Initialize(tower.upgrades[i].unlockCost, i, upgradeText);
                    towerUpgradeButtons.Add(go);
                }
            }
            else if (SelectedTower.towerType == TowerType.toxin)
            {
                ToxinTower tower = (ToxinTower)SelectedTower;
                for (int i = 0; i < tower.upgrades.Length; i++)
                {
                    TowerUpgradeButton go = Instantiate(toxinUpgradePrefab, upgradesPanel);
                    string upgradeText = "Upgrade I";
                    for (int j = 0; j < i; j++)
                    {
                        if (j == 2)
                        {
                            upgradeText = "Upgrade IV";
                        }
                        else
                        {
                            upgradeText += "I";
                        }
                    }
                    go.Initialize(tower.upgrades[i].unlockCost, i, upgradeText);
                    towerUpgradeButtons.Add(go);
                }
            }
            else if (SelectedTower.towerType == TowerType.thunder)
            {
                ThunderTower tower = (ThunderTower)SelectedTower;
                for (int i = 0; i < tower.upgrades.Length; i++)
                {
                    TowerUpgradeButton go = Instantiate(thunderUpgradePrefab, upgradesPanel);
                    string upgradeText = "Upgrade I";
                    for (int j = 0; j < i; j++)
                    {
                        if (j == 2)
                        {
                            upgradeText = "Upgrade IV";
                        }
                        else
                        {
                            upgradeText += "I";
                        }
                    }
                    go.Initialize(tower.upgrades[i].unlockCost, i, upgradeText);
                    towerUpgradeButtons.Add(go);
                }
            }
            foreach (TowerUpgradeButton ub in towerUpgradeButtons)
            {
                if (ub.Unlocked==false)
                {
                    if(ub.UpgradePrice <= Shop.Instance.Coins)
                        ub.Button.interactable = true;
                    break;
                }
            }
        }
    }
    public void LoadCoins()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Coins))
            Coins = PlayerPrefs.GetInt(PlayerPrefsKeys.Coins);
        else Coins = 0;
        foreach (ShopTowerButton tower in shopButtons)
            tower.CheckLocked();
    }
    public void CheckShopButtons()
    {
        foreach (ShopTowerButton tower in shopButtons)
            tower.UpdateButton();
    }
    public void CheckUpgradeButtons()
    {
        foreach (UpgradeButton ub in upgradeButtons)
            ub.CheckCost();
    }
}
