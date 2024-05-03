using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Merman : MonoBehaviour
{
    [SerializeField] float movespeed;
    GameObject playerInstance;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundPlayer.GetInstance().PlayDeathAudio();
        Destroy(gameObject);
    }
}
