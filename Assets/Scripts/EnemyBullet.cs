using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IPoolable
{
    float lifetime = 5f;
    [SerializeField] public float damage = 80;
    Vector3 direction;

    public void SetVectorToPlayer(Vector2 PlayerVector)
    {
        direction = PlayerVector;
    }


    public void Reset()
    {
        lifetime = 5f;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);
        }
        transform.position += direction * 2f * Time.deltaTime;
    }
}
