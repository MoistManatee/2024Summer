using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject playerInstance;

    [SerializeField] Camera Camera;
    [SerializeField] int SpawnWaveSize = 5;
    [SerializeField] int Cooldown = 5;
    private void Start()
    {

        StartCoroutine(SpawnEnemy());
    }

    // Une coroutine est une méthode qui peut inclure des délais de temps
    public IEnumerator SpawnEnemy()
    {
        /*
        Vector3 BLeft = new Vector3(0f, 0f, 0f);
        Vector3 BRight = new Vector3(1f, 0f, 0f);
        Vector3 TLeft = new Vector3(0f, 1f, 0f);
        Vector3 TRight = new Vector3(1f, 1f, 0f);
        */
        Vector3[] arr = new Vector3[4];

        while (true)
        {
            Player player = playerInstance.GetComponent<Player>();
            SpawnWaveSize += (player.GetPlayerLevel() * 2);
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

                int X = Random.Range(0, 3);

                Vector3 p = Camera.ViewportToWorldPoint(arr[X]);
                GameObject prefabTemp = Instantiate(enemyPrefab, new Vector3(p.x, p.y, 0f), Quaternion.identity);
                EnemyProjectile enemy = prefabTemp.GetComponentInChildren<EnemyProjectile>();
                enemy.SetDamage(enemy.damage + (player.GetPlayerLevel() * 1.2f));
                enemy.SetHP(enemy.HP + (player.GetPlayerLevel() * 2));
                enemy.SetPlayerInstance(playerInstance);
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}
