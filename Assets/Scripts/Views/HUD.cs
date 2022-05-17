using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class HUD : MonoBehaviour
{
    public bool rotateToFaceCamera = true;

    public TMP_Text nameText, healthText;
    public Image healthBarFill;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void UpdateHealth(float currentHP, float maxHP)
    {
        healthBarFill.fillAmount = currentHP / maxHP;
        if (healthText != null) { healthText.text = Mathf.RoundToInt(currentHP).ToString(); }

        if (currentHP > maxHP) { healthText.color = Color.green; }
        else if (currentHP < maxHP * 0.4f) { healthText.color = Color.red; }
        else { healthText.color = Color.white; }
    }
    //void Update()
    //{
    //    if (rotateToFaceCamera)
    //    {
    //        transform.LookAt(Camera.main.transform);
    //    }
    //}
}