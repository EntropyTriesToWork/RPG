using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{
    public override void Move()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }

        if (!PlayerVisible())
        {
            Vector2 direction = Random.Range(-1f, 1f) > 0 ? new Vector2(-1, 1) : new Vector2(1, 1);

            _rb.AddForce(Vector2.Scale(direction, -Vector2.right) * MS, movementType);
            transform.localScale = direction;
        }
        else
        {
            _rb.AddForce(Vector2.Scale(target.transform.position.x > transform.position.x ? new Vector2(-1, 1) : new Vector2(1, 1), -Vector2.right) * MS, movementType);
            TurnToTarget();
        }
    }
    public override void FixedUpdate()
    {
        if (_hc.IsDead) { return; }
        if (GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }

        Attack();
        EntityOutOfBounds();
    }
}
