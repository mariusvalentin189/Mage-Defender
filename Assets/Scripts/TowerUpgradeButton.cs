using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public TMP_Text upgradeText;
    public TMP_Text upgradePriceText;
    [SerializeField] Tower tower;
    [SerializeField] Button button;
    [SerializeField] GameObject tooltipPanel;
    [SerializeField] TMP_Text tooltipText;
    public int UpgradePrice { get; set; }
    public int UpgradeId { get; set; }
    public bool Unlocked { get; set; }
    public Button Button { get { return button; } }
    public void Initialize(int price,int id, string upgrade)
    {
        tooltipPanel.SetActive(false);
        UpgradeId = id;
        upgradeText.text = upgrade;
        UpgradePrice = price;
        upgradePriceText.text = UpgradePrice + "$";
        SetTooltip();
        LoadData();
    }
    void LoadData()
    {
        int u = 0;
        if (tower.towerType == TowerType.normal)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.NormalTowerUpgrades + UpgradeId))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.NormalTowerUpgrades + UpgradeId);
        }
        else if (tower.towerType == TowerType.fire)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.FireTowerUpgrades + UpgradeId))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.FireTowerUpgrades + UpgradeId);
        }
        else if (tower.towerType == TowerType.ice)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.IceTowerUpgrades + UpgradeId))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.IceTowerUpgrades + UpgradeId);
        }
        else if (tower.towerType == TowerType.toxin)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.ToxinTowerUpgrades + UpgradeId))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.ToxinTowerUpgrades + UpgradeId);
        }
        else if (tower.towerType == TowerType.thunder)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.ThunderTowerUpgrades + UpgradeId))
                u = PlayerPrefs.GetInt(PlayerPrefsKeys.ThunderTowerUpgrades + UpgradeId);
        }
        if (u == 0)
        {
            Unlocked = false;
        }
        else Unlocked = true;

        UpdateButton();
    }
    public void PurchaseUpgrade()
    {
        if (UpgradePrice <= Shop.Instance.Coins)
        {
            if (tower.towerType == TowerType.normal)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.NormalTowerUpgrades + UpgradeId, 1);
            }
            else if (tower.towerType == TowerType.fire)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.FireTowerUpgrades + UpgradeId, 1);
            }
            else if (tower.towerType == TowerType.ice)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.IceTowerUpgrades + UpgradeId, 1);
            }
            else if (tower.towerType == TowerType.toxin)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.ToxinTowerUpgrades + UpgradeId, 1);
            }
            else if (tower.towerType == TowerType.thunder)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.ThunderTowerUpgrades + UpgradeId, 1);
            }
            Unlocked = true;
            Shop.Instance.Coins -= UpgradePrice;
            Shop.Instance.UpdateUpgrades();
        }
    }
    void UpdateButton()
    {
        if (UpgradePrice > Shop.Instance.Coins)
            button.interactable = false;
        if(Unlocked)
        {
            upgradePriceText.gameObject.SetActive(false);
            button.interactable = false;
        }
    }
    public void ShowTooltip()
    {
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.SetParent(transform.parent.parent);
    }
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
    public void SetTooltip()
    {
        tooltipText.text = "Unlocks tier " + (UpgradeId+1) + " upgrade in game for the following tower:\n" +tower.SetTowerUnlockUpgradeInfo(UpgradeId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
            ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPanel.activeSelf)
        {
            HideTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
        {
            PurchaseUpgrade();
            MenuUI.Instance.PlayButtonSound();
            HideTooltip();
        }
    }
    public void ResetParent()
    {
        tooltipPanel.transform.SetParent(transform);
    }
}
