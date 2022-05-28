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

    public Sprite normal, bronze, silver, gold;

    Button _button;

    public void InitializeButton(LevelSO levelData)
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        levelButton = levelData;
        if (levelButton.levelData.unlocked || levelButton.levelData.passed)
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

            switch (levelData.stars)
            {
                case 0:
                    GetComponent<Image>().sprite = normal;
                    break;
                case 1:
                    GetComponent<Image>().sprite = bronze;
                    break;
                case 2:
                    GetComponent<Image>().sprite = silver;
                    break;
                case 3:
                    GetComponent<Image>().sprite = gold;
                    break;
                default:
                    GetComponent<Image>().sprite = gold;
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
