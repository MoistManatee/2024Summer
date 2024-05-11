
using UnityEngine;


public class Merman : MonoBehaviour, IObserver
{
    [SerializeField] float movespeed;
    [SerializeField] float movespeedMulti = 1;
    [SerializeField] GameObject XP_Crystal;
    [SerializeField] float maxHP = 100;
    [SerializeField] float HP;

    [SerializeField] float damage = 20;

    Rigidbody2D rb;

    public float MaxHP { get => maxHP;}
    public float HP1 { get => HP;}
    public float Damage { get => damage;}

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
