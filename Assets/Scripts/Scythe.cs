using UnityEngine;

public class Scythe : MonoBehaviour, IPoolable
{
    float lifetime = 2f;
    [SerializeField] public float damage = 30;

    public void Reset()
    {
        lifetime = 2f;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);
        }
        transform.position += transform.right * 5f * Time.deltaTime;
    }
}