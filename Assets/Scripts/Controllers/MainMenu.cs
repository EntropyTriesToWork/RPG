using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button arenaButton;
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
    }
    public void OpenLevelSelection()
    {
        LevelManager.Instance.OpenLevelSelection();
    }
    public void Arena()
    {
        SceneManager.LoadScene("Arena");
    }
}
