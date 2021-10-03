using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;

    public List<Level> levels = new List<Level>();
    public UnityEvent onEnd = new UnityEvent();
    public UnityEvent onLevelStart = new UnityEvent();
    public UnityEvent onNewStar = new UnityEvent();

    private bool shouldUpdate = true;

    [SerializeField]private int currentLevelIndex;

    private void Awake()
    {
        instance = this;
    }

    public void SetCurrentLevel(int levelIndex)
    {
        Debug.Log(levelIndex);
        if (currentLevelIndex != levelIndex)
        {
            levels[currentLevelIndex].Cleanup();
            currentLevelIndex = levelIndex;
            levels[currentLevelIndex].StartLevel();
        }
    }

    public void OnPause()
    {
        shouldUpdate = false;
    }

    public void OnResume()
    {
        shouldUpdate = true;
    }

    public int GetCurrentLevel()
    {
        return currentLevelIndex;
    }

    void Start()
    {
        InitializeLevels();
        levels[currentLevelIndex].StartLevel();
        SaveManager.instance.Load();
    }

    private void InitializeLevels()
    {
        foreach (var level in levels)
        {
            level.InitilizeElements();
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(var level in levels)
        {
            if (level != null)
                level.DrawGizmo();
        }
    }

    void Update()
    {
        if (shouldUpdate)
            UpdateCurentLevel();   
    }

    private void UpdateCurentLevel()
    {
        Level level = levels[currentLevelIndex];

        if (level.IsAtTheEnd(PlayerManager.instance.player.transform.position))
        {
            MoveToNextLevel();
        }
        else if (!level.IsWithinLevel(PlayerManager.instance.player.transform.position))
        {
            level.ResetLevel();
        }
        
    }

    public void ResetLevel()
    {
        levels[currentLevelIndex].ResetLevel();
    }

    private void MoveToNextLevel()
    {
        levels[currentLevelIndex].OnEndLevel();
        if (currentLevelIndex == levels.Count - 1)
        {
            shouldUpdate = false;
            onEnd.Invoke();
            return;
        }
        currentLevelIndex++;
        levels[currentLevelIndex].StartLevel();
        SaveManager.instance.Save();
        onLevelStart.Invoke();
    }
}
