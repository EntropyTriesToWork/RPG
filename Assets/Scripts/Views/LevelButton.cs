using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public TMP_Text levelNumber;
    public TMP_Text description;
    public TMP_Text time;

    public LevelSO levelButton;

    public Color D1, D2, D3, D4, D5;

    Button _button;

    public void InitializeButton(LevelSO levelData)
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        levelButton = levelData;
        if (levelButton.levelData.unlocked)
        {
            _button.interactable = true;
            levelNumber.text = levelData.key;
            description.text = levelData.levelData.description;
            if (levelData.levelData.time == 0)
            {
                time.text = "--:--";
            }
            else
            {
                time.text = GameManager.FormatTimeToMinutes(levelData.levelData.time);
            }

            switch (levelData.levelData.difficulty)
            {
                case 1:
                    GetComponent<Image>().color = D1;
                    break;
                case 2:
                    GetComponent<Image>().color = D2;
                    break;
                case 3:
                    GetComponent<Image>().color = D3;
                    break;
                case 4:
                    GetComponent<Image>().color = D4;
                    break;
                case 5:
                    GetComponent<Image>().color = D5;
                    break;
                default:
                    break;
            }
            _button.onClick.AddListener(() => LevelManager.Instance.SelectLevel(levelData));
        }
        else
        {
            _button.interactable = false;

            levelNumber.text = levelData.key;
            description.text = "???";
            time.text = "??:??";
        }
    }
    private void OnDestroy()
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
    }
}
