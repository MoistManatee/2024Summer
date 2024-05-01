using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merman : MonoBehaviour
{
    [SerializeField] AudioSource deathSound;
    void Start()
    {
        deathSound = GetComponentInChildren<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        deathSound.transform.parent = null;
        deathSound.Play();
        Destroy(deathSound.gameObject, deathSound.clip.length);
        Destroy(gameObject);
    }
}
