using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ISubject
{
    private static Player instance;
    public static Player GetInstance() => instance;

    private static GameObject instanceObj;
    public static GameObject GetGameObjectInstance() => instanceObj;

    [SerializeField] GameOver gameOverComponent;
    [SerializeField] PlayerUI playerUIComponent;

    [SerializeField] EnemySpawner enemySpawnerComponent1;
    [SerializeField] EnemySpawner enemySpawnerComponent2;
    [SerializeField] RangedEnemySpawner rangedEnemySpawnerComponent;

    [SerializeField] float movespeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTimer = 2;
    [SerializeField] int scytheCount = 3;
    [SerializeField] float scytheDamage = 30;
    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;

    

    public Slider xpSlider;
    public Slider hpSlider; 
    public TMP_Text levelText;

    [SerializeField] float maxHP = 100;
    [SerializeField] float hp;
    bool isDead = false;

    int currentXP = 0;
    int maxXp = 100;
    int currentLevel = 1;
    private void Awake()
    {
        instance = this;
        instanceObj = gameObject;
        Attach(gameOverComponent);
        Attach(playerUIComponent);
        Attach(enemySpawnerComponent1);
        Attach(enemySpawnerComponent2);
        Attach(rangedEnemySpawnerComponent);
    }

    [SerializeField] private List<IObserver> _observers = new List<IObserver>();

    public float MaxHP { get => maxHP;}
    public float HP { get => hp; }
    public int CurrentXP { get => currentXP;}
    public int MaxXp { get => maxXp;}
    public int CurrentLevel { get => currentLevel;}
    public bool IsDead { get => isDead;}

    // The subscription management methods.
    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
    }

    // Trigger an update in each subscriber.
    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateObserver(this);
        }
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
                TakeDamage(enemy.Damage);
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
        hp -= dmg;
        if (hp <= 0)
        {
            DeathEvent();
        }
        Notify();
    }

    void DeathEvent()
    {
        isDead = true;
        Notify();
    }

    private void Start()
    {
        hp = MaxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Notify();
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
                float newScale = CurrentLevel * 0.1f;
                scytheObj.transform.localScale = (new Vector3(1f + newScale, 1f + newScale, 1f + newScale));

                Scythe scytheClass = scytheObj.GetComponent<Scythe>();
                scytheClass.SetDamage(scytheDamage + (CurrentLevel * 0.5f));

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

    public void GainXP(int amount)
    {
        currentXP += amount;
        Notify();

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
        Notify();
    }
    private int CalculateMaxXPForNextLevel()
    {
        return currentLevel * 100;
    }
}


public interface ISubject
{
    // Attach an observer to the subject.
    void Attach(IObserver observer);

    // Detach an observer from the subject.
    void Detach(IObserver observer);

    // Notify all observers about an event.
    void Notify();
}
public interface IObserver
{
    // Receive update from subject
    void UpdateObserver(ISubject subject);
}

