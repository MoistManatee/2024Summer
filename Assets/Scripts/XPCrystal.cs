using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class XPCrystal : MonoBehaviour, IPoolable
{
    float lifetime = 60f;
    [SerializeField] int XPValue = 50;
    public void Reset()
    {
        lifetime = 60f;
    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Player.GetInstance().GainXP(XPValue);
        }
    }
}
