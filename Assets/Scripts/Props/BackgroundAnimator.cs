using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundAnimator : MonoBehaviour
{
    public float maxSize = 500;
    public float minSize = 200;
    public float animSpeed = 3;

    SpriteRenderer _sr;
    float _currentSize = 200;
    bool _growing = false;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _growing = true;
        _currentSize = minSize;
    }
    void Update()
    {
        if (_growing)
        {
            _currentSize += Time.deltaTime * animSpeed;
            if (_currentSize > maxSize) { _growing = false; }
        }
        else
        {
            _currentSize -= Time.deltaTime * animSpeed;
            if (_currentSize < minSize) { _growing = true; }
        }
        _sr.size = Vector2.one * _currentSize;
    }
}
