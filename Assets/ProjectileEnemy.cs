using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float movespeed;
    GameObject playerInstance;
    [SerializeField] GameObject XP_Crystal;
    [SerializeField] float maxHP = 100;
    [SerializeField] public float HP;

    [SerializeField] public float damage = 20;

    [SerializeField] float projTimer = 3;
    [SerializeField] int projCount = 1;
    [SerializeField] float projDamage = 30;
    float currentProjTimer;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = maxHP;
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
                Vector2 vecToPlayer = Vector2.MoveTowards(transform.position, playerInstance.transform.position, 0f);
                enemyBullet.SetVectorToPlayer(vecToPlayer);

                ballObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                ballObj.SetActive(true);
            }
            currentProjTimer += projTimer;
        }
    }

    public void SetPlayerInstance(GameObject prefab)
    {
        playerInstance = prefab;
    }

    void FixedUpdate()
    {
        Vector2 vector = Vector2.MoveTowards(transform.position, playerInstance.transform.position, movespeed * Time.deltaTime);
        rb.position = new Vector2(vector.x, vector.y);
    }

    public void SetDamage(float dmg)
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

    public void SetHP(float newHP)
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
        XPCrystal crystalIns = crystalObj.GetComponent<XPCrystal>();
        Player player = playerInstance.GetComponent<Player>();
        crystalIns.Attach(player);

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
}
