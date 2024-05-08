using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IPoolable
{
    float lifetime = 5f;
    [SerializeField] public float damage = 80;
    GameObject playerRef;
    Vector2 vector;

    public void SetVectorToPlayer(Vector2 PlayerVector)
    {
        vector = PlayerVector;
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
        transform.position += new Vector3(vector.x, vector.y, 0f) * 2f * Time.deltaTime;
    }
}
