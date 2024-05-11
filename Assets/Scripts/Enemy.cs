using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] public float moveSpeed = 0.5f;
    [SerializeField] int maxHp = 10;
    Rigidbody2D rb;
    int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Damage(int value)
    {
        currentHp -= value;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Attack() { }

    public virtual void Die()
    {
        SoundPlayer.GetInstance().PlayDeathAudio();
        int experiencePoints= 10;
        Player.GetInstance().GainXP(experiencePoints);
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        // direction (vecteur) = destination - source
        var direction = Player.GetInstance().transform.position - transform.position;
        rb.velocity = direction.normalized * moveSpeed;
    }
}