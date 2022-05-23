using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public AnimationCurve spawnCooldown;
    public GameObject enemyPrefab;
    private float _spawnCD;

    public List<Transform> spawnPoints;
    public PlayerController player;

    // Update is called once per frame
    void Update()
    {
        _spawnCD -= Time.deltaTime;

        if (_spawnCD <= 0f)
        {
            Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);
            _spawnCD = spawnCooldown.Evaluate(GameManager.Instance.GameTime);
        }
    }
    public Vector2 GetSpawnPosition()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (Vector2.Distance(player.transform.position, spawnPoints[i].position) > 2) { return spawnPoints[i].position; }
        }
        return Vector2.zero;
    }
}
