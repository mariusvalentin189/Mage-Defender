using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuyTowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Button button;
    [SerializeField] Image outline;
    [SerializeField] Color selectedColorOutline;
    [SerializeField] Color selectedColorBg;
    [SerializeField] Image background;
    [SerializeField] Tower tower;
    [SerializeField] TMP_Text costText;

    Color defaultColorOutline, defaultColorBg;
    bool selected;
    bool unlocked;
    GameManager gm;
    AudioManager am;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        gm.IsOverButton = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gm.IsOverButton = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked)
        {
            Buy();
            am.PlayButtonClickSound();
        }
    }
    private void Start()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;
        defaultColorBg = background.color;
        defaultColorOutline = outline.color;
        int u = 0;
        if (tower.towerType == TowerType.fire)
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
        else if (tower.towerType == TowerType.normal)
        {
            u = 1;
        }
        if (u == 0)
        {
            unlocked = false;
            costText.text = "Locked";
            button.interactable = false;
        }
        else
        {
            unlocked = true;
            costText.text = tower.cost + "$";
            button.interactable = true;
        }
    }
    private void Update()
    {
        if (unlocked == true)
        {
            if (tower.cost > gm.Currency)
            {
                button.interactable = false;
                if (selected)
                {
                    gm.DeselectTower();
                }
                DeselectTower();
            }
            else button.interactable = true;
        }
    }
    void Buy()
    {
        if (!selected)
        {
            gm.ChangeTower(tower,this);
            SelectTower();
        }
        else
        {
            gm.DeselectTower();
            DeselectTower();
        }
    }
    void SelectTower()
    {
        outline.color = selectedColorOutline;
        background.color = selectedColorBg;
        selected = true;
    }
    public void DeselectTower()
    {
        outline.color = defaultColorOutline;
        background.color = defaultColorBg;
        selected = false;
    }
}
