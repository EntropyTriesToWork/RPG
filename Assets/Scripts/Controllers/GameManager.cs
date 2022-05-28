using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [BoxGroup("Read Only")] [ReadOnly] public GameState gameState;
    [BoxGroup("Read Only")] [ReadOnly] [SerializeField] float _time;
    [BoxGroup("Read Only")] [ReadOnly] public int enemiesKilled;
    public float GameTime { get => _time; }

    [BoxGroup("Arena")] public bool arenaMode;

    [FoldoutGroup("Required")] [SerializeField] TMP_Text _timeText;
    [FoldoutGroup("Required")] [SerializeField] CanvasGroup _gameOverCanvas;
    [FoldoutGroup("Required")] [SerializeField] TMP_Text _gameOverTime;
    [FoldoutGroup("Required")] [SerializeField] CanvasGroup _victoryCanvas;
    [FoldoutGroup("Required")] [SerializeField] TMP_Text _victoryTime;
    [FoldoutGroup("Required")] [SerializeField] GameObject _gameHUD;

    private void Awake()
    {
        if (Instance != null) { Destroy(this); }
        else { Instance = this; }

        _gameOverCanvas.interactable = false;
        _gameOverCanvas.blocksRaycasts = false;
        _gameOverCanvas.alpha = 0;

        _victoryCanvas.interactable = false;
        _victoryCanvas.blocksRaycasts = false;
        _victoryCanvas.alpha = 0;
    }
    public void Start()
    {
        if (!arenaMode) { LevelManager.Instance.SpawnLevel(); }
        _gameHUD.SetActive(true);
    }
    private void Update()
    {
        if (gameState == GameState.Normal)
        {
            _time += Time.deltaTime;
            _timeText.text = FormatTimeToMinutes(_time);
        }
    }

    #region State
    public void NextLevel()
    {
        if (!arenaMode) { LevelManager.Instance.GoToNextLevel(); }
        else { SceneManager.LoadScene("MainMenu"); }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        LevelManager.Instance.HideLevelDescription();
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {

    }
    public void ChooseLevels()
    {
        LevelManager.Instance.OpenLevelSelection();
    }

    public void Resume()
    {
        if (gameState != GameState.Normal) { return; }

    }
    public void GameOver()
    {
        if (gameState != GameState.Normal) { return; }
        _gameHUD.SetActive(false);
        gameState = GameState.Defeat;
        _gameOverCanvas.interactable = true;
        _gameOverCanvas.blocksRaycasts = true;
        _gameOverTime.text = FormatTimeToMinutes(_time);
        StartCoroutine(GameOverMenuTurnVisible());
    }
    IEnumerator GameOverMenuTurnVisible()
    {
        while (_gameOverCanvas.alpha < 1 && gameState == GameState.Defeat)
        {
            _gameOverCanvas.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void Victory()
    {
        if (gameState != GameState.Normal) { return; }
        if (arenaMode) { return; }
        _gameHUD.SetActive(false);
        gameState = GameState.Victory;
        _victoryCanvas.interactable = true;
        _victoryCanvas.blocksRaycasts = true;
        _victoryTime.text = FormatTimeToMinutes(_time);

        LevelManager.Instance.LevelPassed(_time);

        StartCoroutine(VictoryMenuTurnVisible());
    }
    IEnumerator VictoryMenuTurnVisible()
    {
        while (_victoryCanvas.alpha < 1 && gameState == GameState.Victory)
        {
            _victoryCanvas.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Static
    public static IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    public static string FormatTimeToMinutes(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public enum GameState
    {
        Normal,
        Paused,
        Victory,
        Defeat
    }
    #endregion
}