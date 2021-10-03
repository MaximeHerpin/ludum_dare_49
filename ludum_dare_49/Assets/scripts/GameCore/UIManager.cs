using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public GameObject gameUI;
    public GameObject endUI;
    public GameObject introPanel;
    public Transform levelsPanel;
    public GameObject powerUpPanel;
    public GameObject levelButtonPrefab;
    public GameObject lockedLevelPrefab;
    public Color unclaimedStarColor;
    public Color claimedStarColor;


    private void Awake()
    {
        instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    public void OnClickStart()
    {
        Resume();
    }

    public void OnClickReset()
    {
        LevelsManager.instance.ResetLevel();
    }

    private void OnClickOnLevel(int levelIndex)
    {
        LevelsManager.instance.SetCurrentLevel(levelIndex);
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1;
        Pause();
    }

    private void Start()
    {
        Pause();
        PopulateLevelsPanel();
        LevelsManager.instance.onEnd.AddListener(DisplayEndUI);
    }

    private void ShowIntro()
    {
        introPanel.SetActive(!SaveManager.instance.GetHasPlayedIntro());
    }

    public void OnClickIntro()
    {
        introPanel.SetActive(false);
        SaveManager.instance.SetPlayedIntro();
    }

    private void Pause()
    {
        startMenu.SetActive(true);
        gameUI.SetActive(false);
        endUI.SetActive(false);
        PopulateLevelsPanel();
        Time.timeScale = 0;
        LevelsManager.instance.OnPause();
    }

    private void Resume()
    {
        startMenu.SetActive(false);
        gameUI.SetActive(true);
        endUI.SetActive(false);
        ShowIntro();
        Time.timeScale = 1;
        LevelsManager.instance.OnResume();
    }

    private void DisplayEndUI()
    {
        startMenu.SetActive(false);
        gameUI.SetActive(false);
        endUI.SetActive(true);
        Time.timeScale = .1f;
    }

    private void PopulateLevelsPanel()
    {
        foreach (Transform child in levelsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < LevelsManager.instance.levels.Count; i++)
        {
            var level = LevelsManager.instance.levels[i];
            if (level.isStarted)
            {
                var levelButton = Instantiate(levelButtonPrefab, levelsPanel);
                PopulateLevelButton(levelButton, level, i);
            }
            else
            {
                Instantiate(lockedLevelPrefab, levelsPanel);
            }
        }
    }

    public void ShowMinePowerUp()
    {
        powerUpPanel.SetActive(true);
    }

    private void PopulateLevelButton(GameObject levelButton, Level level, int levelIndex)
    {
        var button = levelButton.GetComponent<Button>();
        if (levelIndex == LevelsManager.instance.GetCurrentLevel())
        {
            button.Select();
        }
        button.onClick.AddListener(() => OnClickOnLevel(levelIndex));
        var name = levelButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        name.text = (levelIndex+1).ToString();
        for (int i = 1; i < 4; i++)
        {
            var childStar = levelButton.transform.Find($"star{i}");
            var image = childStar.GetComponent<Image>();
            image.color = level.bestScore >= i ? claimedStarColor : unclaimedStarColor;
        }
    }
}
