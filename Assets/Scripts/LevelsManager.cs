using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : Singleton<LevelsManager>
{
    [SerializeField] Button[] levelButtons;
    [SerializeField] GameObject resetProgressButton;
    public bool IsGameSaved { get; set; } = false;
    void Start()
    {
        LoadCompletedLevels();
        if (IsGameSaved)
            resetProgressButton.SetActive(true);
        else resetProgressButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadCompletedLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
        for (int i=0;i< levelButtons.Length;i++)
        {
            int c = 0;
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.Level + i))
            {
                c = PlayerPrefs.GetInt(PlayerPrefsKeys.Level + i);
                IsGameSaved = true;
            }
            if(c==0)
            {
                levelButtons[i].interactable = true;
                return;
            }
            else if(c==1)
            {
                levelButtons[i].interactable = true;
            }
        }
    }
    public void DisableResetButton()
    {
        resetProgressButton.SetActive(false);
        IsGameSaved = false;
    }
}
