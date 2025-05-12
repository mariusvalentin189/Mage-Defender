using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerStatsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameManager gm;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        gm.IsOverButton = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gm.IsOverButton = false;
    }

    void Start()
    {
        gm = GameManager.Instance;
    }
}
