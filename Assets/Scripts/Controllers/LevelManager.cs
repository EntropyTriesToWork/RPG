using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<LevelSO> campaignLevels;
    public List<LevelSO> challengeLevels;

    public LevelSO selectedLevel;

    private void Awake()
    {
        if (Instance != null) { Destroy(this); }
        else { Instance = this; }

        DontDestroyOnLoad(this);
    }
    [Button]
    public void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    private void Start()
    {
        LoadLevelDataFromPlayerPrefs();
    }
    public void LevelPassed(float time)
    {
        selectedLevel.levelData.passed = true;
        selectedLevel.levelData.time = time;

        PlayerPrefs.SetInt(selectedLevel.key + "_Passed", 1);
        PlayerPrefs.SetFloat(selectedLevel.key + "_Time", time);
    }
    public void LoadLevelDataFromPlayerPrefs()
    {
        for (int i = 0; i < campaignLevels.Count; i++)
        {
            campaignLevels[i].levelData.passed = PlayerPrefs.GetInt(campaignLevels[i].key + "_Passed") == 1 ? true : false;
            campaignLevels[i].levelData.time = PlayerPrefs.GetFloat(campaignLevels[i].key + "_Time");
        }
        for (int i = 0; i < challengeLevels.Count; i++)
        {
            challengeLevels[i].levelData.passed = PlayerPrefs.GetInt(challengeLevels[i].key + "_Passed") == 1 ? true : false;
            challengeLevels[i].levelData.time = PlayerPrefs.GetFloat(challengeLevels[i].key + "_Time");
        }
    }
    public bool GetIfNextLevelExists()
    {
        if (campaignLevels.Contains(selectedLevel))
        {
            if (campaignLevels.IndexOf(selectedLevel) < campaignLevels.Count) { return true; }
            return false;
        }
        else
        {
            if (challengeLevels.IndexOf(selectedLevel) < challengeLevels.Count) { return true; }
            return false;
        }
    }
}
[System.Serializable]
public class LevelData
{
    public string description;
    [ValueDropdown("values")] public int difficulty = 1;
    public bool passed;
    public float time;
    public GameObject mapPrefab;

    private int[] values = new int[] { 1, 2, 3, 4, 5 };
}