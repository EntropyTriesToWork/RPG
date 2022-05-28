using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : BaseEnemy
{
    public override void Move()
    {
        if (!PlayerVisible()) { return; }
        _rb.AddForce(DirectionToTarget * MS, movementType);
    }
    public override void OnTriggerStay2D(Collider2D collision)
    {
        //Doesn't take damage from traps
    }
}
