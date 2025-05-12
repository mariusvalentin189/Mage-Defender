using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpeedUpButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] int speed;
    [SerializeField] Image outline;
    [SerializeField] Image background;
    [SerializeField] Color selectedBgColor;
    [SerializeField] Color selectedOutlineColor;

    Color defaultBgColor, defaultOutlineColor;
    bool selected;
    GameManager gm;
    AudioManager am;
    public bool Selected
    {
        get { return selected; }
    }
    private void Awake()
    {
        defaultBgColor = background.color;
        defaultOutlineColor = outline.color;
        gm = GameManager.Instance;
        am = AudioManager.Instance;
    }
    void SetGameSpeed()
    {
        if (!gm.IsPaused)
        {
            gm.SetGameSpeed(speed);
            if (gm.SelectedSpeedButton == this)
            {
                Deselect();
            }
            else
            {
                Select();
                gm.DeselectSpeedButtons();
            }
        }
    }
    void Select()
    {
        background.color = selectedBgColor;
        outline.color = selectedOutlineColor;
        selected = true;
        gm.SelectedSpeedButton = this;
    }
    public void Deselect()
    {
        background.color = defaultBgColor;
        outline.color = defaultOutlineColor;
        selected = false;
        if(gm.SelectedSpeedButton == this)
            gm.SelectedSpeedButton = null;
    }
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
        SetGameSpeed();
        am.PlayButtonClickSound();
    }
}
