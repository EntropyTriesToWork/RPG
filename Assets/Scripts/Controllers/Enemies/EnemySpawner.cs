using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            ShuffleSpawnPoints();
            Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);
            _spawnCD = spawnCooldown.Evaluate(GameManager.Instance.GameTime);
        }
    }
    public Vector2 GetSpawnPosition()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            float dist = Vector2.Distance(player.transform.position, spawnPoints[i].position);
            if (dist > 3f && dist < 10f) { return spawnPoints[i].position; }
        }
        return Vector2.zero;
    }

    public void ShuffleSpawnPoints()
    {
        System.Random random = new System.Random();
        spawnPoints = spawnPoints.OrderBy(a => random.Next()).ToList();
    }
}
