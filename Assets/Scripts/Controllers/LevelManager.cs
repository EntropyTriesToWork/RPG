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

    [FoldoutGroup("UI")] public CanvasGroup levelButtonsCanvas, levelInfoCanvas;
    [FoldoutGroup("UI")] public Transform levelButtonsParent, starRequirementsParent;
    [FoldoutGroup("UI")] public GameObject levelButtonPrefab, starRequirementViewPrefab;
    [FoldoutGroup("UI")] public TMP_Text levelDescText;
    [FoldoutGroup("UI")] public Sprite[] starRequirementTokens;
    [FoldoutGroup("UI")] public Color green, red;

    public LevelSO selectedLevel;
    List<LevelButton> _levelButtons;
    List<StarRequirementView> _starRequirementViews;
    private bool _challengeModeOff = false;

    [ReadOnly] public PlayerController pc;

    #region Messages
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        DontDestroyOnLoad(this);

        if (!PlayerPrefs.HasKey(levels[0].key + "_Passed"))
        {
            ResetSaveData();
        }
        _levelButtons = new List<LevelButton>();
        _starRequirementViews = new List<StarRequirementView>();
        HideLevelInfo();
    }
    private void Start()
    {
        LoadLevelDataFromPlayerPrefs();

        levelDescText.text = "";

        levelButtonsCanvas.alpha = 0;
        levelButtonsCanvas.blocksRaycasts = false;
        levelButtonsCanvas.interactable = false;
    }
    private void Update()
    {
        if (pc != null && !_challengeModeOff)
        {
            UpdateStarRequirementUI();
        }
    }
    #endregion

    private void LoadLevelDataFromPlayerPrefs()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].key + "_Passed"))
            {
                levels[i].levelData.passed = PlayerPrefs.GetInt(levels[i].key + "_Passed") == 1 ? true : false;
                levels[i].levelData.time = PlayerPrefs.GetFloat(levels[i].key + "_Time");
                levels[i].stars = PlayerPrefs.GetInt(levels[i].key + "_Stars");
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
    private void UpdateStarRequirementUI()
    {
        if (selectedLevel.starRequirements.Count < 1) { return; }
        for (int i = 0; i < selectedLevel.starRequirements.Count; i++)
        {
            StarRequirementType type = selectedLevel.starRequirements[i].type;
            switch (type)
            {
                case StarRequirementType.Time:
                    if (selectedLevel.starRequirements[i].time >= Mathf.FloorToInt(GameManager.Instance.GameTime)) { _starRequirementViews[i].UpdateTextColor(green); }
                    else { _starRequirementViews[i].UpdateTextColor(red); }
                    break;
                case StarRequirementType.Hitless:
                    if (pc.lastTimeTookDamage == 0) { _starRequirementViews[i].IsComplete(true); }
                    else { _starRequirementViews[i].IsComplete(false); }
                    break;
                case StarRequirementType.FullHealth:
                    if (pc.GetComponent<HealthComponent>().FullOnHealth) { _starRequirementViews[i].IsComplete(true); }
                    else { _starRequirementViews[i].IsComplete(false); }
                    break;
                case StarRequirementType.NoDashing:
                    if (pc.totalDashesMade <= 0) { _starRequirementViews[i].IsComplete(true); }
                    else { _starRequirementViews[i].IsComplete(false); }
                    break;
                case StarRequirementType.NoJumping:
                    if (pc.totalJumpsMade <= 0) { _starRequirementViews[i].IsComplete(true); }
                    else { _starRequirementViews[i].IsComplete(false); }
                    break;
                case StarRequirementType.NoKilling:
                    if (GameManager.Instance.enemiesKilled <= 0) { _starRequirementViews[i].IsComplete(true); }
                    else { _starRequirementViews[i].IsComplete(false); }
                    break;
                case StarRequirementType.KillAmount:
                    _starRequirementViews[i].UpdateText(GameManager.Instance.enemiesKilled + "/" + selectedLevel.starRequirements[i].kills);
                    if (GameManager.Instance.enemiesKilled >= selectedLevel.starRequirements[i].kills) { _starRequirementViews[i].UpdateTextColor(green); }
                    else { _starRequirementViews[i].UpdateTextColor(red); }
                    break;
                case StarRequirementType.FruitCollected:
                    _starRequirementViews[i].UpdateText(pc.totalFruitCollected + "/" + selectedLevel.starRequirements[i].fruitCollected);
                    if (pc.totalFruitCollected >= selectedLevel.starRequirements[i].fruitCollected) { _starRequirementViews[i].UpdateTextColor(green); }
                    else { _starRequirementViews[i].UpdateTextColor(red); }
                    break;
                default:
                    break;
            }
        }

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
            levels[i].stars = 0;
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

        int totalStars = 0;

        for (int i = 0; i < selectedLevel.starRequirements.Count; i++)
        {
            StarRequirementType type = selectedLevel.starRequirements[i].type;
            PlayerController pc = FindObjectOfType<PlayerController>();

            switch (type)
            {
                case StarRequirementType.Time:
                    if (selectedLevel.starRequirements[i].time >= Mathf.FloorToInt(time)) { totalStars++; }
                    break;
                case StarRequirementType.Hitless:
                    if (pc.lastTimeTookDamage == 0) { totalStars++; }
                    break;
                case StarRequirementType.FullHealth:
                    if (pc.GetComponent<HealthComponent>().FullOnHealth) { totalStars++; }
                    break;
                case StarRequirementType.NoDashing:
                    if (pc.totalDashesMade <= 0) { totalStars++; }
                    break;
                case StarRequirementType.NoJumping:
                    if (pc.totalJumpsMade <= 0) { totalStars++; }
                    break;
                case StarRequirementType.NoKilling:
                    if (GameManager.Instance.enemiesKilled <= 0) { totalStars++; }
                    break;
                case StarRequirementType.KillAmount:
                    if (GameManager.Instance.enemiesKilled >= selectedLevel.starRequirements[i].kills) { totalStars++; }
                    break;
                case StarRequirementType.FruitCollected:
                    if (pc.totalFruitCollected >= selectedLevel.starRequirements[i].fruitCollected) { totalStars++; }
                    break;
                default:
                    break;
            }
        }
        if (totalStars > selectedLevel.stars)
        {
            selectedLevel.stars = totalStars;
            PlayerPrefs.SetInt(selectedLevel.key + "_Stars", selectedLevel.stars);
        }

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
    public void ChangeToNewLevel()
    {
        _challengeModeOff = PlayerPrefs.GetInt("ChallengeMode") == 1;
        levelInfoCanvas.alpha = 1;
        ClearStarRequirementView();
        CreateStarRequirement();
        UpdateLevelDescription();
    }
    private void CreateStarRequirement()
    {
        if (_challengeModeOff) { return; }
        for (int i = 0; i < selectedLevel.starRequirements.Count; i++)
        {
            int index = (int)selectedLevel.starRequirements[i].type;
            StarRequirementView srv = Instantiate(starRequirementViewPrefab, starRequirementsParent).GetComponent<StarRequirementView>();
            _starRequirementViews.Add(srv);

            srv.UpdateToken(starRequirementTokens[index]);
            srv.DisableCheckXMarks();
            if (index == 0) { srv.UpdateText(GameManager.FormatTimeToMinutes(selectedLevel.starRequirements[i].time)); }
            else if (index == 6) { srv.UpdateText("0/" + selectedLevel.starRequirements[i].kills); }
            else if (index == 7) { srv.UpdateText("0/" + selectedLevel.starRequirements[i].fruitCollected); }
            else { srv.UpdateText(""); }
        }
    }
    private void ClearStarRequirementView()
    {
        for (int i = 0; i < _starRequirementViews.Count; i++)
        {
            Destroy(_starRequirementViews[i].gameObject);
        }
        _starRequirementViews = new List<StarRequirementView>();
    }
    public void HideLevelInfo()
    {
        levelInfoCanvas.alpha = 0;
        levelInfoCanvas.blocksRaycasts = false;
        levelInfoCanvas.interactable = false;
    }
    public void GoToNextLevel()
    {
        if (DoesNextLevelExist())
        {
            CloseLevelSelection();
            selectedLevel = GetNextLevel();
            SceneManager.LoadScene("GameScene");
            ChangeToNewLevel();
        }
        else
        {
            CloseLevelSelection();
            HideLevelDescription();
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
        UpdateLevelDescription();
        ChangeToNewLevel();
    }
    public bool HasPlayerPassedLevel(string levelKey)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].key == levelKey) { return levels[i].levelData.passed; }
        }
        return false;
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
    public void UpdateLevelDescription()
    {
        levelDescText.text = selectedLevel.levelData.description;
    }
    public void HideLevelDescription()
    {
        levelDescText.text = "";
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
    public bool passed;
    public bool unlocked;
    public float time;
    public GameObject mapPrefab;
}