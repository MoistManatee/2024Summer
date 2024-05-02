using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void Reset();
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject objectToPool;
    [SerializeField] int poolCount = 100;

    List<GameObject> pooledObjects = new();

    private static ObjectPool instance;

    public static ObjectPool GetInstance() => instance;
    int poolIndex;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            GameObject g = Instantiate(objectToPool, Vector3.zero, Quaternion.identity, transform);
            g.SetActive(false);
            pooledObjects.Add(g);
        }
    }

    public GameObject GetPooledObject()
    {
        poolIndex %= poolCount;
        GameObject p = pooledObjects[poolIndex++];
        p.GetComponent<IPoolable>().Reset();
        return p;
    }
}