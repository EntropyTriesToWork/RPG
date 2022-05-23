using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Rainbowizer : MonoBehaviour
{
    public Gradient gradient;
    public SpriteRenderer spriteRenderer;
    public float speed = 1;
    bool _increasing;
    [ReadOnly] [SerializeField] float _value;
    private void Start()
    {
        _increasing = true;
    }
    void Update()
    {
        if (_increasing)
        {
            _value += Time.deltaTime * speed;
            if (_value >= 1f) { _increasing = false; }
        }
        else
        {
            _value -= Time.deltaTime * speed;
            if (_value <= 0f) { _increasing = true; }
        }
        spriteRenderer.color = gradient.Evaluate(_value);
    }
}
