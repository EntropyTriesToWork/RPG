using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarRequirementView : MonoBehaviour
{
    public Image checkMark, xMark, token;
    public TMPro.TMP_Text starRequirementText;

    public void UpdateToken(Sprite sprite)
    {
        token.sprite = sprite;
    }
    public void UpdateText(string text)
    {
        starRequirementText.text = text;
    }
    public void UpdateTextColor(Color color)
    {
        starRequirementText.color = color;
    }
    public void IsComplete(bool complete)
    {
        if (complete)
        {
            checkMark.enabled = true;
            xMark.enabled = false;
        }
        else
        {
            checkMark.enabled = false;
            xMark.enabled = true;
        }
    }
    public void DisableCheckXMarks()
    {
        checkMark.enabled = false;
        xMark.enabled = false;
    }
}
