using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyHitter
{
    void HandleHitOnEnemy(GameObject hitObject, Collider2D collider);
}
