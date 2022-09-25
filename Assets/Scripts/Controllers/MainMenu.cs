using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button arenaButton;

    public Toggle challengeModeToggle;

    private void Start()
    {
        if (LevelManager.Instance.HasPlayerPassedLevel("5"))
        {
            arenaButton.interactable = true;
        }
        else
        {
            arenaButton.interactable = false;
        }
        if (LevelManager.Instance != null) { LevelManager.Instance.HideLevelInfo(); }

        challengeModeToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("ChallengeMode") == 1);
    }
    public void OpenLevelSelection()
    {
        LevelManager.Instance.OpenLevelSelection();
    }
    public void Arena()
    {
        SceneManager.LoadScene("Arena");
    }

    public void ToggleChallengeMode(bool value)
    {
        PlayerPrefs.SetInt("ChallengeMode", value ? 1 : 0);
    }
}
