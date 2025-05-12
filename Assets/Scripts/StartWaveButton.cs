using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartWaveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    GameManager gm;
    AudioManager am;
    void Awake()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        gm.SpawnNextWave();
        am.PlayButtonClickSound();
        gm.IsOverButton = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gm.IsOverButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gm.IsOverButton = false;
    }
}
