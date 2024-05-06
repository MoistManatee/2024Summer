using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Merman : MonoBehaviour
{
    [SerializeField] float movespeed;
    GameObject playerInstance;
    [SerializeField] GameObject XP_Crystal;
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
        if (collision.gameObject.CompareTag("Weapon"))
        {
            SoundPlayer.GetInstance().PlayDeathAudio();
            GameObject instance = Instantiate(XP_Crystal, transform.position, Quaternion.identity);
            XPCrystal crystalIns = instance.GetComponent<XPCrystal>();
            Player player = playerInstance.GetComponent<Player>();
            crystalIns.Attach(player);
            Destroy(gameObject);
        }
    }
}
