using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text levelNameText;

    public AsyncOperation Operation { get; set; }
    public string LevelName { get; set; }
    private void Start()
    {
        levelNameText.text = LevelName;
    }
    void Update()
    {
        if (loadingBar != null && Operation != null)
            loadingBar.value = Operation.progress;
    }
}
