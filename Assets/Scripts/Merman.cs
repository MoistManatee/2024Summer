using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Merman : MonoBehaviour
{
    [SerializeField] float movespeed;
    [SerializeField] GameObject XP_Crystal;
    [SerializeField] float maxHP = 100;
    [SerializeField] public float HP;

    [SerializeField] public float damage = 20;

    Rigidbody2D rb;
    private void Awake()
    {
        HP = maxHP;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 vector = Vector2.MoveTowards(transform.position, Player.GetGameObjectInstance().transform.position, movespeed * Time.deltaTime);
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
        //XPCrystal crystalIns = crystalObj.GetComponent<XPCrystal>();
        //Player player = playerInstance.GetComponent<Player>();
        //crystalIns.Attach(player);

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
