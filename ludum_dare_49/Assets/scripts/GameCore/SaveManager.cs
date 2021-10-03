using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    private class LevelSave
    {
        public int levelIndex;
        public bool isFinished;
        public bool isStarted;
        public int score;
    }

    public static SaveManager instance;

    public bool resetSave = false;


    private void Awake()
    {
        instance = this;
        if (resetSave)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void Save()
    {
        for (int i = 0; i < LevelsManager.instance.levels.Count; i++)
        {
            var level = LevelsManager.instance.levels[i];
            LevelSave save = new LevelSave();
            save.levelIndex = i;
            save.score = level.bestScore;
            save.isFinished = level.isFinished;
            save.isStarted = level.isStarted;

            string data = JsonUtility.ToJson(save);
            PlayerPrefs.SetString($"level_{i}", data);
        }
        Debug.Log("saved level data to player prefs");
        PlayerPrefs.Save();
    }

    public void Load()
    {
        for (int i = 0; i < LevelsManager.instance.levels.Count; i++)
        {
            var level = LevelsManager.instance.levels[i];
            if (level == null)
            {
                Debug.LogWarning($"level{i} is null");
                continue;
            }
            string data = PlayerPrefs.GetString($"level_{i}", "");
            if (data == "")
            {
                Debug.LogWarning($"no save data for level {i}");
                continue;
            }
            LevelSave save = JsonUtility.FromJson<LevelSave>(data);
            level.bestScore = save.score;
            level.isFinished = save.isFinished;
            level.isStarted = save.isStarted;
        }
    }


    public bool GetHasPlayedIntro()
    {
        return PlayerPrefs.GetInt("intro", 0) > 0;
    }

    public void SetPlayedIntro()
    {
        PlayerPrefs.SetInt("intro", 1);
    }

    public bool GetHasShownPowerUp()
    {
        return PlayerPrefs.GetInt("explosive", 0) > 0;
    }

    public void SetHasShownPowerUp()
    {
        PlayerPrefs.SetInt("explosive", 1);
    }
}
