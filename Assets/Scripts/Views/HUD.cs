using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class HUD : MonoBehaviour
{
    public bool rotateToFaceCamera = true;

    public TMP_Text nameText;
    public Image healthBarFill;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;       
    }

    public void UpdateHealth(int currentHP, int maxHP)
    {
        healthBarFill.fillAmount = (float)maxHP / (float)currentHP;
    }
    void Update()
    {
        if (rotateToFaceCamera)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}