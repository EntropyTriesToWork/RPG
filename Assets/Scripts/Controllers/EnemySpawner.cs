using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float baseSpawnCooldown;
    public GameObject enemyPrefab;
    private float _spawnCD;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _spawnCD -= Time.deltaTime;

        if(_spawnCD <= 0f)
        {
            Instantiate(enemyPrefab, (Vector3)Random.insideUnitCircle + Vector3.zero, Quaternion.identity);
            _spawnCD = baseSpawnCooldown;
        }
    }
}
