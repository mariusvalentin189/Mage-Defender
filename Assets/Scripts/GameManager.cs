using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class Waves
{
    public float spawnDelay;
    public Monster[] monsters;
}

public class GameManager : Singleton<GameManager>
{
    public SpeedUpButton SelectedSpeedButton { get; set; }
    public Tower SelectedTower { get; private set; }

    public GameObject MouseObject { get; private set; }
    public BuyTowerButton SelectedButton { get; set; }
    public bool IsOverButton { get; set; }
    public Tower HighlightedTower { get; set; }
    public List<Monster> ActiveMonsters { get; set; } = new List<Monster>();
    public List<Tower> ActiveTowers { get; set; } = new List<Tower>();
    [SerializeField] Animator anim;
    [SerializeField] Animator gamePanelAnim;
    [Header("Waves")]
    [SerializeField] int maxTowersToBuy;
    [SerializeField] TMP_Text wavesText;
    [SerializeField] GameObject wavesButton;
    [SerializeField] TMP_Text waveTimerText;
    [SerializeField] float waveStartDelay;
    [SerializeField] int waveRushReward;
    [Header("Player")]
    [SerializeField] int lives;
    [SerializeField] int currency;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text currencyText;
    [Header("Towers")]
    [SerializeField] GameObject towerMenuPanel;
    [SerializeField] GameObject towersPanel;
    [SerializeField] GameObject towersPanelImage;
    [SerializeField] GameObject towersPanelText;
    [SerializeField] TMP_Text towerInfoText;
    [SerializeField] TMP_Text sellText;
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] Button upgradeButton;
    [Header("Gameplay")]
    [SerializeField] SpeedUpButton[] speedUpButtons;
    [Header("Level")]
    [SerializeField] GameObject finishedLevelUI;
    [SerializeField] TMP_Text finishedLevelMessage;
    [SerializeField] TMP_Text finishedLevelReward;
    [SerializeField] TMP_Text levelNameText;
    [SerializeField] GameObject finishedLevelButton;
    [Header("Settings")]
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject pauseMainPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] LoadingBar loadingPanel;

    int waveCount = 0;
    GameObject mousePositionObject;
    int boughtTowers;
    bool spawning;
    bool levelWon;
    bool levelLost;
    float waveDelay;
    bool towersPanelActive;
    AudioManager am;
    GameWaves gmw;
    int reward;
    public float TowerBuildTime { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsInSettings { get; private set; }
    float currentTimeScale;
    public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            UpdateTexts();
        }
    }
    public int Currency
    {
        get { return currency; }
        set
        {
            currency = value;
            UpdateTexts();
        }
    }
    private void Awake()
    {
        LoadData();
        am = AudioManager.Instance;
        gmw = GameWaves.Instance;
        wavesButton.SetActive(false);
        waveTimerText.gameObject.SetActive(false);
        waveDelay = waveStartDelay;
        UpdateTexts();
        wavesButton.SetActive(true);
        wavesText.text = "Start Wave " + (waveCount + 1).ToString();
        if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            levelNameText.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 1);
        }
        Destroy(levelNameText.gameObject, 2f);
    }
    private void Update()
    {
        if (WaveFinished() && !spawning && levelWon == false && lives >= 1 && waveCount>=1)
        {
            if (waveCount < gmw.waves.Length)
            {
                spawning = true;
                waveTimerText.gameObject.SetActive(true);
                SetGameSpeed(1);
                DeselectAllSpeedButtons();
                wavesButton.SetActive(true);
                wavesText.text = "Start Wave " + (waveCount + 1).ToString();
            }
            else
            {
                levelWon = true;
                Time.timeScale = 0;
                finishedLevelUI.SetActive(true);
                LevelEnd();
            }
        }
        else if (lives <= 0 && !levelLost)
        {
            levelLost = true;
            Time.timeScale = 0;
            finishedLevelUI.SetActive(true);
            LevelEnd();
        }

        if (wavesButton.activeSelf && waveCount>=1)
        {
            waveDelay -= Time.deltaTime;
            waveTimerText.text = waveDelay.ToString("n0") + " " + waveRushReward * Convert.ToInt32(waveDelay) + "$";
            if (waveDelay <= 0)
            {
                SpawnNextWave();
            }
        }

        if (SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(1))
                DeselectTower();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    #region Towers
    public void ChangeTower(Tower tower, BuyTowerButton button)
    {
        if (MouseObject != null)
            DeselectTower();
        if (HighlightedTower)
            HighlightedTower.DeselectTower();
        SelectedButton = button;
        SelectedTower = tower;
        mousePositionObject = tower.selectTowerPrefab;
        MouseObject = Instantiate(mousePositionObject);
    }
    public void BuyTower(Tower tower)
    {
        Currency -= tower.cost;
        ActiveTowers.Add(tower);
        boughtTowers++;
    }
    public void DeselectTower()
    {
        if (MouseObject != null)
        {
            HideTowerMenu();
            Destroy(MouseObject);
            SelectedButton.DeselectTower();
            SelectedButton = null;
            MouseObject = null;
            SelectedTower = null;
        }
    }
    public void ShowTowerMenu()
    {
        towerMenuPanel.SetActive(true);
        sellText.text = "+" + (HighlightedTower.cost / 2) + "$";
        sellText.color = Color.green;
        if (HighlightedTower.CheckNextUpgrade())
        {
            int u = 0;
            if (HighlightedTower.towerType == TowerType.normal)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.NormalTowerUpgrades + HighlightedTower.CurrentUpgrade))
                    u = PlayerPrefs.GetInt(PlayerPrefsKeys.NormalTowerUpgrades + HighlightedTower.CurrentUpgrade);
            }
            else if (HighlightedTower.towerType == TowerType.fire)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.FireTowerUpgrades + HighlightedTower.CurrentUpgrade))
                    u = PlayerPrefs.GetInt(PlayerPrefsKeys.FireTowerUpgrades + HighlightedTower.CurrentUpgrade);
            }
            else if (HighlightedTower.towerType == TowerType.ice)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.IceTowerUpgrades + HighlightedTower.CurrentUpgrade))
                    u = PlayerPrefs.GetInt(PlayerPrefsKeys.IceTowerUpgrades + HighlightedTower.CurrentUpgrade);
            }
            else if (HighlightedTower.towerType == TowerType.toxin)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.ToxinTowerUpgrades + HighlightedTower.CurrentUpgrade))
                    u = PlayerPrefs.GetInt(PlayerPrefsKeys.ToxinTowerUpgrades + HighlightedTower.CurrentUpgrade);
            }
            else if (HighlightedTower.towerType == TowerType.thunder)
            {
                if (PlayerPrefs.HasKey(PlayerPrefsKeys.ThunderTowerUpgrades + HighlightedTower.CurrentUpgrade))
                    u = PlayerPrefs.GetInt(PlayerPrefsKeys.ThunderTowerUpgrades + HighlightedTower.CurrentUpgrade);
            }
            if (Currency >= HighlightedTower.GetNextUpgrade())
            {           
                upgradeButton.gameObject.SetActive(true);
                if (u == 1)
                {
                    upgradeButton.interactable = true;
                    upgradeText.color = Color.red;
                    upgradeText.text = "-" + HighlightedTower.GetNextUpgrade() + "$";
                }
                else
                {
                    upgradeButton.interactable = false;
                    upgradeText.color = Color.black;
                    upgradeText.text = "X";
                }
            }
            else if(u==1)
            {
                upgradeButton.interactable = false;
                upgradeText.text = "-" + HighlightedTower.GetNextUpgrade() + "$";
                upgradeText.color = Color.black;
            }
            else
            {
                upgradeButton.interactable = false;
                upgradeText.color = Color.black;
                upgradeText.text = "X";
            }
        }
        else
        {
            upgradeButton.gameObject.SetActive(false);
            IsOverButton = false;
        }
        SetTowerInfo();
    }
    public void SetTowerUpgradeMenu()
    {
        switch (HighlightedTower.towerType)
        {
            case TowerType.normal:
                {
                    towerInfoText.color = Color.black;
                    break;
                }
            case TowerType.fire:
                {
                    towerInfoText.color = Color.red;
                    break;
                }
            case TowerType.ice:
                {
                    towerInfoText.color = Color.blue;
                    break;
                }
            case TowerType.toxin:
                {
                    towerInfoText.color = Color.green;
                    break;
                }
            case TowerType.thunder:
                {
                    towerInfoText.color = Color.magenta;
                    break;
                }
        }
        towerInfoText.text = HighlightedTower.SetTowerUpgradeInfo();
    }
    public void SetTowerInfo()
    {
        switch (HighlightedTower.towerType)
        {
            case TowerType.normal:
                {
                    towerInfoText.color = Color.black;
                    break;
                }
            case TowerType.fire:
                {
                    towerInfoText.color = Color.red;
                    break;
                }
            case TowerType.ice:
                {
                    towerInfoText.color = Color.blue;
                    break;
                }
            case TowerType.toxin:
                {
                    towerInfoText.color = Color.green;
                    break;
                }
            case TowerType.thunder:
                {
                    towerInfoText.color = Color.magenta;
                    break;
                }
        }
        towerInfoText.text = HighlightedTower.SetTowerInfo();
    }
    public void HideTowerMenu()
    {
        HighlightedTower = null;
        towerMenuPanel.SetActive(false);
        boughtTowers = 0;
    }
    public void UpdateTexts()
    {
        currencyText.text =currency.ToString();
        livesText.text = lives.ToString();

        if (towerMenuPanel.activeSelf)
            ShowTowerMenu();
    }
    public void SellTower()
    {
        Currency += HighlightedTower.cost / 2;
        Destroy(HighlightedTower.gameObject);
        HighlightedTower = null;
        HideTowerMenu();
        IsOverButton = false;
    }
    public void UpgradeTower()
    {
        HighlightedTower.UpgradeTower();
        ShowTowerMenu();
        if (HighlightedTower.CheckNextUpgrade())
            SetTowerUpgradeMenu();
    }
    public bool CanBuyTower()
    {
        return (boughtTowers < maxTowersToBuy);
    }
    public void HandleTowersPanel()
    {
        if (!IsPaused)
        {
            towersPanelActive = !towersPanelActive;
            if (towersPanelActive)
            {
                gamePanelAnim.SetTrigger("OpenTowersPanel");
                towersPanelImage.SetActive(false);
                towersPanelText.SetActive(true);
            }
            else
            {
                gamePanelAnim.SetTrigger("CloseTowersPanel");
                towersPanelImage.SetActive(true);
                towersPanelText.SetActive(false);
            }
        }
    }
    #endregion

    #region Gameplay
    IEnumerator SpawnMonsters()
    {
        for (int j = 0; j < gmw.waves[waveCount].monsters.Length; j++)
        {
            Monster m = Instantiate(gmw.waves[waveCount].monsters[j], gmw.spawnPosition.position, Quaternion.identity);
            ActiveMonsters.Add(m);
            yield return new WaitForSeconds(gmw.waves[waveCount].spawnDelay);
        }
        waveCount++;
        spawning = false;
        yield return null;
    }
    public void RemoveMonster(Monster monster, float time)
    {
        if (ActiveMonsters.Contains(monster))
        {
            foreach (Tower tower in ActiveTowers)
                tower.RemoveTarget(monster);
            ActiveMonsters.Remove(monster);
            Destroy(monster.gameObject, time);
        }
    }
    public bool WaveFinished()
    {
        return (ActiveMonsters.Count == 0);
    }
    public void SpawnNextWave()
    {
        if (!IsPaused)
        {
            if (waveCount>=1)
            {
                Currency += waveRushReward * Convert.ToInt32(waveDelay);
            }
            waveTimerText.gameObject.SetActive(false);
            StartCoroutine(SpawnMonsters());
            wavesButton.SetActive(false);
            waveDelay = waveStartDelay;
        }
    }
    public void ClickedButton()
    {
        IsOverButton = false;
    }
    void LevelEnd()
    {
        if (levelWon)
        {
            reward = (Lives * Currency) / 50;
            finishedLevelMessage.text = "Congratulations! You finished the level!";
            finishedLevelReward.text = "Reward: " + reward +" $";
            finishedLevelButton.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Next Level";
            if (SceneManager.GetActiveScene().buildIndex >= 21)
                finishedLevelButton.SetActive(false);
            PlayerPrefs.SetInt(PlayerPrefsKeys.Level + (SceneManager.GetActiveScene().buildIndex - 1).ToString(), 1);
            SaveCurrency();
        }
        else if (levelLost)
        {
            finishedLevelMessage.text = "You lost!";
            finishedLevelReward.text = "";
            finishedLevelButton.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Restart level";
        }
    }
    public void EndLevel()
    {
        Time.timeScale = 1;
        if (levelWon)
        {
            Time.timeScale = 1;
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        }
        else if (levelLost)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void SaveCurrency()
    {
        int c = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.Coins))
            c = PlayerPrefs.GetInt(PlayerPrefsKeys.Coins);
        c += reward;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Coins, c);
    }
    public void SetGameSpeed(int speed)
    {
        if (Time.timeScale != speed)
        {
            Time.timeScale = speed;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void DeselectSpeedButtons()
    {
        foreach (SpeedUpButton button in speedUpButtons)
            if (button.Selected && button != SelectedSpeedButton)
            {
                button.Deselect();
                return;
            }
    }
    public void DeselectAllSpeedButtons()
    {
        foreach (SpeedUpButton button in speedUpButtons)
            if (button.Selected)
            {
                button.Deselect();
            }
        SelectedSpeedButton = null;
    }
    #endregion

    #region Pause Menu
    void PauseGame()
    {
        if (!IsInSettings)
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                anim.SetTrigger("PauseGame");
            }
            else
            {
                anim.SetTrigger("ResumeGame");
            }
        }
        else
        {
            anim.SetTrigger("CloseSettings");
            anim.SetTrigger("OpenPauseMainPanel");
        }
    }
    public void ResumeGame()
    {
        anim.SetTrigger("ResumeGame");
    }
    public void OpenSettings()
    {
        anim.SetTrigger("OpenSettings");
        anim.SetTrigger("ClosePauseMainPanel");
    }
    public void CloseSettings()
    {
        anim.SetTrigger("CloseSettings");
        anim.SetTrigger("OpenPauseMainPanel");
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Operation=SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Operation = SceneManager.LoadSceneAsync(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Button Events
    public void PlayButtonSound()
    {
        am.PlayButtonClickSound();
    }
    #endregion

    #region Animation events
    public void HandlePauseMenuPanel()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        if (pauseMenuPanel.activeSelf)
        {
            IsPaused = true;
            currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = currentTimeScale;
            IsPaused = false;
        }
    }
    public void HandlePauseMainPanel()
    {
        pauseMainPanel.SetActive(!pauseMainPanel.activeSelf);
    }
    public void HandleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        if (settingsPanel.activeSelf)
            IsInSettings = true;
        else IsInSettings = false;
    }
    #endregion

    void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.startingCurrency))
            Currency = PlayerPrefs.GetInt(PlayerPrefsKeys.startingCurrency);
        else Currency = currency;

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.towersBuildTime))
            TowerBuildTime = PlayerPrefs.GetFloat(PlayerPrefsKeys.towersBuildTime);
        else TowerBuildTime = 3f;

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.startingLives))
            Lives = PlayerPrefs.GetInt(PlayerPrefsKeys.startingLives);
        else Lives = lives;
    }
}
