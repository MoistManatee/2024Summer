using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileEnemy : MonoBehaviour, IObserver
{
    [SerializeField] float movespeed;
    [SerializeField] float movespeedMulti = 1;
    [SerializeField] GameObject XP_Crystal;
    [SerializeField] float maxHP = 100;
    [SerializeField] public float HP;

    [SerializeField] public float damage = 20;

    [SerializeField] float projTimer = 3;
    [SerializeField] int projCount = 1;
    float currentProjTimer;

    Rigidbody2D rb;
    private void Awake()
    {
        HP = maxHP;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentProjTimer -= Time.deltaTime;
        if (currentProjTimer <= 0)
        {
            for (int i = 0; i < projCount; i++)
            {
                GameObject ballObj = ObjectPool_EP.GetInstance().GetPooledObject();
                EnemyBullet enemyBullet = ballObj.GetComponent<EnemyBullet>();
                Vector2 vecToPlayer = Player.GetGameObjectInstance().transform.position - transform.position;
                enemyBullet.SetVectorToPlayer(vecToPlayer.normalized);

                ballObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                ballObj.SetActive(true);
            }
            currentProjTimer += projTimer;
        }
    }

    void FixedUpdate()
    {
        Vector2 vector = Vector2.MoveTowards(transform.position, Player.GetGameObjectInstance().transform.position, (movespeed + movespeedMulti) * Time.deltaTime);
        rb.position = new Vector2(vector.x, vector.y);
    }

    void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void TakeDamage(float dmg)
    {
        HP = HP - dmg;
        if (HP <= 0)
        {
            DeathEvent();
        }
    }
    void SetSpeed(float speedMulti)
    {
        movespeedMulti = speedMulti;
    }

    void SetHP(float newHP)
    {
        maxHP = newHP;
        HP = newHP;
    }

    void DeathEvent()
    {
        SoundPlayer.GetInstance().PlayDeathAudio();
        GameObject crystalObj = ObjectPool_Crystal.GetInstance().GetPooledObject();
        crystalObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        crystalObj.SetActive(true);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            Scythe weapon = collision.GetComponent<Scythe>();
            TakeDamage(weapon.damage);
        }
    }
    public void UpdateObserver(ISubject subject)
    {
        SetDamage(damage + (Player.GetInstance().CurrentLevel * 1.2f));
        SetHP(HP + (Player.GetInstance().CurrentLevel * 2));
        SetSpeed(Player.GetInstance().CurrentLevel * 0.2f);
    }
}
