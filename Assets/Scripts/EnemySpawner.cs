using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISubject, IObserver
{
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Camera Camera;
    [SerializeField] int SpawnWaveSize = 5;
    [SerializeField] int Cooldown = 5;

    [SerializeField] private List<IObserver> _observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
    }

    // Trigger an update in each subscriber.
    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateObserver(this);
        }
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    public void UpdateObserver(ISubject subject)
    {
        SpawnWaveSize = (Player.GetInstance().CurrentLevel * 2);
    }
    public IEnumerator SpawnEnemy()
    {
        Vector3[] arr = new Vector3[4];

        while (true)
        {
            for (int i = 0; i < SpawnWaveSize; i++)
            {
                float num = Random.Range(0f, 1f);
                Vector3 Bottom = new Vector3(num, -0.1f, 0f);
                num = Random.Range(0f, 1f);
                Vector3 Top = new Vector3(num, 1.1f, 0f);
                num = Random.Range(0f, 1f);
                Vector3 Left = new Vector3(-0.1f, num, 0f);
                num = Random.Range(0f, 1f);
                Vector3 Right = new Vector3(1.1f, num, 0f);

                arr[0] = Bottom;
                arr[1] = Top;
                arr[2] = Left;
                arr[3] = Right;

                int X = Random.Range(0,3);

                Vector3 p = Camera.ViewportToWorldPoint(arr[X]);
                GameObject spawnedEnemy = Instantiate(enemyPrefab, new Vector3(p.x, p.y, 0f), Quaternion.identity);
                Attach(spawnedEnemy.GetComponent<Merman>());
                Notify();
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}