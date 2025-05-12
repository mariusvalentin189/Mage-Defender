using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUpgradeButton : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    [SerializeField] Button upgradeButton;
    GameManager gm;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgradeButton.interactable)
        {
            gm.UpgradeTower();
            gm.PlayButtonSound();
            if(!upgradeButton.gameObject.activeSelf)
                gm.SetTowerInfo();
            else if (upgradeButton.interactable)
                gm.SetTowerUpgradeMenu();
            else gm.SetTowerInfo();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeButton.interactable)
        {
            gm.SetTowerUpgradeMenu();
        }
        gm.IsOverButton = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gm.SetTowerInfo();
        gm.IsOverButton = false;
    }
    void Start()
    {
        gm = GameManager.Instance;
    }
}
