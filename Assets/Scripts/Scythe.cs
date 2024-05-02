using UnityEngine;

public class Scythe : MonoBehaviour, IPoolable
{
    float lifetime = 2f;

    public void Reset()
    {
        lifetime = 2f;
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