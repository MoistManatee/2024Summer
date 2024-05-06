using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IObserver
{
    [SerializeField] float movespeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTimer = 2;
    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;

    public Slider xpSlider;
    public TMP_Text levelText;

    private int currentXP = 0;
    private int maxXp = 100;
    private int currentLevel = 1;

    private void Start()
    {
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
            for (int i = 0; i < 3; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360f));

                //Instantiate(scythePrefab, transform.position, rot);
                GameObject scythe = ObjectPool.GetInstance().GetPooledObject();
                scythe.transform.SetPositionAndRotation(transform.position, rot);
                scythe.SetActive(true);
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

    public void SetLevel(int level)
    {
        levelText.text = "Level: " + level.ToString();
    }

    private void UpdateUI()
    {
        SetXP((float)currentXP / maxXp);
        SetLevel(currentLevel);
    }

    public void UpdateObserver(ISubject subject, int XP)
    {
        GainXP(XP);
        UpdateUI();
    }
}

