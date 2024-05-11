using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// intermediaire proxy
public class EnemyWeakpoint : Enemy
{
    public Enemy original;

  
    public override void Damage(int value)
    {
        original.Damage(value * 10);
    }
}