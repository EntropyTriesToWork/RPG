using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NumberPopup : MonoBehaviour
{
    public TMP_Text numberText;
    public Image indicatorImage;
    public float floatSpeed = 3f;

    public void Initialize(string number, Color color, Sprite indicator)
    {
        indicatorImage.gameObject.SetActive(false);

        numberText.text = number;
        numberText.color = color;
        if (indicator != null)
        {
            indicatorImage.gameObject.SetActive(true);
            indicatorImage.sprite = indicator;
        }
        StartCoroutine(GameManager.DelayedAction(0.5f, () => gameObject.SetActive(false)));
    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }
}
