using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPopupManager : MonoBehaviour
{
    public static NumberPopupManager Instance;

    public GameObject numberPopup;

    List<NumberPopup> _numberPopups;

    public void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        else { Instance = this; }

        _numberPopups = new List<NumberPopup>();
    }
    public NumberPopup GetNumberPopup()
    {
        for (int i = 0; i < _numberPopups.Count; i++)
        {
            if (!_numberPopups[i].gameObject.activeInHierarchy)
            {
                _numberPopups[i].gameObject.SetActive(true);
                return _numberPopups[i];
            }
        }
        NumberPopup obj = Instantiate(numberPopup, transform).GetComponent<NumberPopup>();
        _numberPopups.Add(obj);
        return obj;
    }
    public void DamageNumber(Vector2 position, int damage)
    {
        NumberPopup numberPopup = GetNumberPopup();
        numberPopup.gameObject.transform.position = position;
        numberPopup.Initialize(damage.ToString(), Color.red, null);
    }
    public void HealingNumber(Vector2 position, int heal)
    {
        NumberPopup numberPopup = GetNumberPopup();
        numberPopup.gameObject.transform.position = position;
        numberPopup.Initialize(heal.ToString(), Color.green, null);
    }
}
