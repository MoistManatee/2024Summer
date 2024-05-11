using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Enemy
{
    [SerializeField] GameObject weakPointPrefab;
    private EnemyWeakpoint weakpoint;
    private LineRenderer lineRenderer;

    private void Start()
    {
        weakpoint = Instantiate(weakPointPrefab, transform.position,
            Quaternion.identity).GetComponent<EnemyWeakpoint>();
        weakpoint.original = this;
       
    }

   
    public override void Die()
    {
        weakpoint.Die();
        base.Die();
    }

}