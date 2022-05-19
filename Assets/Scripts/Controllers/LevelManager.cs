using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<LevelSO> levels;

    [FoldoutGroup("UI")] public CanvasGroup levelButtonsCanvas;
    [FoldoutGroup("UI")] public Transform levelButtonsParent;
    [FoldoutGroup("UI")] public GameObject levelButtonPrefab;

    public LevelSO selectedLevel;
    List<LevelButton> _levelButtons;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        LoadLevelDataFromPlayerPrefs();
        _levelButtons = new List<LevelButton>();

        levelButtonsCanvas.alpha = 0;
        levelButtonsCanvas.blocksRaycasts = false;
        levelButtonsCanvas.interactable = false;
    }
    private void LoadLevelDataFromPlayerPrefs()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].key + "_Passed"))
            {
                levels[i].levelData.passed = PlayerPrefs.GetInt(levels[i].key + "_Passed") == 1 ? true : false;
                levels[i].levelData.time = PlayerPrefs.GetFloat(levels[i].key + "_Time");
                if (DoesNextLevelExist(levels[i]))
                {
                    levels[i + 1].levelData.unlocked = true;
                }
            }
            else
            {
                levels[i].levelData.passed = false;
                levels[i].levelData.time = 0;
            }
        }
        levels[0].levelData.unlocked = true;
    }

    #region Public
    [Button]
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].levelData.unlocked = false;
            levels[i].levelData.passed = false;
            levels[i].levelData.time = 0;

            levels[0].levelData.unlocked = true;
        }

        if (levelButtonsCanvas.alpha > 0)
        {
            CreateLevelButtons();
        }
    }
#if UNITY_EDITOR
    [Button]
    public void GetAllLevels()
    {
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/LevelData" });
        levels.Clear();
        levels = new List<LevelSO>();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<LevelSO>(SOpath);
            levels.Add(character);
        }
    }
#endif
    public void LevelPassed(float time)
    {
        selectedLevel.levelData.passed = true;
        PlayerPrefs.SetInt(selectedLevel.key + "_Passed", 1);

        if (time < selectedLevel.levelData.time || selectedLevel.levelData.time == 0f)
        {
            selectedLevel.levelData.time = time;
            PlayerPrefs.SetFloat(selectedLevel.key + "_Time", time);
        }

        LevelSO level = GetNextLevel();
        if (level != null)
        {
            level.levelData.unlocked = true;
            PlayerPrefs.SetInt(level.key + "_Unlocked", 1);
        }
    }
    public void GoToNextLevel()
    {
        if (DoesNextLevelExist())
        {
            CloseLevelSelection();
            selectedLevel = GetNextLevel();
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            CloseLevelSelection();
            SceneManager.LoadScene("MainMenu");
        }
    }
    public bool DoesNextLevelExist(LevelSO level = null)
    {
        if (level == null) { level = selectedLevel; }
        if (levels.Contains(level))
        {
            if (levels.IndexOf(level) < levels.Count - 1)
            {
                return true;
            }
        }
        return false;
    }
    public LevelSO GetNextLevel()
    {
        if (DoesNextLevelExist())
        {
            return levels[levels.IndexOf(selectedLevel) + 1];
        }
        return null;
    }
    public void SpawnLevel()
    {
        Instantiate(selectedLevel.levelData.mapPrefab);
    }
    public void SelectLevel(LevelSO level)
    {
        CloseLevelSelection();
        selectedLevel = level;
        SceneManager.LoadScene("GameScene");
    }
    public void OpenLevelSelection()
    {
        LoadLevelDataFromPlayerPrefs();

        levelButtonsCanvas.alpha = 1;
        levelButtonsCanvas.blocksRaycasts = true;
        levelButtonsCanvas.interactable = true;

        CreateLevelButtons();
    }
    public void CloseLevelSelection()
    {
        levelButtonsCanvas.alpha = 0;
        levelButtonsCanvas.blocksRaycasts = false;
        levelButtonsCanvas.interactable = false;

        DeleteLevelButtons();
    }
    #endregion 

    #region UI
    public void CreateLevelButtons()
    {
        DeleteLevelButtons();
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelData.unlocked)
            {
                GameObject obj = Instantiate(levelButtonPrefab, levelButtonsParent);
                obj.GetComponent<LevelButton>().InitializeButton(levels[i]);
                _levelButtons.Add(obj.GetComponent<LevelButton>());
            }
            else if (levels[i - 1].levelData.unlocked)
            {
                GameObject obj = Instantiate(levelButtonPrefab, levelButtonsParent);
                obj.GetComponent<LevelButton>().InitializeButton(levels[i]);
                _levelButtons.Add(obj.GetComponent<LevelButton>());
            }
        }
    }
    public void DeleteLevelButtons()
    {
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            Destroy(_levelButtons[i].gameObject);
        }
        _levelButtons = new List<LevelButton>();
    }
    #endregion
}
[System.Serializable]
public class LevelData
{
    public string description;
    [ValueDropdown("values")] public int difficulty = 1;
    public bool passed;
    public bool unlocked;
    public float time;
    public GameObject mapPrefab;

    private int[] values = new int[] { 1, 2, 3, 4, 5 };
}