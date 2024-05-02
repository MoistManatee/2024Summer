using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movespeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTimer = 2;
    float currentScytheTimer;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentScytheTimer -= Time.deltaTime;
        if (currentScytheTimer <= 0)
        {
            //Spawn le scythe
            for (int i = 0; i < 3; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360f));

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
}