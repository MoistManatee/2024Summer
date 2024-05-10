using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IObserver
{
    private static Player instance;
    public static Player GetInstance() => instance;

    private static GameObject instanceObj;
    public static GameObject GetGameObjectInstance() => instanceObj;

    [SerializeField] GameOver gameOverScreen;
    [SerializeField] float movespeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTimer = 2;
    [SerializeField] int scytheCount = 3;
    [SerializeField] float scytheDamage = 30;
    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;

    [SerializeField] float maxHP = 100;
    [SerializeField] float HP;

    public Slider xpSlider;
    public Slider hpSlider; 
    public TMP_Text levelText;

    private int currentXP = 0;
    private int maxXp = 100;
    private int currentLevel = 1;
    private void Awake()
    {
        instance = this;
        instanceObj = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.GetComponent<ProjectileEnemy>())
            {
                ProjectileEnemy enemyRanged = collision.GetComponent<ProjectileEnemy>();
                TakeDamage(enemyRanged.damage);
            }
            if (collision.GetComponent<Merman>())
            {
                Merman enemy = collision.GetComponent<Merman>();
                TakeDamage(enemy.damage);
            }
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            EnemyBullet bullet = collision.GetComponent<EnemyBullet>();
            TakeDamage(bullet.damage);
        }
    }

    void TakeDamage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            DeathEvent();
        }
        UpdateUI();
    }

    void DeathEvent()
    {
        gameObject.SetActive(false);
        gameOverScreen.Setup();
    }

    private void Start()
    {
        HP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateUI();
    }

    private void Update()
    {
        currentScytheTimer -= Time.deltaTime;
        if (currentScytheTimer <= 0)
        {
            //Spawn le scythe
            for (int i = 0; i < scytheCount; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360f));

                GameObject scytheObj = ObjectPool_Scythe.GetInstance().GetPooledObject();
                scytheObj.transform.SetPositionAndRotation(transform.position, rot);
                float newScale = currentLevel * 0.1f;
                scytheObj.transform.localScale = (new Vector3(1f + newScale, 1f + newScale, 1f + newScale));

                Scythe scytheClass = scytheObj.GetComponent<Scythe>();
                scytheClass.SetDamage(scytheDamage + (currentLevel * 2f));

                scytheObj.SetActive(true);
            }
            currentScytheTimer += scytheTimer;
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(x, y) * movespeed;
        //animator.SetFloat("Speed", rb.velocity.magnitude);

        if (x != 0)
        {
            int a;
            if (x > 0)
            {
                a = 1;
            }
            else
            {
                a = -1;
            }
            transform.localScale = new Vector3(a, 1, 1);

            int b = x > 0 ? 1 : -1;
            transform.localScale = new Vector3(b, 1, 1);

            transform.localScale = new Vector3(x > 0 ? 1 : -1, 1, 1);
        }
    }


    public int Foo()
    {
        return 5;
    }

    public int Foo2() => 5;

    public void GainXP(int amount)
    {
        currentXP += amount;
        UpdateUI();

        if (currentXP >= maxXp)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        if (scytheTimer >= 0)
        {
            scytheTimer = scytheTimer - (currentLevel * 0.01f);
            if (scytheTimer < 0.8)
            {
                scytheTimer = 0.8f;
            }
        }
        scytheCount += (int)Mathf.Round((currentLevel / 4));
        currentLevel++;
        currentXP = 0;
        maxXp = CalculateMaxXPForNextLevel();
        UpdateUI();
    }
    private int CalculateMaxXPForNextLevel()
    {
        return currentLevel * 100;
    }
    public void SetXP(float xpNormalized)
    {
        xpSlider.value = xpNormalized;
    }

    public void SetHP(float hpNormalized)
    {
        hpSlider.value = hpNormalized;
    }

    public void SetLevel(int level)
    {
        levelText.text = "Level: " + level.ToString();
    }

    private void UpdateUI()
    {
        SetXP((float)currentXP / maxXp);
        SetHP(HP / maxHP);
        SetLevel(currentLevel);
    }

    public int GetPlayerLevel()
    {
        return currentLevel;
    }

    public void UpdateObserver(ISubject subject, int XP)
    {
        GainXP(XP);
        UpdateUI();
    }
}

